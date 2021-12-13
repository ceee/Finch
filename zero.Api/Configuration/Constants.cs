namespace zero.Api.Configuration;

public static class ApiConstants
{
  public const string ChangeToken = "zero.api.change_vector";

  public static class HttpErrors
  {
    public const string NoIdMatchOnUpdate = "The Id as part of the URL does not match the Id of the model";

    public const string IdNotFound = "Could not find persisted model for the given Id";

    public const string ChangeTokenMismatch = "The change token is not valid anymore";

    public const string ChildNotAllowed = "Not allowed as a child entity";
  }
}