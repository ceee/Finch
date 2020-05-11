using Microsoft.Extensions.Options;
using System;
using System.Linq;
using zero.Core.Entities;

namespace zero.Core.Api
{
  public class SectionsApi : ISectionsApi
  {
    protected IZeroOptions Options { get; private set; }


    public SectionsApi(IZeroOptions options)
    {
      Options = options;
    }


    /// <inheritdoc />
    public SectionCollection GetAll()
    {
      return Options.Backoffice.Sections;
    }


    /// <inheritdoc />
    public ISection GetByAlias(string alias)
    {
      return Options.Backoffice.Sections.FirstOrDefault(section => section.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));
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
