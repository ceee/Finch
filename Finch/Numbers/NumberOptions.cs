namespace Finch.Numbers;

public class NumberOptions : Dictionary<string, NumberOptionsItem>
{
}


public class NumberOptionsItem
{
  public string Template { get; set; } = "{number}";

  public long StartNumber { get; set; } = 1;

  public int MinLength { get; set; } = 1;

  public int Step { get; set; } = 1;

  public bool ResetPerYear { get; set; } = false;
}
