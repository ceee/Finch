namespace zero.Core.Extensions
{
  public static class NumberExtensions
  {
    public static int Limit(this int input, int min, int max)
    {
      if (input < min)
      {
        return min;
      }
      if (input > max)
      {
        return max;
      }
      return input;
    }
  }
}
