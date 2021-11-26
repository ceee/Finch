namespace zero.Backoffice.Models;

public abstract class BasicModel<T> : ZeroIdEntity where T : ZeroIdEntity
{
  public string Name { get; set; } 
}