using Microsoft.AspNetCore.Identity;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;

namespace zero.Core.Identity
{
  public partial class RavenUserStore<TUser, TRole> : RavenUserStore<TUser>
    where TUser : class, IIdentityUserWithRoles
    where TRole : class, IIdentityUserRole
  {
    public RavenUserStore(IDocumentStore raven) : base(raven) { }
  }



  public partial class RavenUserStore<TUser> : IUserStore<TUser>, IQueryableUserStore<TUser> where TUser : class, IIdentityUser
  {
    protected IDocumentStore Raven { get; private set; }

    public RavenUserStore(IDocumentStore raven)
    {
      Raven = raven;
    }


    /// <inheritdoc />
    public IQueryable<TUser> Users
    {
      get
      {
        using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
        {
          return session.Query<TUser>();
        }
      }
    }


    /// <inheritdoc />
    public async Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        // try to reserve the key for the new user
        if (!await Raven.ReserveAsync(Constants.Database.ReservationPrefix + user.Email, user.Id))
        {
          return IdentityResult.Failed(new[]
          {
            new IdentityError
            {
              Code = "DuplicateEmail",
              Description = $"The email address {user.Email} is already taken."
            }
          });
        }

        // This model allows us to lookup a user by name in order to get the id
        await session.StoreAsync(user, cancellationToken);

        // Because this a a cluster-wide operation due to compare/exchange tokens,
        // we need to save changes here; if we can't store the user, 
        // we need to roll back the email reservation.
        try
        {
          await session.SaveChangesAsync(cancellationToken);
        }
        catch (Exception)
        {
          // The compare/exchange email reservation is cluster-wide, outside of the session scope. 
          // We need to manually roll it back.
          await Raven.RemoveReservationAsync(Constants.Database.ReservationPrefix + user.Email);
          throw;
        }
      }

      return IdentityResult.Success;
    }


    /// <inheritdoc />
    public async Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken)
    {
      // Remove the cluster-wide compare/exchange key.
      await Raven.RemoveReservationAsync(Constants.Database.ReservationPrefix + user.Email);

      // Delete the user and save it. We must save it because deleting is a cluster-wide operation.
      // Only if the deletion succeeds will we remove the cluseter-wide compare/exchange key.
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        TUser source = await session.LoadAsync<TUser>(user.Id, cancellationToken);

        session.Delete(source);
        await session.SaveChangesAsync(cancellationToken);
      }

      return IdentityResult.Success;
    }


    /// <inheritdoc />
    public async Task<TUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.LoadAsync<TUser>(userId);
      }
    }


    /// <inheritdoc />
    public async Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<TUser>().FirstOrDefaultAsync(x => x.Username == normalizedUserName, cancellationToken);
      }
    }


    /// <inheritdoc />
    public Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.Username);
    }


    /// <inheritdoc />
    public Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.Id);
    }


    /// <inheritdoc />
    public Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.Username);
    }


    /// <inheritdoc />
    public Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken)
    {
      user.Username = normalizedName.ToLowerInvariant();
      return Task.CompletedTask;
    }


    /// <inheritdoc />
    public async Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken)
    {
      await SetNormalizedUserNameAsync(user, userName, cancellationToken);
    }


    /// <inheritdoc />
    public async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        TUser source = await session.LoadAsync<TUser>(user.Id, cancellationToken);

        if (source == null)
        {
          return IdentityResult.Failed(new[]
          {
            new IdentityError
            {
              Code = "UserNotFound",
              Description = $"Could not find stored user with id {user.Id}."
            }
          });
        }

        if (source.Email != user.Email)
        {
          // try to reserve the key for the new user
          if (!await Raven.ReserveAsync(Constants.Database.ReservationPrefix + user.Email, user.Id))
          {
            return IdentityResult.Failed(new[]
            {
              new IdentityError
              {
                Code = "DuplicateEmail",
                Description = $"The email address {user.Email} is already taken."
              }
            });
          }

          // Remove the cluster-wide compare/exchange key.
          await Raven.RemoveReservationAsync(Constants.Database.ReservationPrefix + source.Email);
        }
      }

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        await session.StoreAsync(user, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
      }

      return IdentityResult.Success;
    }


    /// <inheritdoc />
    void IDisposable.Dispose()
    {
      
    }
  }
}
