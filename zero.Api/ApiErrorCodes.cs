namespace zero.Api;

public static class ApiErrorCodes
{
  public static class Categories
  {
    public const string Server = "server";
    public const string Validation = "validation";
  }

  public static class Server
  {
    public const string Exception = "server.error";
  }
}
