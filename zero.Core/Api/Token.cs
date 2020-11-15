using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Database;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Options;

namespace zero.Core.Api
{
  public class Token : IToken
  {
    protected IZeroStore Store { get; private set; }

    protected IZeroOptions Options { get; private set; }

    private const string PREFIX = "changeTokens";


    public Token(IZeroStore store, IZeroOptions options)
    {
      Store = store;
      Options = options;
    }


    /// <inheritdoc />
    public bool Verify(IZeroIdEntity entity, string token)
    {
      return Verify(entity?.Id, token);
    }


    /// <inheritdoc />
    public bool Verify(string entityId, string token)
    {
      if (token.IsNullOrWhiteSpace() && entityId.IsNullOrEmpty())
      {
        return true;
      }
      
      if (token.IsNullOrWhiteSpace() || entityId.IsNullOrEmpty())
      {
        return false;
      }

      using (IDocumentSession session = Store.OpenSession())
      {
        return session.Query<ChangeToken>().Any(x => x.Id == token && x.ReferenceId == entityId);
      }
    }


    /// <inheritdoc />
    public string Get(IZeroIdEntity entity)
    {
      return Get(entity?.Id);
    }


    /// <inheritdoc />
    public string Get(string entityId)
    {
      if (entityId.IsNullOrEmpty())
      {
        return null;
      }

      ChangeToken token = new ChangeToken()
      {
        Id = Options.Raven.CollectionPrefix.EnsureEndsWith(Store.RavenStore.Conventions.IdentityPartsSeparator) + PREFIX.EnsureEndsWith(Store.RavenStore.Conventions.IdentityPartsSeparator) + Guid.NewGuid(),
        ReferenceId = entityId
      };

      using (IDocumentSession session = Store.OpenSession())
      {
        session.Store(token);
        session.Advanced.GetMetadataFor(token)[Constants.Database.Expires] = DateTime.UtcNow.AddMinutes(Options.TokenExpiration);
        session.SaveChanges();
      }

      return token.Id;
    }
  }


  public interface IToken
  {
    /// <summary>
    /// Verifies if the change token is valid for the entity
    /// </summary>
    bool Verify(IZeroIdEntity entity, string token);

    /// <summary>
    /// Verifies if the change token is valid for the entity
    /// </summary>
    bool Verify(string entityId, string token);

    /// <summary>
    /// Get a new change token for the entity
    /// </summary>
    string Get(IZeroIdEntity entity);

    /// <summary>
    /// Get a new change token for the entity
    /// </summary>
    string Get(string entityId);
  }
}
