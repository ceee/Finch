namespace Mixtape.Extensions;

public static class ResultExtensions
{
  public static Result WithoutModel<TSource>(this Result<TSource> origin)
  {
    Result result = new();
    result.IsSuccess = origin.IsSuccess;
    result.Errors = origin.Errors;
    return result;
  }

  public static Result<TTarget> ConvertTo<TTarget>(this Result origin, TTarget model)
  {
    Result<TTarget> result = new();
    result.IsSuccess = origin.IsSuccess;
    result.Errors = origin.Errors;
    result.Model = model;
    return result;
  }


  public static Result AddError(this Result origin, string property, string message)
  {
    origin.IsSuccess = false;
    origin.Errors.Add(new(property, message));
    return origin;
  }


  public static Result AddError(this Result origin, string message)
  {
    origin.IsSuccess = false;
    origin.Errors.Add(new("__mixtape_no_field", message));
    return origin;
  }


  public static Result AppendErrors(this Result origin, Result with)
  {
    foreach (ResultError error in with.Errors)
    {
      origin.Errors.Add(error);
    }
    origin.IsSuccess = origin.Errors.Any();
    return origin;
  }
}