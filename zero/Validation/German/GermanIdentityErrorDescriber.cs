using Microsoft.AspNetCore.Identity;

namespace zero.Validation;

public class GermanIdentityErrorDescriber : IdentityErrorDescriber
{
  public override IdentityError DefaultError() { return new IdentityError { Code = nameof(DefaultError), Description = $"Ein unbekannter Fehler ist aufgetreten" }; }
  public override IdentityError ConcurrencyFailure() { return new IdentityError { Code = nameof(ConcurrencyFailure), Description = "Ein unbekannter Fehler ist aufgetreten" }; }
  public override IdentityError PasswordMismatch() { return new IdentityError { Code = nameof(PasswordMismatch), Description = "Die Passwörter stimmen nicht überein" }; }
  public override IdentityError InvalidToken() { return new IdentityError { Code = nameof(InvalidToken), Description = "Der Token ist ungültig" }; }
  public override IdentityError LoginAlreadyAssociated() { return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "Ein Benutzer mit diesem Login existiert bereits" }; }
  public override IdentityError InvalidUserName(string userName) { return new IdentityError { Code = nameof(InvalidUserName), Description = $"Der Username '{userName}' ist ungültig (nur Buchstaben und Zahlen gültig)" }; }
  public override IdentityError InvalidEmail(string email) { return new IdentityError { Code = nameof(InvalidEmail), Description = $"Die Email-Adresse '{email}' ist ungültig" }; }
  public override IdentityError DuplicateUserName(string userName) { return new IdentityError { Code = nameof(DuplicateUserName), Description = $"Der Username '{userName}' ist bereits vergeben" }; }
  public override IdentityError DuplicateEmail(string email) { return new IdentityError { Code = nameof(DuplicateEmail), Description = $"Die Email-Adresse '{email}' ist bereits vergeben" }; }
  //public override IdentityError InvalidRoleName(string role) { return new IdentityError { Code = nameof(InvalidRoleName), Description = $"Role name '{role}' is invalid." }; }
  //public override IdentityError DuplicateRoleName(string role) { return new IdentityError { Code = nameof(DuplicateRoleName), Description = $"Role name '{role}' is already taken." }; }
  //public override IdentityError UserAlreadyHasPassword() { return new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = "User already has a password set." }; }
  //public override IdentityError UserLockoutNotEnabled() { return new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = "Lockout is not enabled for this user." }; }
  //public override IdentityError UserAlreadyInRole(string role) { return new IdentityError { Code = nameof(UserAlreadyInRole), Description = $"User already in role '{role}'." }; }
  //public override IdentityError UserNotInRole(string role) { return new IdentityError { Code = nameof(UserNotInRole), Description = $"User is not in role '{role}'." }; }
  public override IdentityError PasswordTooShort(int length) { return new IdentityError { Code = nameof(PasswordTooShort), Description = $"Das Passwort muss mindestens {length} Zeichen lang sein" }; }
  public override IdentityError PasswordRequiresNonAlphanumeric() { return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Passwörter müssen zumindest ein alphanumerisches Zeichen (a-z, 0-9) enthalten" }; }
  public override IdentityError PasswordRequiresDigit() { return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "Passwörter müssen zumindest eine Zahl enthalten ('0'-'9')" }; }
  public override IdentityError PasswordRequiresLower() { return new IdentityError { Code = nameof(PasswordRequiresLower), Description = "Passwörter müssen zumindest einen Kleinbuchstaben enthalten ('a'-'z')" }; }
  public override IdentityError PasswordRequiresUpper() { return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "Passwörter müssen zumindest einen Großbuchstaben enthalten ('A'-'Z')" }; }
  public override IdentityError PasswordRequiresUniqueChars(int uniqueChars) { return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = $"Passwörter müssen zumindest {uniqueChars} verschiedene Zeichen enthalten" }; }
  //public override IdentityError RecoveryCodeRedemptionFailed() { return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "Passwords must have at least one uppercase ('A'-'Z')." }; }
}