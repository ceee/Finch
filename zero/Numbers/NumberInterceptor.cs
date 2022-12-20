using Raven.Client.Documents;

namespace zero.Commerce.Numbers;

public class NumberInterceptor : Interceptor<Number>
{
  protected IZeroStore Store { get; private set; }


  public NumberInterceptor(IZeroStore store)
  {
    Store = store;
  }


  /// <inheritdoc />
  public override Task<InterceptorResult<Number>> Creating(InterceptorParameters args, Number model) => Saving(args, model, false);

  /// <inheritdoc />
  public override Task<InterceptorResult<Number>> Updating(InterceptorParameters args, Number model) => Saving(args, model, true);


  protected async Task<InterceptorResult<Number>> Saving(InterceptorParameters args, Number model, bool update)
  {
    string flavor = model.Flavor;
    string existingId = await args.Operations.Session.Query<Number>().Where(x => x.Flavor == flavor).Select(x => x.Id).FirstOrDefaultAsync();

    if (!existingId.IsNullOrEmpty() && existingId != model.Id)
    {
      return new()
      {
        Result = Result<Number>.Fail("@shop.errors.number.multiplenotallowed")
      };
    }

    model.Alias = flavor;
    model.IsActive = true;
    model.Name = null;

    return default;
  }
}