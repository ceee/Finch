using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Options;

namespace zero.Core.Api
{
  public class Token : IToken
  {
    protected IDocumentStore Raven { get; private set; }

    protected IZeroOptions Options { get; private set; }

    private const string PREFIX = "changeTokens";


    public Token(IDocumentStore raven, IZeroOptions options)
    {
      Raven = raven;
      Options = options;
    }


    /// <inheritdoc />
    public async Task<bool> Verify(IZeroEntity entity, string token)
    {
      return await Verify(entity?.Id, token);
    }


    /// <inheritdoc />
    public async Task<bool> Verify(string entityId, string token)
    {
      if (token.IsNullOrWhiteSpace() && entityId.IsNullOrEmpty())
      {
        return true;
      }
      
      if (token.IsNullOrWhiteSpace() || entityId.IsNullOrEmpty())
      {
        return false;
      }

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<ChangeToken>().AnyAsync(x => x.Id == token && x.ReferenceId == entityId);
      }
    }


    /// <inheritdoc />
    public async Task<string> Get(IZeroEntity entity)
    {
      return await Get(entity?.Id);
    }


    /// <inheritdoc />
    public async Task<string> Get(string entityId)
    {
      if (entityId.IsNullOrEmpty())
      {
        return null;
      }

      ChangeToken token = new ChangeToken()
      {
        Id = Options.Raven.CollectionPrefix.EnsureEndsWith(Raven.Conventions.IdentityPartsSeparator) + PREFIX.EnsureEndsWith(Raven.Conventions.IdentityPartsSeparator) + Guid.NewGuid(),
        ReferenceId = entityId
      };

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        await session.StoreAsync(token);
        session.Advanced.GetMetadataFor(token)[Constants.Database.Expires] = DateTime.UtcNow.AddMinutes(Options.TokenExpiration);
        await session.SaveChangesAsync();
      }

      return token.Id;
    }
  }


  public interface IToken
  {
    /// <summary>
    /// Verifies if the change token is valid for the entity
    /// </summary>
    Task<bool> Verify(IZeroEntity entity, string token);

    /// <summary>
    /// Verifies if the change token is valid for the entity
    /// </summary>
    Task<bool> Verify(string entityId, string token);

    /// <summary>
    /// Get a new change token for the entity
    /// </summary>
    Task<string> Get(IZeroEntity entity);

    /// <summary>
    /// Get a new change token for the entity
    /// </summary>
    Task<string> Get(string entityId);
  }
}
