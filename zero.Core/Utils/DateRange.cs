namespace zero.Utils;

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


  public string Format(string format)
  {
    if (!From.HasValue && !To.HasValue)
    {
      return null;
    }
    if (!From.HasValue && To.HasValue)
    {
      return "≤ " + To.Value.ToString(format);
    }
    if (From.HasValue && !To.HasValue)
    {
      return "≥ " + From.Value.ToString(format);
    }
    return From.Value.ToString(format) + " – " + To.Value.ToString(format);
  }
}