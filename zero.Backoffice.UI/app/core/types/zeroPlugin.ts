
import { App } from 'vue';
import { Zero } from './zero';


export interface ZeroPlugin
{
  name: string;
  install: (app: App, zero: Zero) => void;
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