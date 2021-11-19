using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace zero.Core.Renderer
{
  public class RazorRenderer : IRazorRenderer, IDisposable
  {
    protected IRazorViewEngine ViewEngine { get; set; }

    protected ITempDataDictionaryFactory TempDataDictionaryFactory { get; set; }

    protected IModelMetadataProvider ModelMetadataProvider { get; set; }

    protected IHttpContextAccessor HttpContextAccessor { get; set; }

    protected IServiceScope ServiceScope { get; set; }

    protected HtmlHelperOptions HtmlHelperOptions { get; set; }


    public RazorRenderer(IRazorViewEngine viewEngine, IHttpContextAccessor httpContextAccessor, ITempDataDictionaryFactory tempDataDictionaryFactory, 
      IModelMetadataProvider modelMetadataProvider, IServiceProvider serviceProvider, IOptions<MvcViewOptions> mvcHelperOptions)
    {
      ViewEngine = viewEngine;
      HttpContextAccessor = httpContextAccessor;
      TempDataDictionaryFactory = tempDataDictionaryFactory;
      ModelMetadataProvider = modelMetadataProvider;
      ServiceScope = serviceProvider.CreateScope();
      HtmlHelperOptions = mvcHelperOptions.Value.HtmlHelperOptions;
    }


    /// <summary>
    /// Renders a razor component to a string
    /// </summary>
    public async Task<string> ComponentAsync<T>(object args = null) where T : ViewComponent
    {
      return await ComponentAsync(typeof(T), args);
    }


    /// <summary>
    /// Renders a razor component to a string
    /// </summary>
    public async Task<string> ComponentAsync(Type componentType, object args = null)
    {
      return await ComponentAsync(componentType, BuildActionContext(), args);
    }


    /// <summary>
    /// Renders a razor component to a string
    /// </summary>
    public async Task<string> ComponentAsync(string componentName, object args = null)
    {     
      return await ComponentAsync(componentName, BuildActionContext(), args);
    }


    /// <summary>
    /// Renders a razor component to a string
    /// </summary>
    public async Task<string> ComponentAsync<T>(ActionContext context, object args = null) where T : ViewComponent
    {
      return await ComponentAsync(typeof(T), context, args);
    }


    /// <summary>
    /// Renders a razor component to a string
    /// </summary>
    public async Task<string> ComponentAsync(Type componentType, ActionContext context, object args = null)
    {
      IViewComponentHelper viewComponentHelper = context.HttpContext.RequestServices.GetRequiredService<IViewComponentHelper>();
      
      using StringWriter stringWriter = new();

      ViewContext viewContext = BuildViewContext(context, stringWriter);

      (viewComponentHelper as IViewContextAware)?.Contextualize(viewContext);
      IHtmlContent result = await viewComponentHelper.InvokeAsync(componentType, args);

      result.WriteTo(stringWriter, HtmlEncoder.Default);
      await stringWriter.FlushAsync();
      return stringWriter.ToString();
    }


    /// <summary>
    /// Renders a razor component to a string
    /// </summary>
    public async Task<string> ComponentAsync(string componentName, ActionContext context, object args = null)
    {
      IViewComponentHelper viewComponentHelper = context.HttpContext.RequestServices.GetRequiredService<IViewComponentHelper>();
      
      using StringWriter stringWriter = new();

      ViewContext viewContext = BuildViewContext(context, stringWriter);

      (viewComponentHelper as IViewContextAware)?.Contextualize(viewContext);
      IHtmlContent result = await viewComponentHelper.InvokeAsync(componentName, args);

      result.WriteTo(stringWriter, HtmlEncoder.Default);
      await stringWriter.FlushAsync();
      return stringWriter.ToString();
    }


    /// <summary>
    /// Renders a razor view to a string
    /// </summary>
    public async Task<string> ViewAsync(string view, object model = null)
    {
      return await ViewAsync(BuildActionContext(), view, model);
    }


    /// <summary>
    /// Renders a razor view to a string
    /// </summary>
    public async Task<string> ViewAsync(ActionContext context, string view, object model = null)
    {
      IView viewResult = FindView(context, view);

      using StringWriter stringWriter = new();

      ViewContext viewContext = BuildViewContext(context, stringWriter, viewResult);
      viewContext.RouteData = context.RouteData;
      viewContext.ViewData.Model = model;

      await viewResult.RenderAsync(viewContext);
      await stringWriter.FlushAsync();

      return stringWriter.ToString();
    }


    /// <inheritdoc />
    public void Dispose()
    {
      ServiceScope?.Dispose();
    }


    /// <summary>
    /// Build the view context
    /// </summary>
    protected virtual ViewContext BuildViewContext(ActionContext context, StringWriter writer, IView view = null)
    {
      ViewDataDictionary viewData = new(ModelMetadataProvider, context.ModelState); // result.ViewData ?? new ViewData...;
      ITempDataDictionary tempData = TempDataDictionaryFactory.GetTempData(context.HttpContext); //  result.TempData ?? TempData...;

      return new ViewContext(context, view ?? NullView.Instance, viewData, tempData, writer, HtmlHelperOptions);
    }


    /// <summary>
    /// Builds a new view context
    /// </summary>
    protected virtual ActionContext BuildActionContext()
    {
      HttpContext context = GetHttpContext();
      RouteData routeData = context.GetRouteData();
      return new ActionContext(context, routeData, new ActionDescriptor());
    }


    /// <summary>
    /// Get HTTP context or mock one
    /// </summary>
    protected virtual HttpContext GetHttpContext()
    {
      HttpContext context = HttpContextAccessor.HttpContext;
      context ??= new DefaultHttpContext() 
      { 
        RequestServices = ServiceScope.ServiceProvider 
      };

      return context;
    }


    /// <summary>
    /// Tries to find a view
    /// </summary>
    protected virtual IView FindView(ActionContext actionContext, string viewName)
    {
      ViewEngineResult getViewResult = ViewEngine.GetView(executingFilePath: null, viewPath: viewName, isMainPage: false);
      if (getViewResult.Success)
      {
        return getViewResult.View;
      }

      ViewEngineResult findViewResult = ViewEngine.FindView(actionContext, viewName, isMainPage: false);
      if (findViewResult.Success)
      {
        return findViewResult.View;
      }

      IEnumerable<string> searchedLocations = getViewResult.SearchedLocations.Concat(findViewResult.SearchedLocations);
      string errorMessage = String.Join(Environment.NewLine, new[] { $"Unable to find view '{viewName}'. The following locations were searched:" }.Concat(searchedLocations));

      throw new InvalidOperationException(errorMessage);
    }
  }


  public interface IRazorRenderer
  {
    /// <summary>
    /// Renders a razor component to a string
    /// </summary>
    Task<string> ComponentAsync<T>(object args = null) where T : ViewComponent;

    /// <summary>
    /// Renders a razor component to a string
    /// </summary>
    Task<string> ComponentAsync(Type componentType, object args = null);

    /// <summary>
    /// Renders a razor component to a string
    /// </summary>
    Task<string> ComponentAsync(string componentName, object args = null);

    /// <summary>
    /// Renders a razor component to a string
    /// </summary>
    Task<string> ComponentAsync<T>(ActionContext context, object args = null) where T : ViewComponent;

    /// <summary>
    /// Renders a razor component to a string
    /// </summary>
    Task<string> ComponentAsync(Type componentType, ActionContext context, object args = null);

    /// <summary>
    /// Renders a razor component to a string
    /// </summary>
    Task<string> ComponentAsync(string componentName, ActionContext context, object args = null);

    /// <summary>
    /// Renders a razor view to a string
    /// </summary>
    Task<string> ViewAsync(string view, object model = null);

    /// <summary>
    /// Renders a razor view to a string
    /// </summary>
    Task<string> ViewAsync(ActionContext context, string view, object model = null);
  }


  class GenericController : ControllerBase { }


  class NullView : IView
  {
    public static readonly NullView Instance = new NullView();

    public string Path => string.Empty;

    public Task RenderAsync(ViewContext context)
    {
      if (context == null)
      {
        throw new ArgumentNullException(nameof(context));
      }

      return Task.CompletedTask;
    }
  }
}
