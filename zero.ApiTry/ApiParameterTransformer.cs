using zero.Persistence;

namespace zero.ApiTry;

public class ApiParameterTransformer : IOutboundParameterTransformer
{
  public string? TransformOutbound(object? value)
  {
    return value == null ? null : Safenames.Alias(value);
  }
}