namespace zero.Core.Entities
{
  public class MediaRef : Ref
  {
    public bool IsCore { get; set; }

    public MediaRef() : base() { }
    public MediaRef(string id, bool isCore) : base(id)
    {
      IsCore = isCore;
    }

    public static implicit operator string(MediaRef reference) => reference.Id;

    public static implicit operator MediaRef(string id) => new MediaRef(id, false);
  }
}
