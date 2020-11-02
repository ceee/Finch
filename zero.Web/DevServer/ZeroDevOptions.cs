using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Zero.Web.DevServer
{
  public class ZeroDevOptions
  {
    public int Port { get; set; } = 3399;

    public bool ForwardLog { get; set; } = false;

    public string WorkingDirectory { get; set; }
  }
}
