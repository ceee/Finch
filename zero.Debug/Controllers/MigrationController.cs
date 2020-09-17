using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using zero.Commerce.Api;
using zero.Commerce.Entities;
using zero.Core;
using zero.Core.Api;
using zero.Core.Attributes;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Utils;
using zero.Web.Filters;

namespace zero.Debug.Controllers
{
  public class MigrationController : Controller
  {
    private IDocumentStore Raven { get; set; }

    public MigrationController(IDocumentStore raven)
    {
      Raven = raven;
    }


    [HttpGet]
    public async Task<IActionResult> SharedEntities()
    {
      async Task<object> Handle<T>() where T : IZeroIdEntity
      {
        return await SaveAll<T>();
      }

      return Json(new
      {
        Applications = await Handle<IApplication>(),
        Categories = await Handle<ICategory>(),
        Channels = await Handle<IChannel>(),
        Countries = await Handle<ICountry>(),
        Currencies = await Handle<ICurrency>(),
        Customers = await Handle<ICustomer>(),
        Languages = await Handle<ILanguage>(),
        MailTemplates = await Handle<IMailTemplate>(),
        Manufacturers = await Handle<IManufacturer>(),
        Media = await Handle<IMedia>(),
        MediaFolders = await Handle<IMediaFolder>(),
        NumberTemplates = await Handle<INumberTemplate>(),
        OrderDetailStates = await Handle<IOrderDetailState>(),
        Orders = await Handle<IOrder>(),
        Pages = await Handle<IPage>(),
        ProductProperties = await Handle<IProperty>(),
        Products = await Handle<IProduct>(),
        RecycleBin = await Handle<IRecycledEntity>(),
        ShippingOptions = await Handle<IShippingOption>(),
        SpaceContents = await Handle<ISpaceContent>(),
        TaxRates = await Handle<ITaxRate>(),
        Translations = await Handle<ITranslation>(),
        UserRoles = await Handle<IUserRole>(),
        Users = await Handle<IUser>(),
      });
    }


    async Task<HashSet<string>> SaveAll<T>() where T : IZeroIdEntity
    {
      HashSet<string> changedIds = new HashSet<string>();
      IList<T> items;

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        items = await session.Query<T>().ToListAsync();
      }

      foreach (T item in items)
      {
        if (await Save(item))
        {
          changedIds.Add(item.Id);
        }
      }

      return changedIds;
    }


    async Task<bool> Save<T>(T model) where T : IZeroIdEntity
    {
      IAppAwareEntity appAwareEntity = model as IAppAwareEntity;
      IZeroEntity zeroEntity = model as IZeroEntity;

      bool hasChange = false;

      // set app id
      if (appAwareEntity != null && appAwareEntity.AppId.IsNullOrEmpty())
      {
        hasChange = true;
        appAwareEntity.AppId = "shared";
      }

      // set unset Raven Ids
      foreach (ObjectTraverser.Result<GenerateIdAttribute> item in ObjectTraverser.FindAttribute<GenerateIdAttribute>(model))
      {
        string id = item.Property.GetValue(item.Parent, null) as string;
        if (String.IsNullOrWhiteSpace(id))
        {
          hasChange = true;
          item.Property.SetValue(item.Parent, item.Item.Length.HasValue ? IdGenerator.Create(item.Item.Length.Value) : IdGenerator.Create());
        }
      }

      string userId = HttpContext.User.Claims.FirstOrDefault(x => x.Type == Constants.Auth.Claims.UserId)?.Value;

      // set default properties
      if (zeroEntity != null && zeroEntity.CreatedById == null)
      {
        hasChange = true;
        zeroEntity.CreatedDate = zeroEntity.CreatedDate == default ? DateTimeOffset.Now : zeroEntity.CreatedDate;
        zeroEntity.CreatedById = userId;
      }

      if (zeroEntity != null && model is ITranslation)
      {
        zeroEntity.Name = ((ITranslation)model).Key;
      }

      // update name alias and last modified
      if (zeroEntity != null && zeroEntity.Alias.IsNullOrEmpty())
      {
        hasChange = true;
        zeroEntity.Alias = Safenames.Alias(zeroEntity.Name);
      }
      if (zeroEntity != null && zeroEntity.LastModifiedById == default)
      {
        hasChange = true;
        zeroEntity.LastModifiedById = userId;
      }

      if (!hasChange)
      {
        return false;
      }

      using IAsyncDocumentSession session = Raven.OpenAsyncSession();
      await session.StoreAsync(model);
      await session.SaveChangesAsync();
      return true;
    }
  }
}