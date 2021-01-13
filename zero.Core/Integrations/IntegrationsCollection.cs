using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Collections;
using zero.Core.Options;

namespace zero.Core.Integrations
{
  public class IntegrationsCollection : FixedCollectionBase<Integration>, IIntegrationsCollection
  {
    public IntegrationsCollection(IZeroContext context, ICollectionInterceptorHandler interceptorHandler) : base(context, interceptorHandler) { }


    protected override IEnumerable<OptionsType> GetDefinedTypes() => Context.Options.Integrations.GetAllItems();


    /// <inheritdoc />
    public async Task<bool> Any(string tag)
    {
      IEnumerable<IntegrationType> types = GetDefinedTypes().Cast<IntegrationType>().Where(x => x.Tags.Contains(tag, StringComparer.InvariantCultureIgnoreCase));

      if (!types.Any())
      {
        return false;
      }

      if (types.Any(x => x.IsAutoActivated))
      {
        return true;
      }

      string[] aliases = types.Select(x => x.Alias).ToArray();
      return await Session.Query<IIntegration>().AnyAsync(x => x.TypeAlias.In(aliases) && x.IsActive);
    }


    ///// <inheritdoc />
    //public async Task<T> Get<T>() where T : class, IIntegrationModel
    //{
    //  Type type = typeof(T);
    //  IIntegration integration = RegisteredTypes.FirstOrDefault(x => x.ModelType == type);

    //  if (integration == null)
    //  {
    //    return default;
    //  }

    //  await Preload();
    //  return Items.FirstOrDefault(x => x.IntegrationAlias == integration.Alias) as T;
    //}


    ///// <inheritdoc />
    //public async Task<IIntegrationModel> GetByAlias(string alias)
    //{
    //  IIntegration integration = RegisteredTypes.FirstOrDefault(x => x.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));

    //  if (integration == null)
    //  {
    //    return default;
    //  }

    //  await Preload();
    //  return Items.FirstOrDefault(x => x.IntegrationAlias == integration.Alias);
    //}


    ///// <inheritdoc />
    //public async Task<T> GetById<T>(string id) where T : class, IIntegrationModel
    //{
    //  return await GetById(id) as T;
    //}
  }


  public interface IIntegrationsCollection : ICollectionBase<Integration>
  {
    ///// <summary>
    ///// Get settings for a specific integration
    ///// </summary>
    //Task<T> Get<T>() where T : class, IIntegrationModel;

    ///// <summary>
    ///// Get settings for a specific integration
    ///// </summary>
    //Task<IIntegrationModel> GetByAlias(string alias);

    ///// <summary>
    ///// Get settings for a specific integration
    ///// </summary>
    //Task<T> GetById<T>(string id) where T : class, IIntegrationModel;
  }
}
