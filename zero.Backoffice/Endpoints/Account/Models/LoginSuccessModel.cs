namespace zero.Backoffice.Endpoints.Account;

public class LoginSuccessModel : LoginSuccessMinimalModel
{
  public UserModel User { get; set; } 
}

public class LoginSuccessMinimalModel
{
  public string ApiKey { get; set; }
}