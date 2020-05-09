using System;
using System.Linq;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Extensions;
using zero.Web.Models;
using Raven.Client.Documents;
using zero.Core.Mapper;

namespace zero.Web.Mapper
{
  public class SpaceMapperConfig : IMapperConfig
  {
    /// <inheritdoc />
    public void Configure(IMapper config)
    {
      //config.CreateMap<SpaceContentEditModel, SpaceContent>((source, target) =>
      //{
      //  target.Id = source.Id;
      //  target.Name = source.Name;
      //  target.IsActive = source.IsActive;
      //  target.CreatedDate = source.CreatedDate;
      //  target.
      //});
    }
  }
}
