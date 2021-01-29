//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace zero.Core.Entities
//{
//  /// <inheritdoc />
//  public class LinkArea : ILinkArea
//  {
//    /// <inheritdoc />
//    public string Alias { get; set; }

//    /// <inheritdoc />
//    public string Name { get; set; }

//    /// <inheritdoc />
//    public string Icon { get; set; }

//    /// <summary>
//    /// HEX color (#aabbcc or #abc). Defaults to a neutral color
//    /// </summary>
//    public string Color { get; set; }

//    /// <inheritdoc />
//    public IList<IChildSection> Children { get; } = new List<IChildSection>();


//    public LinkArea() { }

//    public LinkArea(string alias, string name, string icon = null, string color = null)
//    {
//      Alias = alias;
//      Name = name;
//      Icon = icon;
//      Color = color;
//    }
//  }


//  /// <summary>
//  /// A section is a main part of the backoffice application
//  /// </summary>
//  public interface ILinkArea
//  {
//    /// <summary>
//    /// The section alias which acts as the url slug for navigation
//    /// </summary>
//    string Alias { get; }

//    /// <summary>
//    /// The name of the section (either a string or a translation key with @ prefix)
//    /// </summary>
//    string Name { get; }

//    /// <summary>
//    /// Icon of the section
//    /// </summary>
//    string Icon { get; }

//    /// <summary>
//    /// HEX color (#aabbcc or #abc)
//    /// </summary>
//    string Color { get; }

//    /// <summary>
//    /// Children are displayed as a sub-navigation in the main nav area
//    /// </summary>
//    IList<IChildSection> Children { get; }
//  }
//}
