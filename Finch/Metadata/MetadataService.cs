using System.Reflection;
using System.Text;
using Microsoft.Extensions.Hosting;

namespace Finch.Metadata;

public class MetadataService(ILocalizer localizer, IHostEnvironment env) : IMetadataService
{
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
    // noindex+nofollow when not in production
    if (!env.IsProduction())
    {
      noIndex = true;
      noFollow = true;
    }
    model.Robots = $"{(noIndex ? "noindex" : "index")},{(noFollow ? "nofollow" : "follow")}";

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
      sb.Append(string.Join(options.TitleFragmentsSeparator, fragments.Select(localizer.Maybe)));
      
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
