using System;
using System.Collections.Generic;
using System.Text;

namespace zero.Core.Entities
{
  public class LanguageVariant
  {
    public string LanguageId { get; set; }

    public string Name { get; set; }

    public bool HideEntity { get; set; }
  }


  public interface ILanguageVariant
  {
    string LanguageId { get; set; }
  }
}
