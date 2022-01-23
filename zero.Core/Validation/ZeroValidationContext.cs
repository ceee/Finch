namespace zero.Validation;

public class ZeroValidationContext
{
  public IZeroContext Context { get; set; }

  public IZeroDocumentSession Session { get; set; }

  public ValidationOp Operation { get; set; }
}

public enum ValidationOp
{
  Unknown = 0,
  Create = 1,
  Update = 2
}