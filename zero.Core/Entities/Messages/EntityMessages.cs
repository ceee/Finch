namespace zero.Core.Entities.Messages
{
  public class EntityCreatedMessage : EntityMessageBase<IZeroEntity>
  {
    
  }

  public class EntityUpdatedMessage : EntityMessageBase<IZeroEntity>
  {

  }

  public class EntityDeletedMessage : EntityMessageBase<IZeroEntity>
  {

  }

  public class EntitySavedMessage<T> : EntityMessageBase<T>
  {
    public bool IsCreate { get; set; }

    public bool IsDelete { get; set; }
  }
}
