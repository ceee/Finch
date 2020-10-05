//using Microsoft.AspNetCore.Mvc;
//using Raven.Client.Documents;
//using Raven.Client.Documents.Session;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using zero.Commerce.Entities;
//using zero.Core;
//using zero.Core.Api;
//using zero.Core.Attributes;
//using zero.Core.Entities;
//using zero.Core.Extensions;
//using zero.Core.Utils;

//namespace zero.Debug.Controllers
//{
//  public partial class MigrationController : Controller
//  {


//    [HttpGet]
//    public async Task<IActionResult> CreateBlueprints()
//    {
//      IList<IApplication> apps;

//      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
//      {
//        apps = await session.Query<IApplication>().ToListAsync();
//      }

//      async Task<object> Handle<T>() where T : IZeroEntity
//      {
//        return await CreateBlueprintsForAll<T>(apps);
//      }

//      return Json(new
//      {
//        //Applications = await Handle<IApplication>(),
//        //Categories = await Handle<ICategory>(),
//        //Channels = await Handle<IChannel>(),
//        //Countries = await Handle<ICountry>(),
//        //Currencies = await Handle<ICurrency>(),
//        //Customers = await Handle<ICustomer>(),
//        //Languages = await Handle<ILanguage>(),
//        //MailTemplates = await Handle<IMailTemplate>(),
//        //Manufacturers = await Handle<IManufacturer>(),
//        //Media = await Handle<IMedia>(),
//        //MediaFolders = await Handle<IMediaFolder>(),
//        //NumberTemplates = await Handle<INumberTemplate>(),
//        //OrderDetailStates = await Handle<IOrderDetailState>(),
//        //Orders = await Handle<IOrder>(),
//        //Pages = await Handle<IPage>(),
//        //ProductProperties = await Handle<IProperty>(),
//        //Products = await Handle<IProduct>(),
//        //RecycleBin = await Handle<IRecycledEntity>(),
//        ShippingOptions = await Handle<IShippingOption>(),
//        //SpaceContents = await Handle<ISpaceContent>(),
//        //TaxRates = await Handle<ITaxRate>(),
//        //Translations = await Handle<ITranslation>(),
//        //UserRoles = await Handle<IUserRole>(),
//        //Users = await Handle<IUser>(),
//      });
//    }


//    async Task<HashSet<string>> CreateBlueprintsForAll<T>(IList<IApplication> apps) where T : IZeroEntity
//    {
//      HashSet<string> ids = new HashSet<string>();
//      IList<T> items;

//      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
//      {
//        session.Advanced.MaxNumberOfRequestsPerSession = 10000;

//        items = await session.Query<T>().ToListAsync();

//        foreach (T item in items)
//        {
//          HashSet<IZeroIdEntity> variants = ExtractBlueprints(item, apps);

//          if (variants.Count > 0)
//          {
//            ids.Add("blueprint." + item.Id);

//            foreach (IZeroIdEntity variant in variants)
//            {
//              ((IZeroEntity)variant).BlueprintId = item.Id;

//              //await UpdateReferences(variant);

//              await session.StoreAsync(variant);
//              ids.Add(variant.Id);
//            }
//          }
//        }

//        await session.SaveChangesAsync();
//      }

//      using (IAsyncDocumentSession session = Raven.OpenAsyncSession())
//      {
//        session.Advanced.MaxNumberOfRequestsPerSession = 10000;

//        items = await session.Query<T>().ToListAsync(); //.Where(x => x.BlueprintId != null).ToListAsync();

//        foreach (T item in items)
//        {
//          await UpdateReferences(item, session);

//          ids.Add(item.Id);
//        }

//        await session.SaveChangesAsync();
//      }

//      return ids;
//    }


//    async Task UpdateReferences(IZeroIdEntity entity, IAsyncDocumentSession session)
//    {
//      await Task.Delay(1);

//      List<ObjectTraverser.Result<Refs>> references = ObjectTraverser.Find<Refs>(entity);

//      foreach (ObjectTraverser.Result<Refs> reference in references)
//      {
//        if (reference.Item != null && reference.Item.Ids.Length > 0)
//        {
//          Type type = reference.Item.GetType();
//          Type generic = type.GetGenericArguments().FirstOrDefault();

//          string collection = session.Advanced.DocumentStore.Conventions.FindCollectionName(generic);

//          for (int idx = 0; idx < reference.Item.Ids.Length; idx++)
//          {
//            string id = reference.Item.Ids[idx];
//            IZeroEntity xentity = await session.Query<IZeroEntity>(collectionName: collection).FirstOrDefaultAsync(x => x.BlueprintId == id);

//            if (xentity != null)
//            {
//              reference.Item.Ids[idx] = xentity.Id;
//            }
//          }
//        }
//      }
//    }


//    HashSet<IZeroIdEntity> ExtractBlueprints<T>(T model, IList<IApplication> apps) where T : IZeroIdEntity
//    {
//      HashSet<IZeroIdEntity> newModels = new HashSet<IZeroIdEntity>();

//      IAppAwareEntity appAwareEntity = model as IAppAwareEntity;
//      IZeroEntity zeroEntity = model as IZeroEntity;

//      if (appAwareEntity != null && appAwareEntity.AppId == "shared")
//      {
//        foreach (IApplication app in apps)
//        {
//          IZeroEntity newSpecificEntity = zeroEntity.Clone();
//          newSpecificEntity.Id = null;
//          ((IAppAwareEntity)newSpecificEntity).AppId = app.Id;

//          newModels.Add(newSpecificEntity);
//        }
//      }

//      return newModels;
//    }
//  }
//}