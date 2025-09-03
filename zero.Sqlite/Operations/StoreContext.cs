using System;
using zero.Communication;
using zero.Configuration;
using zero.Context;

namespace zero.Sqlite;

public class StoreContext
{
  public IZeroContext Context { get; private set; }

  public IZeroOptions Options { get; private set; }

  public IServiceProvider Services { get; private set; }

  public IMessageAggregator Messages { get; private set; }


  public StoreContext(IZeroContext context, IServiceProvider serviceProvider, IMessageAggregator messages)
  {
    Options = context.Options;
    Context = context;
    Services = serviceProvider;
    Messages = messages;
  }
}