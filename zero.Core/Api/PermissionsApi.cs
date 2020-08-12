using System.Collections.Generic;
using System.Linq;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Options;

namespace zero.Core.Api
{
  public class PermissionsApi : IPermissionsApi
  {
    protected IZeroOptions Options { get; set; }


    public PermissionsApi(IZeroOptions options)
    {
      Options = options;
    }


    /// <inheritdoc />
    public IList<PermissionCollection> GetAll()
    {
      List<PermissionCollection> result = Options.Permissions.GetAllItems().ToList();
      PermissionCollection spaceCollection = result.FirstOrDefault(x => x.Alias == Constants.PermissionCollections.Spaces);

      if (spaceCollection != null)
      {
        spaceCollection.Items.Clear();

        foreach (Space space in Options.Spaces.GetAllItems())
        {
          spaceCollection.Items.Add(new Permission(Permissions.Spaces.PREFIX + space.Alias, space.Name, null, PermissionValueType.CRUD));
        }
      }

      return result;
    }
  }


  public interface IPermissionsApi
  {
    /// <summary>
    /// Get all available permissions to choose from
    /// </summary>
    IList<PermissionCollection> GetAll();
  }
}
