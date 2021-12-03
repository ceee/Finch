namespace zero.Identity.Models;

public enum LoginResult
{
  [Localize("@login.errors.unknown")]
  Unknown = 0,
  [Localize("@login.errors.wrongcredentials")]
  WrongCredentials = 1,
  [Localize("@login.errors.lockedout")]
  LockedOut = 2,
  [Localize("@login.errors.notallowed")]
  NotAllowed = 3,
  [Localize("@login.errors.requirestwofactor")]
  RequiresTwoFactor = 4,
  Success = 10
}