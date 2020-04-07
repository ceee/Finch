using Microsoft.Extensions.Options;
using System;
using System.Linq;
using zero.Core.Entities;

namespace zero.Core.Api
{
  public class SectionsApi : ISectionsApi
  {
    protected ZeroOptions Options { get; private set; }


    public SectionsApi(IOptionsMonitor<ZeroOptions> options)
    {
      Options = options.CurrentValue;
    }


    /// <inheritdoc />
    public SectionCollection GetAll()
    {
      return Options.Sections;
    }


    /// <inheritdoc />
    public ISection GetByAlias(string alias)
    {
      return Options.Sections.FirstOrDefault(section => section.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));
    }
  }


  public interface ISectionsApi
  {
    /// <summary>
    /// Get all available backoffice sections
    /// </summary>
    SectionCollection GetAll();

    /// <summary>
    /// Get backoffice section by alias
    /// </summary>
    ISection GetByAlias(string alias);
  }
}
