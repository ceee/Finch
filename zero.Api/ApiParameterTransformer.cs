using Microsoft.AspNetCore.Routing;

namespace zero.Api;

public class ApiParameterTransformer : IOutboundParameterTransformer
{
  public string TransformOutbound(object value)
  {
    return value == null ? null : Safenames.Alias(value);
  }
}