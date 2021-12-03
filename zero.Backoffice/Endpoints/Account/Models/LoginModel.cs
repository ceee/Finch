using System.ComponentModel.DataAnnotations;

namespace zero.Backoffice.Endpoints.Account;

public class LoginModel
{
  [Required, EmailAddress]
  public string Email { get; set; }

  [Required]
  public string Password { get; set; }

  public bool IsPersistent { get; set; } = true;
}