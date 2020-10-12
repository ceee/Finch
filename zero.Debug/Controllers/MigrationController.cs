using Microsoft.AspNetCore.Mvc;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Commerce.Entities;
using zero.Core;
using zero.Core.Api;
using zero.Core.Attributes;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Utils;

namespace zero.Debug.Controllers
{
  public partial class MigrationController : Controller
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

        foreach (T item in items)
        {
          if (Update(item, session.Advanced.GetLastModifiedFor(item)))
          {
            changedIds.Add(item.Id);
          }
        }

        await session.SaveChangesAsync();
      }

      return changedIds;
    }


    bool Update<T>(T model, DateTime? modifiedDate) where T : IZeroIdEntity
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

      if (zeroEntity != null)
      {
        if (zeroEntity.CreatedById == null)
        {
          hasChange = true;
          zeroEntity.CreatedDate = zeroEntity.CreatedDate == default ? DateTimeOffset.Now : zeroEntity.CreatedDate;
          zeroEntity.CreatedById = userId.Ref<IUser>();
        }
        if (model is ITranslation && zeroEntity.Name.IsNullOrEmpty())
        {
          hasChange = true;
          zeroEntity.Name = ((ITranslation)model).Key;
        }
        if (zeroEntity.Alias.IsNullOrEmpty())
        {
          hasChange = true;
          zeroEntity.Alias = Safenames.Alias(zeroEntity.Name);
        }
        if (zeroEntity.LastModifiedById == default)
        {
          hasChange = true;
          zeroEntity.LastModifiedById = userId.Ref<IUser>();
        }
        if (zeroEntity.LastModifiedDate == default)
        {
          hasChange = true;
          zeroEntity.LastModifiedDate = modifiedDate.HasValue ? new DateTimeOffset(modifiedDate.Value) : zeroEntity.CreatedDate;
        }
      }

      return hasChange;
    }
  }
}