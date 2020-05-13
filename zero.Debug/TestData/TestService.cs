namespace zero.Debug.TestData
{
  public class TestService : ITestService
  {
    public string GetName()
    {
      return "tobi";
    }
  }


  public interface ITestService
  {
    string GetName();
  }
}
