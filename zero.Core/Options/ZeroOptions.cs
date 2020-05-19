using FluentValidation;
using System.Collections.Generic;
using zero.Core.Entities;
using zero.Core.Mapper;
using zero.Core.Plugins;
using zero.Core.Renderer;

namespace zero.Core.Options
{
  public class ZeroOptions : IZeroOptions
  {
    public ZeroOptions()
    {
      SupportedLanguages = new string[2] { "en-US", "de-DE" };
      DefaultLanguage = SupportedLanguages[0];
      TokenExpiration = 60;
      BackofficePath = "/zero";
      Sections = new SectionOptions();
      Features = new FeatureOptions();
      Pages = new PageOptions();
      Permissions = new PermissionOptions();
      Renderers = new RendererOptions();
      Settings = new SettingsOptions();
      Spaces = new SpaceOptions();
      Mapper = new MapperOptions();
      Types = new TypeOptions();
    }

    /// <inheritdoc />
    public string ZeroVersion { get; set; }

    /// <inheritdoc />
    public string DefaultLanguage { get; set; }

    /// <inheritdoc />
    public string[] SupportedLanguages { get; private set; }

    /// <inheritdoc />
    public int TokenExpiration { get; set; }

    /// <inheritdoc />
    public RavenOptions Raven { get; set; }

    /// <inheritdoc />
    public string BackofficePath { get; set; }

    /// <inheritdoc />
    //public IZeroPluginConfiguration Backoffice { get; set; }

    /// <inheritdoc />
    public SectionOptions Sections { get; private set; }

    /// <inheritdoc />
    public FeatureOptions Features { get; private set; }

    /// <inheritdoc />
    public PageOptions Pages { get; private set; }

    /// <inheritdoc />
    public PermissionOptions Permissions { get; private set; }

    /// <inheritdoc />
    public RendererOptions Renderers { get; private set; }

    /// <inheritdoc />
    public SettingsOptions Settings { get; private set; }

    /// <inheritdoc />
    public SpaceOptions Spaces { get; private set; }

    /// <inheritdoc />
    public MapperOptions Mapper { get; private set; }

    /// <inheritdoc />
    public TypeOptions Types { get; private set; }

    /// <inheritdoc />
    public void Extend<T, TTarget>() where TTarget : class, T
    {
      Types.Add<T, TTarget>();
    }

    public ZeroExtend<T> Extend<T>() where T : class, IZeroEntity
    {
      return new ZeroExtend<T>();
    }
  }


  public class RavenOptions
  {
    public string Url { get; set; }

    public string Database { get; set; }
  }


  public interface IZeroOptions
  {
    /// <summary>
    /// The currently active version
    /// This should not be set manually, as it is used for setup and migrations and incremented automatically
    /// </summary>
    string ZeroVersion { get; set; }

    /// <summary>
    /// Default language ISO code
    /// </summary>
    string DefaultLanguage { get; set; }

    /// <summary>
    /// Language ISO codes which are supported by the zero backoffice
    /// </summary>
    string[] SupportedLanguages { get; }

    /// <summary>
    /// Expiration in minutes of a generated change token for an entity
    /// </summary>
    int TokenExpiration { get; set; }

    /// <summary>
    /// RavenDB configuration data
    /// </summary>
    RavenOptions Raven { get; set; }

    /// <summary>
    /// URL path to the backoffice (defaults to /zero)
    /// </summary>
    string BackofficePath { get; set; }

    /// <summary>
    /// 
    /// </summary>
    SectionOptions Sections { get; }

    /// <summary>
    /// 
    /// </summary>
    FeatureOptions Features { get; }

    /// <summary>
    /// 
    /// </summary>
    PageOptions Pages { get; }

    /// <summary>
    /// 
    /// </summary>
    PermissionOptions Permissions { get; }

    /// <summary>
    /// 
    /// </summary>
    RendererOptions Renderers { get; }

    /// <summary>
    /// 
    /// </summary>
    SettingsOptions Settings { get; }

    /// <summary>
    /// 
    /// </summary>
    SpaceOptions Spaces { get; }

    /// <summary>
    /// 
    /// </summary>
    MapperOptions Mapper { get; }

    /// <summary>
    /// 
    /// </summary>
    TypeOptions Types { get; }

    /// <inheritdoc />
    void Extend<T, TTarget>() where TTarget : class, T;

    ZeroExtend<T> Extend<T>() where T : class, IZeroEntity;

    /// <summary>
    /// Default settings for the backoffice
    /// </summary>
    //IZeroPluginConfiguration Backoffice { get; set; }
  }


  public class ZeroExtend<T> where T : IZeroEntity
  {
    public ZeroExtend<T> Use<TTarget>() where TTarget : class, T
    {
      return this;
    }

    public ZeroExtend<T> Use<TTarget, TRenderer>() 
      where TTarget : class, T
      where TRenderer : IRenderer<T>
    {
      return this;
    }

    public ZeroExtend<T> Use<TTarget, TRenderer, TValidator>() 
      where TTarget : class, T
      where TRenderer : IRenderer<T> 
      where TValidator : IValidator<T>
    {
      return this;
    }


    public ZeroExtend<T> Use<TTarget, TRenderer, TValidator, TMapper>() 
      where TTarget : class, T
      where TRenderer : IRenderer<T> 
      where TValidator : IValidator<T>
      where TMapper : IMapperConfig
    {
      return this;
    }


    public ZeroExtend<T> UseRenderer<TRenderer>() where TRenderer : IRenderer<T>
    {
      return this;
    }

    public ZeroExtend<T> UseValidator<TValidator>() where TValidator : IRenderer<T>
    {
      return this;
    }

    public ZeroExtend<T> UseMapper<TMapper>() where TMapper : IRenderer<T>
    {
      return this;
    }
  }
}
