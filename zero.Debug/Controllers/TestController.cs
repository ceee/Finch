using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Raven.Client.Documents.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using zero.Commerce.Api;
using zero.Commerce.Entities;
using zero.Core.Api;
using zero.Core.Entities;
using zero.Core.Extensions;
using zero.Core.Utils;
using zero.Web.Filters;

namespace zero.Debug.Controllers
{
  [ServiceFilter(typeof(ModelStateValidationFilterAttribute))]
  public class TestController : Controller
  {
    private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;


    public TestController(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
    {
      _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
    }


    [HttpGet]
    public IActionResult Services()
    {
      List<object> items = new List<object>();

      foreach (var service in Startup.Services)
      {
        if (!service.ServiceType.FullName.StartsWith("zero.") && (service.ImplementationType == null || !service.ImplementationType.FullName.StartsWith("zero.")))
        {
          continue;
        }

        items.Add(new
        {
          name = service.ServiceType.FullName,
          lifetime = service.Lifetime,
          instance = service.ImplementationType?.FullName
        });
      }

      return Json(new
      {
        count = items.Count,
        items
      }, new JsonSerializerSettings()
      {
        TypeNameHandling = TypeNameHandling.None
      });
    }


    [HttpGet]
    public async Task<IActionResult> GetTranslations([FromServices] ITranslationsApi api)
    {
      return Json(await api.GetAll());
    }



    [HttpGet]
    public IActionResult Things([FromServices] IValidator<Application> appValidator)
    {
      return Ok();
    }



    [HttpGet]
    public IActionResult Clone([FromServices] IRecycledEntity blueprint)
    {
      IRecycledEntity entity = blueprint.Clone();
      return Json(entity);
    }


    [HttpPost]
    public IActionResult SaveTest([FromBody] IPage model) => Json(model);




    [HttpGet]
    public IActionResult Routes()
    {

      var routes = _actionDescriptorCollectionProvider.ActionDescriptors.Items.ToList();
      if (routes != null && routes.Any())
      {
        return Json(routes);
      }
      return Json(new string [0]);
    }


    [HttpGet]
    public async Task<IActionResult> AddOrder([FromServices] IOrdersApi ordersApi)
    {
      OrderAddress address = new OrderAddress()
      {
        Email = "cee@live.at",
        FirstName = "Tobias",
        LastName = "Klika",
        Company = "brothers Klika OG",
        Gender = Gender.Male,
        VatNo = "ATU68334803",
        PhoneNumber = "+43 660 2594892",
        Address = "Laabstraße 9",
        AddressLine1 = "Top 2",
        Zip = "5280",
        City = "Braunau am Inn",
        Country = "Österreich",
        CountryId = "countries.256-A"
      };

      OrderAddress shippingAddress = new OrderAddress()
      {
        Email = "cee@live.at",
        FirstName = "Tobias",
        LastName = "Klika",
        Company = "brothers Klika OG",
        Gender = Gender.Male,
        VatNo = "ATU68334803",
        PhoneNumber = "+43 660 2594892",
        Address = "Salzburger Strasse 44",
        Zip = "5280",
        City = "Braunau am Inn",
        Country = "Österreich",
        CountryId = "countries.256-A"
      };

      Order order = new Order()
      {
        ChannelId = "channels.1-A",
        Name = "23-0002",
        Number = "23-0002",
        LanguageId = "languages.2-A",
        CurrencyId = "currencies.33-A",
        State = OrderState.Completed,
        DetailStateId = "orderDetailStates.3-A",
        IsRequest = false,
        CustomerId = "customers.1-A",
        CustomerNote = "Danke für die Hilfe. Das war wirklich dringend nötig <3",
        AssignedToUserId = "users.1-A",
        Address = address,
        Items = new List<IOrderItem>()
        {
          new OrderItem()
          {
            Id = IdGenerator.Create(),
            ProductId = "products.1-A",
            VariantId = "4a5ddabc-8158-43f5-90eb-1d115cc062cc",
            Name = "Nike Jersey 2.0",
            Description = "Polyester / XS",
            Sort = 0,
            Quantity = 3,
            Discount = 0,
            Price = 17,
            TaxRate = 20
          },
          new OrderItem()
          {
            Id = IdGenerator.Create(),
            ProductId = "products.1-A",
            VariantId = "4a5ddabc-8158-43f5-90eb-1d115cc062cc",
            Name = "Nike Jersey 2.0",
            Description = "Polyester / s",
            Sort = 1,
            Quantity = 1,
            Discount = 5,
            Price = 12,
            TaxRate = 20
          },
          new OrderItem()
          {
            Id = IdGenerator.Create(),
            Name = "Flex-Druck",
            Sort = 2,
            Price = 1.99m,
            Quantity = 2,
            TaxRate = 12
          }
        },
        Price = 72.98m
      };

      order.Shipping = new OrderShipping()
      {
        Name = "Österreichische Post",
        ShippingOptionId = "shippingOptions.1-A",
        Price = 7.99m,
        Lines = new List<OrderShippingLine>()
        {
          new OrderShippingLine() { Key = "Postfach",  Value = "7-A" }
        },
        Address = shippingAddress,
        Deliveries = new List<IOrderDelivery>()
        {
          new OrderDelivery()
          {
            CreatedDate = DateTimeOffset.Now,
            TrackingNumber = "1035304005500970252807",
            TrackingUrl = "https://www.post.at/sv/sendungsdetails?snr=1035304005500970252807",
            Description = "Waren sind im Lager 2 (Regal 23)",
            ShipmentDate = DateTimeOffset.Now.AddDays(2),
            Items = new List<OrderDeliveryProduct>()
            {
              new OrderDeliveryProduct() { Id = order.Items[0].Id, Quantity = 2 },
              new OrderDeliveryProduct() { Id = order.Items[1].Id, Quantity = 1 }
            }
          },
          new OrderDelivery()
          {
            CreatedDate = DateTimeOffset.Now,
            ShipmentDate = DateTimeOffset.Now.AddDays(4),
            Items = new List<OrderDeliveryProduct>()
            {
              new OrderDeliveryProduct() { Id = order.Items[0].Id, Quantity = 1 }
            }
          }
        }
      };

      return Json(await ordersApi.Save(order));
    }
  }

  internal class RouteModel
  {
    public string Name { get; set; }
    public string Template { get; set; }
  }
}