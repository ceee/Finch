using System;
using zero.Core.Blueprints;
using zero.Core.Entities;

namespace zero.Core.Options
{
  public class BlueprintOptions : ZeroBackofficeCollection<Blueprint>, IZeroCollectionOptions
  {
    public BlueprintOptions()
    {
      Enabled = false;
      Add<Media>();
      Add<MediaFolder>();
      Add<Language>();
      Add<Country>();
      Add<Translation>();
      Add<MailTemplate>();
    }


    public bool Enabled { get; set; }


    public void Add<T>() where T : Blueprint, new()
    {
      Items.Add(new T());
    }


    public void Add<T>(Blueprint<T> implementation) where T : ZeroEntity, new()
    {
      Items.Add(implementation);
    }


    public void Add<T>(Action<Blueprint<T>> createExpression = null) where T : ZeroEntity, new()
    {
      Items.Add(new DefaultBlueprint<T>(createExpression));
    }
  }
}
