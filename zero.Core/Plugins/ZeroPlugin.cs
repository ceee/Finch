using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using zero.Core.Entities;
using zero.Core.Identity;
using zero.Core.Mapper;

namespace zero.Core.Plugins
{
  public abstract class ZeroPlugin
  {
    protected virtual void Configure(IServiceCollection services, IZeroPluginBuilder builder) { }
  }


  public interface IZeroPlugin
  {
    void Configure(IServiceCollection services, IZeroPluginBuilder builder);
  }
}