using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;
using zero.Commerce.Entities;
using zero.Core.Entities;
using zero.Core.Entities.Messages;
using zero.Core.Messages;
using zero.Debug.Sync;

namespace zero.TestData
{
  public class TestPluginStartup : IHostedService
  {
    IMessageAggregator Messages;


    public TestPluginStartup(IMessageAggregator messages)
    {
      Messages = messages;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
      Messages.Subscribe<EntitySavedMessage<ICountry>, CountryBlueprintHandler>();
      Messages.Subscribe<EntitySavedMessage<IProperty>, PropertyBlueprintHandler>();
      return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      return Task.CompletedTask;
    }
  }
}
