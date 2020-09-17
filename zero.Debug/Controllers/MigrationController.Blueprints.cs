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


    [HttpGet]
    public async Task<IActionResult> CreateBlueprints()
    {
      IList<IApplication> apps;

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        apps = await session.Query<IApplication>().ToListAsync();
      }

      async Task<object> Handle<T>() where T : IZeroIdEntity
      {
        return await CreateBlueprintsForAll<T>(apps);
      }

      return Json(new
      {
        //Applications = await Handle<IApplication>(),
        //Categories = await Handle<ICategory>(),
        //Channels = await Handle<IChannel>(),
        //Countries = await Handle<ICountry>(),
        //Currencies = await Handle<ICurrency>(),
        //Customers = await Handle<ICustomer>(),
        //Languages = await Handle<ILanguage>(),
        //MailTemplates = await Handle<IMailTemplate>(),
        //Manufacturers = await Handle<IManufacturer>(),
        //Media = await Handle<IMedia>(),
        //MediaFolders = await Handle<IMediaFolder>(),
        //NumberTemplates = await Handle<INumberTemplate>(),
        //OrderDetailStates = await Handle<IOrderDetailState>(),
        //Orders = await Handle<IOrder>(),
        //Pages = await Handle<IPage>(),
        //ProductProperties = await Handle<IProperty>(),
        //Products = await Handle<IProduct>(),
        //RecycleBin = await Handle<IRecycledEntity>(),
        ShippingOptions = await Handle<IShippingOption>(),
        //SpaceContents = await Handle<ISpaceContent>(),
        //TaxRates = await Handle<ITaxRate>(),
        //Translations = await Handle<ITranslation>(),
        //UserRoles = await Handle<IUserRole>(),
        //Users = await Handle<IUser>(),
      });
    }


    async Task<HashSet<string>> CreateBlueprintsForAll<T>(IList<IApplication> apps) where T : IZeroIdEntity
    {
      HashSet<string> ids = new HashSet<string>();
      IList<T> items;

      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
      {
        session.Advanced.MaxNumberOfRequestsPerSession = 10000;

        items = await session.Query<T>().ToListAsync();

        foreach (T item in items)
        {
          HashSet<IZeroIdEntity> variants = ExtractBlueprints(item, apps);

          if (variants.Count > 0)
          {
            ids.Add("blueprint." + item.Id);

            foreach (IZeroIdEntity variant in variants)
            {
              ((IZeroEntity)variant).BlueprintId = item.Id;

              // find all references
              List<ObjectTraverser.Result<ReferenceAttribute>> references = ObjectTraverser.FindAttribute<ReferenceAttribute>(variant);

              foreach (ObjectTraverser.Result<ReferenceAttribute> reference in references)
              {
                object value = reference.Property.GetValue(reference.Parent, null);

                //if (String.IsNullOrWhiteSpace(id) || id.StartsWith(NEW_ID))
                //{
                //  item.Property.SetValue(item.Parent, item.Item.Length.HasValue ? Raven.Id(item.Item.Length.Value) : Raven.Id());
                //}
              }

              //await session.StoreAsync(variant);
              ids.Add(variant.Id);
            }
          }
        }

        //await session.SaveChangesAsync();
      }

      return ids;
    }


    HashSet<IZeroIdEntity> ExtractBlueprints<T>(T model, IList<IApplication> apps) where T : IZeroIdEntity
    {
      HashSet<IZeroIdEntity> newModels = new HashSet<IZeroIdEntity>();

      IAppAwareEntity appAwareEntity = model as IAppAwareEntity;
      IZeroEntity zeroEntity = model as IZeroEntity;

      if (appAwareEntity != null && appAwareEntity.AppId == "shared")
      {
        foreach (IApplication app in apps)
        {
          IZeroEntity newSpecificEntity = zeroEntity.Clone();
          newSpecificEntity.Id = null;
          ((IAppAwareEntity)newSpecificEntity).AppId = app.Id;

          newModels.Add(newSpecificEntity);
        }
      }

      return newModels;
    }
  }
}