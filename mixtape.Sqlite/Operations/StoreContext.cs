using System;
using Mixtape.Communication;
using Mixtape.Configuration;
using Mixtape.Context;

namespace Mixtape.Sqlite;

public class StoreContext
{
  public IMixtapeContext Context { get; private set; }

  public IMixtapeOptions Options { get; private set; }

  public IServiceProvider Services { get; private set; }

  public IMessageAggregator Messages { get; private set; }


  public StoreContext(IMixtapeContext context, IServiceProvider serviceProvider, IMessageAggregator messages)
  {
    Options = context.Options;
    Context = context;
    Services = serviceProvider;
    Messages = messages;
  }
}