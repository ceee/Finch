namespace zero.Api.Endpoints.Mails;

public class MailMapperProfile : ZeroMapperProfile
{
  public override void Configure(IZeroMapper mapper)
  {
    mapper.Define<MailTemplate, MailBasic>((source, ctx) => new(), Map);
    mapper.Define<MailTemplate, MailEdit>((source, ctx) => new(), Map);
    mapper.Define<MailSave, MailTemplate>((source, ctx) => new(), Map);
  }


  protected virtual void Map(MailTemplate source, MailBasic target, IZeroMapperContext ctx)
  {
    this.MapBasicData(source, target);
    target.Subject = source.Subject;
  }

  protected virtual void Map(MailTemplate source, MailEdit target, IZeroMapperContext ctx)
  {
    this.MapDisplayData(source, target);
    target.Subject = source.Subject;
    target.SenderEmail = source.SenderEmail;
    target.SenderName = source.SenderName;
    target.RecipientEmail = source.RecipientEmail;
    target.Cc = source.Cc;
    target.Bcc = source.Bcc;
    target.Body = source.Body;
    target.Preheader = source.Preheader;
}

  protected virtual void Map(MailSave source, MailTemplate target, IZeroMapperContext ctx)
  {
    this.MapSaveData(source, target);
    target.Subject = source.Subject;
    target.SenderEmail = source.SenderEmail;
    target.SenderName = source.SenderName;
    target.RecipientEmail = source.RecipientEmail;
    target.Cc = source.Cc;
    target.Bcc = source.Bcc;
    target.Body = source.Body;
    target.Preheader = source.Preheader;
  }
}