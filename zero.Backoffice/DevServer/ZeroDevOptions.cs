namespace zero.Backoffice.DevServer;

public class ZeroDevOptions
{
  public int Port { get; set; } = 3399;

  public bool ForwardLog { get; set; } = false;

  public string WorkingDirectory { get; set; }

  public bool Enabled { get; set; } = true;
}
