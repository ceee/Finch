using System;
using System.Collections.Generic;
using System.Linq;
using zero.Core.Entities;
using zero.Core.Options;

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
    public IReadOnlyCollection<ISection> GetAll()
    {
      return Options.Sections.GetAllItems();
    }


    /// <inheritdoc />
    public ISection GetByAlias(string alias)
    {
      return Options.Sections.GetAllItems().FirstOrDefault(section => section.Alias.Equals(alias, StringComparison.InvariantCultureIgnoreCase));
    }
  }


  public interface ISectionsApi
  {
    /// <summary>
    /// Get all available backoffice sections
    /// </summary>
    IReadOnlyCollection<ISection> GetAll();

    /// <summary>
    /// Get backoffice section by alias
    /// </summary>
    ISection GetByAlias(string alias);
  }
}
