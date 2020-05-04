using System;
using System.Collections.Generic;
using zero.Core.Entities;

namespace zero.Web.Models
{
  public class TranslationEditModel : EditModel
  {
    public string Key { get; set; }

    public string Value { get; set; }

    public TranslationDisplay Display { get; set; }

    public DateTimeOffset CreatedDate { get; set; }
  }
}
