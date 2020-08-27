//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace zero.Core.Entities
//{
//  public class ZeroEntityInterfaceBinderProvider : IModelBinderProvider
//  {
//    public IModelBinder GetBinder(ModelBinderProviderContext context)
//    {
//      if (context == null)
//      {
//        throw new ArgumentNullException(nameof(context));
//      }

//      if (context.Metadata.ModelType == typeof(IPage))
//      {
//        return new BinderTypeModelBinder(typeof(ZeroEntityInterfaceBinder));
//      }

//      return null;
//    }
//  }


//  public class ZeroEntityInterfaceBinder : IModelBinder
//  {
//    //private readonly AuthorContext _context;

//    //public ZeroEntityInterfaceBinder(AuthorContext context)
//    //{
//    //  _context = context;
//    //}

//    public Task BindModelAsync(ModelBindingContext bindingContext)
//    {
//      if (bindingContext == null)
//      {
//        throw new ArgumentNullException(nameof(bindingContext));
//      }

//      var modelName = bindingContext.OriginalModelName;

//      // Try to fetch the value of the argument by name
//      var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);

//      if (valueProviderResult == ValueProviderResult.None)
//      {
//        return Task.CompletedTask;
//      }

//      bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

//      var value = valueProviderResult.FirstValue;

//      // Check if the argument value is null or empty
//      //if (string.IsNullOrEmpty(value))
//      //{
//      //  return Task.CompletedTask;
//      //}

//      //if (!int.TryParse(value, out var id))
//      //{
//      //  // Non-integer arguments result in model state errors
//      //  bindingContext.ModelState.TryAddModelError(
//      //      modelName, "Author Id must be an integer.");

//      //  return Task.CompletedTask;
//      //}

//      // Model will be null if not found, including for
//      // out of range id values (0, -3, etc.)
//      //var model = _context.Authors.Find(id);
//      bindingContext.Result = ModelBindingResult.Success(new { name = "tobi" });
//      return Task.CompletedTask;
//    }
//  }
//}
