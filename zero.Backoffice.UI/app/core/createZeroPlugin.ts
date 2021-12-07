
import { App } from 'vue';
import { ZeroRuntime, ZeroInstallOptions } from './index';


export function createZeroPlugin(options?: ZeroInstallOptions)
{
  return {
    install: (app: App) =>
    {
      const zero = new ZeroRuntime(app, options);
      app.config.globalProperties.zero = zero;

      zero.useZero();
      zero.usePlugins();
      zero.useRouter();
    }
  };
}