
import { App } from 'vue';
import { Router } from 'vue-router';
import { ZeroInstallOptions } from './types/zeroInstallOptions';
import { Zero } from './types/zero';
import registerDirectives from '../directives/register';
import registerComponents from '../components/register';

export class ZeroRuntime implements Zero
{
  _app: App;
  _options: ZeroInstallOptions;

  get version(): string
  {
    return "0.0.1";
  }

  constructor(app: App, options?: ZeroInstallOptions)
  {
    this._app = app;
    this._options = options || {};

    console.info('[zero installed]');
  }

  start()
  {
    registerDirectives(this._app);
    registerComponents(this._app);
  }
}