namespace Finch.Validation;

public class FluentValidationGermanLanguage
{
	public static Dictionary<string, string> Translations = new()
	{
		{ "EmailValidator", "Keine gültige E-Mail-Adresse" },
    { "UrlValidator", "Keine gültige URL" },
    { "HexValidator", "Keine gültige HEX Farbangabe" },
    { "GreaterThanOrEqualValidator", "Der Wert muss grösser oder gleich '{ComparisonValue}' sein." },
		{ "GreaterThanValidator", "Der Wert muss grösser sein als '{ComparisonValue}'." },
		{ "LengthValidator", "Die Länge muss zwischen {MinLength} und {MaxLength} Zeichen liegen. Es wurden {TotalLength} Zeichen eingetragen." },
		{ "MinimumLengthValidator", "Die Länge muss größer oder gleich {MinLength} sein. Sie haben {TotalLength} Zeichen eingegeben." },
		{ "MaximumLengthValidator", "Die Länge muss kleiner oder gleich {MaxLength} sein. Sie haben {TotalLength} Zeichen eingegeben." },
		{ "LessThanOrEqualValidator", "Der Wert muss kleiner oder gleich '{ComparisonValue}' sein." },
		{ "LessThanValidator", "Der Wert muss kleiner sein als '{ComparisonValue}'." },
		{ "NotEmptyValidator", "Dieses Feld darf nicht leer sein." },
		{ "NotEqualValidator", "Dieser Wert darf nicht '{ComparisonValue}' sein." },
		{ "NotNullValidator", "Dieses Feld darf nicht leer sein." },
		{ "PredicateValidator", "Der Wert entspricht nicht der festgelegten Bedingung." },
		{ "AsyncPredicateValidator", "Der Wert entspricht nicht der festgelegten Bedingung." },
		{ "RegularExpressionValidator", "Dieses Feld weist ein ungültiges Format auf." },
		{ "EqualValidator", "Der Wert muss gleich '{ComparisonValue}' sein." },
		{ "ExactLengthValidator", "Dieses Feld muss genau {MaxLength} lang sein. Es wurden {TotalLength} eingegeben." },
		{ "ExclusiveBetweenValidator", "Der Wert muss zwischen {From} und {To} sein (exklusiv). Es wurde {PropertyValue} eingegeben." },
		{ "InclusiveBetweenValidator", "Der Wert muss zwischen {From} and {To} sein. Es wurde {PropertyValue} eingegeben." },
		{ "CreditCardValidator", "Keine gültige Kreditkartennummer." },
		{ "ScalePrecisionValidator", "Das Feld darf insgesamt nicht mehr als {ExpectedPrecision} Ziffern enthalten, mit Berücksichtigung von {ExpectedScale} Dezimalstellen. Es wurden {Digits} Ziffern und {ActualScale} Dezimalstellen gefunden." },
		{ "EmptyValidator", "Dieses Feld sollte leer sein." },
		{ "NullValidator", "Dieses Feld sollte leer sein." },
		{ "EnumValidator", "Dieses Feld hat einen Wertebereich, der '{PropertyValue}' nicht enthält." },
		// Additional fallback messages used by clientside validation integration.
		{ "Length_Simple", "Die Länge muss zwischen {MinLength} und {MaxLength} Zeichen liegen." },
		{ "MinimumLength_Simple", "Die Länge muss größer oder gleich {MinLength} sein." },
		{ "MaximumLength_Simple", "Die Länge muss kleiner oder gleich {MaxLength} sein." },
		{ "ExactLength_Simple", "Dieses Feld muss genau {MaxLength} lang sein." },
		{ "InclusiveBetween_Simple", "Der Wert muss zwischen {From} and {To} sein." },
		{ "CaptchaValidator", "Das Catpcha wurde nicht erfolgreich gelöst." }
	};
}