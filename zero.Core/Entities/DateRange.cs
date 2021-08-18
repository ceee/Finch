using System;

namespace zero.Core.Entities
{
  public class DateRange
  {
    public DateTimeOffset? From { get; set; }

    public DateTimeOffset? To { get; set; }

    public bool IsWithin(DateTimeOffset date)
    {
      if (From.HasValue && date < From)
      {
        return false;
      }
      if (To.HasValue && date > To)
      {
        return false;
      }
      return true;
    }
  }
}
