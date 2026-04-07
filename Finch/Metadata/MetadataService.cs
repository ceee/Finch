using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text;

namespace Finch.Metadata;

public class MetadataService : IMetadataService
{
  protected ILogger<MetadataService> Logger { get; private set; }

  protected ILocalizer Localizer { get; private set; }


  public MetadataService(ILogger<MetadataService> logger, ILocalizer localizer)
  {
    Logger = logger;
    Localizer = localizer;
  }


  /// <inheritdoc />
  public virtual Metadata Generate(string pageName, MetadataOptions options)
  {
    Metadata model = new()
    {
      Description = options.Description,
      Icon = options.Icon,
      Image = options.Image,
      Author = options.Author,
      PageName = pageName,
      AppVersion = Assembly.GetEntryAssembly()!.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion.Split('+').FirstOrDefault()};

    // generate robots
    bool noIndex = options.NoIndex ?? false;
    bool noFollow = options.NoFollow ?? false;
    model.Robots = String.Format("{0},{1}", noIndex ? "noindex" : "index", noFollow ? "nofollow" : "follow");

    // title
    HashSet<string> fragments = options.TitleFragments.Where(x => !x.IsNullOrWhiteSpace()).Reverse().ToHashSet();
    model.Title = GenerateTitle(pageName, fragments, options);

    // add schema
    //if (options.Schema != null)
    //{
    //  model.Schema = options.Schema.ToJson();
    //}

    return model;
  }


  /// <summary>
  /// Generates the page title based on passed fragments
  /// </summary>
  protected virtual string GenerateTitle(string pageName, HashSet<string> fragments, MetadataOptions options)
  {
    StringBuilder sb = new();

    if (fragments.Count != 0)
    {
      sb.Append(string.Join(options.TitleFragmentsSeparator, fragments.Select(fragment => Localizer.Maybe(fragment))));
      
      if (!options.HidePageName)
      {
        sb.Append(options.TitlePageNameToFragmentSeparator);
        sb.Append(pageName);
      }
    }
    else 
    {
      sb.Append(pageName);
    }

    return sb.ToString();
  }
}


public interface IMetadataService
{
  /// <summary>
  /// Generates metadata.
  /// </summary>
  Metadata Generate(string pageName, MetadataOptions options);
}
