using FluentValidation.Results;
using Raven.Client.Documents;
using System;
using System.Threading;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Validation;
using zero.Core.Extensions;

namespace zero.Core.Identity
{
  public class BackofficeUserStore<TUser, TRole>
    where TUser : IBackofficeUser, new()
    where TRole : IBackofficeUserRole, new()
  {
    /// <summary>
    /// Gets the database context for this store.
    /// </summary>
    protected IDocumentStore Raven { get; set; }

    /// <summary>
    /// Configuration
    /// </summary>
    protected IZeroConfiguration Config { get; private set; }

    /// <summary>
    /// Currently logged-in user
    /// </summary>
    protected IBackofficeUser Current { get; private set; }


    public BackofficeUserStore(IDocumentStore raven, IZeroConfiguration config, IBackofficeUser currentUser)
    {
      Raven = raven;
      Config = config;
    }


    /// <summary>
    /// Adds a new backoffice user
    /// </summary>
    public async Task<EntityChangeResult<TUser>> Add(TUser model, CancellationToken token = default)
    {
      // set default language
      if (model.LanguageId.IsNullOrEmpty())
      {
        model.LanguageId = Config.DefaultLanguage;
      }

      // validate model
      ValidationResult validation = await new BackofficeUserValidator(isCreate: true).ValidateAsync(model);

      if (!validation.IsValid)
      {
        return EntityChangeResult<TUser>.Fail(validation);
      }

      return EntityChangeResult<TUser>.Success();
    }
  }
}
