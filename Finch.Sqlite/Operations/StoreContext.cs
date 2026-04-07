using System;
using Finch.Communication;
using Finch.Configuration;
using Finch.Context;

namespace Finch.Sqlite;

public class StoreContext
{
  public IFinchContext Context { get; private set; }

  public IFinchOptions Options { get; private set; }

  public IServiceProvider Services { get; private set; }

  public IMessageAggregator Messages { get; private set; }


  public StoreContext(IFinchContext context, IServiceProvider serviceProvider, IMessageAggregator messages)
  {
    Options = context.Options;
    Context = context;
    Services = serviceProvider;
    Messages = messages;
  }
}