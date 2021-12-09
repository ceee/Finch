
import { App } from 'vue';
import { ZeroInstallOptions } from './types/zeroInstallOptions';
import { ZeroPluginOptions } from './types/zeroPluginOptions';
import { Zero } from './types/zero';
import { createRouter, RouteRecordRaw, RouterOptions } from 'vue-router';
import registerDirectives from '../directives/register';
import registerComponents from '../components/register';
import registerFormComponents from '../forms/register';
import { getRouterConfig, appendRouterGuards } from './router/routerConfig';
import { countryPlugin, applicationPlugin } from '../modules';

export class ZeroRuntime implements Zero
{
  _app: App;
  _options: ZeroInstallOptions;
  _routerConfig: RouterOptions;

  /**
   * version of zero backoffice
   **/
  get version(): string
  {
    return "0.0.1";
  }

  constructor(app: App, options?: ZeroInstallOptions)
  {
    this._app = app;
    this._options = options || {};
    this._routerConfig = getRouterConfig('/zero', this);
  }


  /**
   * register core components
   **/
  useZero()
  {
    let app = this._app;

    registerDirectives(app);
    registerComponents(app);
    registerFormComponents(app);
  }


  /**
   * create plugin options which are passed to install() for all plugins
   **/
  usePlugins()
  {
    const pluginOptions = {
      vue: this._app,
      routes: this._routerConfig.routes,
      route(route: RouteRecordRaw)
      {
        this.routes.push(route);
      }
    } as ZeroPluginOptions;

    // install all plugins
    countryPlugin.install(pluginOptions);
    applicationPlugin.install(pluginOptions);
  }


  /**
   * after all plugins were installed we can create the router 
   * with the designated routes
   **/
  useRouter()
  {
    const router = createRouter(this._routerConfig);
    appendRouterGuards(router);
    this._app.use(router);
  }
}