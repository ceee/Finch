namespace zero.Configuration;

public abstract class OptionsEnumerable<T>
{
  protected List<T> Items { get; set; } = new List<T>();


  public IReadOnlyCollection<T> GetAllItems()
  {
    return Items.AsReadOnly();
  }


  public void RemoveAt(int index)
  {
    Items.RemoveAt(index);
  }


  public bool Remove(T item)
  {
    return Items.Remove(item);
  }
}


public interface IOptionsEnumerable { }