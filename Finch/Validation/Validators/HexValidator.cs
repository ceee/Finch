using System.Text.RegularExpressions;

namespace Finch.Validation.Validators;

public class HexValidator<T> : RegexValidator<T>
{
  public override string Name => "HexValidator";

  static Regex Regex = CreateRegex("(^$)|(#[0-9a-fA-F]{3,8})");

  public HexValidator() : base(Regex) { }
}