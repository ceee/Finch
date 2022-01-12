
import { App, Component } from 'vue';
import { ZeroInstallOptions } from './types/zeroInstallOptions';
import { ZeroPluginOptions } from './types/zeroPluginOptions';
import { Zero, ZeroOptions } from './types/zero';
import { createRouter, RouteRecordRaw, RouterOptions } from 'vue-router';
import registerDirectives from '../directives/register';
import registerComponents from '../components/register';
import registerEditorComponents from '../editor/register';
import { getRouterConfig, appendRouterGuards } from './router/routerConfig';
import
{
  countryPlugin,
  applicationPlugin,
  settingsPlugin,
  languagePlugin,
  mediaPlugin,
  spacePlugin,
  pagePlugin,
  mailTemplatePlugin,
  translationPlugin,
  integrationPlugin,
  userPlugin
} from '../modules';
import editorPlugin from '../editor/plugin';
import { ZeroSchema } from 'zero/schemas';
import { ZeroSchemaProp } from './zero';
import * as zeroOptions from '../options';
import plugins from '../plugins.generated';
import eventHub from '../services/eventhub';
import { Emitter, EventType } from 'mitt';

plugins.push(
  editorPlugin, countryPlugin, applicationPlugin, settingsPlugin, languagePlugin,
  mediaPlugin, spacePlugin, pagePlugin, mailTemplatePlugin,
  translationPlugin, integrationPlugin, userPlugin
);


export class ZeroRuntime implements Zero
{
  _app: App;
  _installOptions: ZeroInstallOptions;
  _routerConfig: RouterOptions;
  _schemas: Record<string, ZeroSchemaProp> = {};
  _fieldTypes: Record<string, Component> = {};

  /**
   * version of zero backoffice
   **/
  get version(): string
  {
    return "0.0.1";
  }

  /**
   * access the event hub
   **/
  get events(): Emitter<Record<EventType, unknown>>
  {
    return eventHub;
  }

  /**
   * options
   **/
  options: ZeroOptions;

  /**
   * runtime variables
   **/
  runtimeVariables: Record<string, any> = {};

  constructor(app: App, options?: ZeroInstallOptions)
  {
    this._app = app;
    this._installOptions = options || {};
    this._routerConfig = getRouterConfig('/zero', this);
    this.options = zeroOptions;
  }


  /**
   * register core components
   **/
  useZero()
  {
    let app = this._app;

    registerDirectives(app);
    registerComponents(app);
    registerEditorComponents(app);
  }


  /**
   * create plugin options which are passed to install() for all plugins
   **/
  usePlugins()
  {
    const pluginOptions = {
      vue: this._app,
      routes: this._routerConfig.routes,
      schemas: this._schemas,
      fieldTypes: this._fieldTypes,
      route(route: RouteRecordRaw)
      {
        this.routes.push(route);
      },
      schema(alias: string, schema: ZeroSchemaProp)
      {
        this.schemas[alias] = schema;
      },
      fieldType(alias: string, component: Component)
      {
        this.fieldTypes[alias] = component;
      }
    } as ZeroPluginOptions;

    // install all plugins
    plugins.forEach(plugin =>
    {
      plugin.install(pluginOptions);
    });
  }


  runPlugins()
  {
    plugins.forEach(plugin =>
    {
      if (typeof plugin.run === 'function')
      {
        plugin.run(this);
      }
    });
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


  /**
   * get a defined field type component
   **/
  getFieldTypeComponent(alias: string): Component | undefined
  {
    return this._fieldTypes[alias];
  }


  /**
   * get a defined list or editor schema
   **/
  async getSchema(alias: string): Promise<ZeroSchema | null>
  {
    const schema: ZeroSchemaProp = this._schemas[alias];

    if (!schema)
    {
      return Promise.resolve(null);
    }

    if (typeof schema === 'function')
    {
      const res = await schema();
      return res.default;
    }

    return Promise.resolve(schema);
  }
}