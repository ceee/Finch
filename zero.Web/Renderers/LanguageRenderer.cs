//using FluentValidation;
//using System.Threading.Tasks;
//using zero.Core.Entities;
//using zero.Core.Renderer;

//namespace zero.Web.Renderers
//{
//  public class LanguageRenderer : AbstractRenderer<Language>
//  {
//    public LanguageRenderer()
//    {
//      Field(x => x.Name).Text();
//      Field(x => x.Code).Text();
//      Field(x => x.IsDefault).Toggle();

//      RuleFor(x => x.Name).NotEmpty();
//    }
//  }
//}