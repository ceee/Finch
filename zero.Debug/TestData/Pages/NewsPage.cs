using System;
using zero.Core.Entities;

namespace zero.TestData
{
  public class NewsPage : Page
  {
    public DateTimeOffset PublishDate { get; set; }

    public string Text { get; set; }
  }
}
