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
  public partial class UserStore : IUserStore<IUser>, IQueryableUserStore<IUser>
  {
    protected IDocumentStore Raven { get; private set; }

    public UserStore(IDocumentStore raven)
    {
      Raven = raven;
    }


    /// <inheritdoc />
    public IQueryable<IUser> Users
    {
      get
      {
        using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
        {
          return session.Query<IUser>();
        }
      }
    }


    /// <inheritdoc />
    public async Task<IdentityResult> CreateAsync(IUser user, CancellationToken cancellationToken)
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
    public async Task<IdentityResult> DeleteAsync(IUser user, CancellationToken cancellationToken)
    {
      // Remove the cluster-wide compare/exchange key.
      await Raven.RemoveReservationAsync(Constants.Database.ReservationPrefix + user.Email);

      // Delete the user and save it. We must save it because deleting is a cluster-wide operation.
      // Only if the deletion succeeds will we remove the cluseter-wide compare/exchange key.
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        session.Delete(user);
        await session.SaveChangesAsync(cancellationToken);
      }

      return IdentityResult.Success;
    }


    /// <inheritdoc />
    public Task<IUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return session.LoadAsync<IUser>(userId);
      }
    }


    /// <inheritdoc />
    public async Task<IUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<IUser>().FirstOrDefaultAsync(x => x.Name == normalizedUserName, cancellationToken);
      }
    }


    /// <inheritdoc />
    public Task<string> GetNormalizedUserNameAsync(IUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.Name);
    }


    /// <inheritdoc />
    public Task<string> GetUserIdAsync(IUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.Id);
    }


    /// <inheritdoc />
    public Task<string> GetUserNameAsync(IUser user, CancellationToken cancellationToken)
    {
      return Task.FromResult(user.Name);
    }


    /// <inheritdoc />
    public async Task SetNormalizedUserNameAsync(IUser user, string normalizedName, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        user.Name = normalizedName.ToLowerInvariant();
        await session.StoreAsync(user, cancellationToken);
        await session.SaveChangesAsync(cancellationToken);
      }
    }


    /// <inheritdoc />
    public async Task SetUserNameAsync(IUser user, string userName, CancellationToken cancellationToken)
    {
      await SetNormalizedUserNameAsync(user, userName, cancellationToken);
    }


    /// <inheritdoc />
    public async Task<IdentityResult> UpdateAsync(IUser user, CancellationToken cancellationToken)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        IUser source = await FindByIdAsync(user.Id, cancellationToken);

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
