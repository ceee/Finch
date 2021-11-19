namespace zero.Core;

using FluentValidation.Results;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Utils;

public abstract partial class EntityCollection<T> : IEntityCollection<T> where T : ZeroIdEntity
{
  /// <inheritdoc />
  public virtual async Task<EntityResult<T>> Save(T model)
  {
    if (model == null)
    {
      return EntityResult<T>.Fail("@errors.onsave.empty");
    }

    bool isUpdate = model.Id.IsNullOrEmpty() ? false : await Session.Advanced.ExistsAsync(model.Id);

    // set IDs
    model.AutoSetIds();

    // get current user
    string userId = Context.BackofficeUser.FindFirstValue(Constants.Auth.Claims.UserId);

    if (model is ZeroEntity)
    {
      ZeroEntity zeroModel = model as ZeroEntity;

      // set default properties
      if (!isUpdate)
      {
        zeroModel.CreatedDate = DateTimeOffset.Now;
        zeroModel.CreatedById = userId;
        zeroModel.LanguageId ??= "languages.1-A"; // TODO correct language id
      }

      // update name alias and last modified
      zeroModel.Alias = Safenames.Alias(zeroModel.Name);
      zeroModel.LastModifiedById = userId;
      zeroModel.LastModifiedDate = DateTimeOffset.Now;
      zeroModel.CreatedById ??= userId;
      zeroModel.Hash ??= IdGenerator.Classic();
    }

    // run validator
    ValidationResult validation = await Validate(model);
    if (!validation.IsValid)
    {
      return EntityResult<T>.Fail(validation);
    }

    // create ID before-hand so interceptors can use it
    if (!isUpdate)
    {
      model.Id = await Session.Advanced.DocumentStore.Conventions.GenerateDocumentIdAsync(Session.Advanced.DocumentStore.Database, model);
    }

    // run interceptor
    InterceptorInstruction<T> saveInstruction = Interceptors.CreateInstruction(this, InterceptorType.Save, model);
    InterceptorInstruction<T> instruction = Interceptors.CreateInstruction(this, isUpdate ? InterceptorType.Update : InterceptorType.Create, model);

    if (!await saveInstruction.Run())
    {
      return saveInstruction.Result;
    }
    if (!await instruction.Run())
    {
      return instruction.Result;
    }

    // store our model
    await Session.StoreAsync(model);

    // run after-handlers for interceptors
    await saveInstruction.Complete();
    await instruction.Complete();

    await Session.SaveChangesAsync();

    return EntityResult<T>.Success(model);
  }
}