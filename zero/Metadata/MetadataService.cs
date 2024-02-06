using Microsoft.Extensions.Logging;
using System.Reflection;

namespace zero.Metadata;

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
  public Metadata Generate(string pageName, MetadataOptions options)
  {
    Metadata model = new()
    {
      Description = options.Description,
      Icon = options.Icon,
      Image = options.Image,
      Author = options.Author,
      PageName = pageName,
      AppVersion = Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion
    };

    // generate robots
    bool noIndex = options.NoIndex ?? false;
    bool noFollow = options.NoFollow ?? false;
    model.Robots = String.Format("{0},{1}", noIndex ? "noindex" : "index", noFollow ? "nofollow" : "follow");

    // title
    HashSet<string> fragments = options.TitleFragments.Where(x => !x.IsNullOrWhiteSpace()).Reverse().ToHashSet();
    model.Title = !fragments.Any() ? pageName : (String.Join(" / ", fragments.Select(fragment => Localizer.Maybe(fragment))) + " / " + pageName);

    // add schema
    //if (options.Schema != null)
    //{
    //  model.Schema = options.Schema.ToJson();
    //}

    return model;
  }
}


public interface IMetadataService
{
  /// <summary>
  /// Generates metadata.
  /// </summary>
  Metadata Generate(string pageName, MetadataOptions options);
}
