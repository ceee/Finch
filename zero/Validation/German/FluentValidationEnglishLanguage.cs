namespace zero.Validation;

public class FluentValidationEnglishLanguage
{
	public static Dictionary<string, string> Translations = new()
	{
		{ "EmailValidator", "Invalid email address" },
    { "UrlValidator", "Not a well-formed URL" },
    { "HexValidator", "Invalid HEX value" },
		{ "CaptchaValidator", "The captcha was not solved" }
	};
}