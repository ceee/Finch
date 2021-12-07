
import { RouteRecordRaw } from 'vue-router';
import { App } from 'vue';

export interface ZeroPluginOptions
{
  vue: App;
  routes: RouteRecordRaw[];
  route: (route: RouteRecordRaw) => void;
}

//export class Zero
//{
//  _app: App;
//  _options: ZeroInstallOptions;

//  get version(): string
//  {
//    return "0.0.1";
//  }

//  constructor(app: App, options?: ZeroInstallOptions)
//  {
//    this._app = app;
//    this._options = options || {};
//    console.info('[zero installed]');
//  }
//}