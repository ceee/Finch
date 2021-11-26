namespace zero.Backoffice.Models;

public class ListQueryDateRange
{
  public DateTimeOffset? From { get; set; }

  public DateTimeOffset? To { get; set; }
}

public class ListQueryRange
{
  public decimal? From { get; set; }

  public decimal? To { get; set; }
}