using FluentValidation.Results;
using Microsoft.Extensions.Options;
using Raven.Client.Documents;
using Raven.Client.Documents.Linq;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Options;
using zero.Core.Plugins;
using zero.Core.Renderer;

namespace zero.Core.Api
{
  public class SpacesApi : ISpacesApi
  {
    protected IDocumentStore Raven { get; private set; }

    protected IPermissionsApi PermissionsApi { get; private set; }

    protected IZeroOptions Options { get; set; }


    public SpacesApi(IDocumentStore raven, IPermissionsApi permissionsApi, IZeroOptions options)
    {
      Raven = raven;
      PermissionsApi = permissionsApi;
      Options = options;
    }


    /// <inheritdoc />
    public Space GetByAlias(string alias)
    {
      return Options.Spaces.GetAllItems().FirstOrDefault(x => x.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));
    }


    /// <inheritdoc />
    public IReadOnlyCollection<Space> GetAll()
    {
      return Options.Spaces.GetAllItems();
    }


    /// <inheritdoc />
    public RendererConfig GetEditorConfig(string alias)
    {
      Space space = GetByAlias(alias);

      if (space == null)
      {
        return null;
      }

      AbstractGenericRenderer renderer = Options.Renderers.GetAllItems().FirstOrDefault(x => x.TargetType == space.Type);

      if (renderer == null)
      {
        return null;
      }

      return renderer.Build();
    }


    /// <inheritdoc />
    public async Task<T> GetItem<T>(string alias) where T : SpaceContent
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<SpaceContent>()
          .Where(x => x.SpaceAlias == alias)
          .ProjectInto<T>()
          .FirstOrDefaultAsync();
      }
    }


    /// <inheritdoc />
    public async Task<T> GetItem<T>(string alias, string id) where T : SpaceContent
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<SpaceContent>()
          .Where(x => x.SpaceAlias == alias && x.Id == id)
          .ProjectInto<T>()
          .FirstOrDefaultAsync();
      }
    }


    /// <inheritdoc />
    public async Task<IList<T>> GetList<T>(string alias) where T : SpaceContent
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<SpaceContent>()
          .Where(x => x.SpaceAlias == alias)
          .ProjectInto<T>()
          .ToListAsync();
      }
    }


    /// <inheritdoc />
    public async Task<ListResult<T>> GetListByQuery<T>(string alias, ListQuery<T> query, string appId = null) where T : SpaceContent
    {
      query.SearchSelector = item => item.Name;

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        return await session.Query<T>()
          .ForApp(appId)
          .Where(x => x.SpaceAlias == alias)
          .ToQueriedListAsync(query);
      }
    }


    /// <inheritdoc />
    public async Task<EntityResult<T>> Save<T>(string alias, T model) where T : SpaceContent
    {
      Space space = GetByAlias(alias);
      RendererConfig config = GetEditorConfig(alias); 

      if (config.Validator != null)
      {
        ValidationResult validation = await config.Validator.ValidateAsync(model);

        if (!validation.IsValid)
        {
          return EntityResult<T>.Fail(validation);
        }
      }

      if (model.Id.IsNullOrEmpty())
      {
        model.AppId = "zero.applications.1-A"; // TODO real app id
        model.CreatedDate = DateTimeOffset.Now;
      }

      model.SpaceAlias = alias;
      model.Alias = Alias.Generate(model.Name);

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        await session.StoreAsync(model);
        await session.SaveChangesAsync();
      }

      return EntityResult<T>.Success(model);
    }


    /// <inheritdoc />
    public async Task<EntityResult<object>> Delete(string alias, string id)
    {
      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        SpaceContent spaceContent = await session.LoadAsync<SpaceContent>(id);

        if (spaceContent == null || !spaceContent.SpaceAlias.Equals(alias, StringComparison.InvariantCultureIgnoreCase))
        {
          return EntityResult.Fail("@errors.ondelete.idnotfound");
        }

        session.Delete(spaceContent);

        await session.SaveChangesAsync();
      }

      return EntityResult.Success();
    }
  }


  public interface ISpacesApi
  {
    /// <summary>
    /// Returns a space by the defined alias
    /// </summary>
    Space GetByAlias(string alias);

    /// <summary>
    /// Get all spaces
    /// </summary>
    IReadOnlyCollection<Space> GetAll();

    /// <summary>
    /// Get editor configuration for a space
    /// </summary>
    RendererConfig GetEditorConfig(string alias);

    /// <summary>
    /// Get editor item for a space
    /// </summary>
    Task<T> GetItem<T>(string alias) where T : SpaceContent;

    /// <summary>
    /// Get editor item for a space
    /// </summary>
    Task<T> GetItem<T>(string alias, string id) where T : SpaceContent;

    /// <summary>
    /// Get all list items by space alias
    /// </summary>
    Task<IList<T>> GetList<T>(string alias) where T : SpaceContent;

    /// <summary>
    /// Get all list items for a space (with query)
    /// </summary>
    Task<ListResult<T>> GetListByQuery<T>(string alias, ListQuery<T> query, string appId = null) where T : SpaceContent;

    /// <summary>
    /// Saves a content item in a space
    /// </summary>
    Task<EntityResult<T>> Save<T>(string alias, T model) where T : SpaceContent;

    /// <summary>
    /// Deletes a space content item
    /// </summary>
    Task<EntityResult<object>> Delete(string alias, string id);
  }
}
