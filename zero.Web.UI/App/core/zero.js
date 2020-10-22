
// ref: 
// https://github.com/vuejs/vue-router/blob/dev/src/index.js

import Axios from 'axios';
import ZeroPlugin from './plugin.zero.js';
import EventHub from '../services/eventhub.js';
import VueRouter from 'vue-router';
import Vue from 'vue';
import CommercePlugin from '../../../zero.Commerce/Plugins/zero.Commerce/plugin.js'; // TODO dynPath
import TestPlugin from '../../../../Laola/Laola.Backoffice/Plugin/plugin.js'; // TODO dynPath
import options from './options.js';
import routerConfig from '../config/router.config.js'

class Zero
{
  static install;
  static instance;

  config = { ...options };

  #vue = null;
  #plugins = [];
  #editors = [];
  #lists = [];
  #routes = [];
  #router = null;
  #setupDone = false;


  constructor(vue, opts)
  {
    this.config = { ...options, ...opts };
    this.#vue = vue;

    this.use(ZeroPlugin);
    this.use(CommercePlugin);
    this.use(TestPlugin);
  }


  /*
   * Get the currently installed version of the zero frontend
   */
  get version()
  {
    return this.config.version;
  }

  /*
   * Get all installed zero plugins
   */
  get plugins()
  {
    return this.#plugins;
  }


  /*
   * Get all installed zero plugins
   */
  get router()
  {
    return this.#router;
  }


  /*
   * Locates zero plugins and install them
   */
  setup()
  {
    this.#router = new VueRouter({
      ...routerConfig,
      routes: this.#routes
    });

    //const result = await Axios.get('zerovue/config');
    //this.config = { ...this.config, ...result.data };

    //console.info(this.#vue.router);

    this.#setupDone = true;
    //EventHub.$emit('zero.setup');
  }


  /*
   * Installs a zero plugin by a given path
   * Each plugin is a superset of a vue (client) plugin and can therefore hook into the vue system
   */
  useByPath(pluginPath)
  {
    //const plugin = import('../../../' + pluginPath + '/plugin.js').then(res =>
    //{
    //  this.use(res.default);
    //});
  }


  /*
   * Installs a zero plugin
   * Each plugin is a superset of a vue (client) plugin and can therefore hook into the vue system
   */
  use(plugin)
  {
    if (typeof plugin.install === 'function')
    {
      plugin.install(this.#vue, this);
    }

    this.#plugins.push(plugin);

    // append routes
    if (this.#setupDone)
    {
      this.#router.addRoutes(plugin.routes);
    }
    else
    {
      plugin.routes.forEach(x => this.#routes.push(x));
    }

    // append editors
    plugin.editors.forEach(x => this.#editors.push(x));

    // append lists
    plugin.lists.forEach(x => this.#lists.push(x));

    console.log(`[zero] Installed %c${plugin.name}%cplugin`, 'font-style:italic;');
  }


  /*
   * Returns an editor renderer
   */
  getEditor(alias)
  {
    const renderer = this.#editors.find(x => x.alias === alias);

    if (!renderer)
    {
      console.warn(`[zero] Could not find editor renderer ${alias}`);
    }

    return renderer;
  }


  /*
  * Returns a list renderer
  */
  getList(alias)
  {
    const renderer = this.#lists.find(x => x.alias === alias);

    if (!renderer)
    {
      console.warn(`[zero] Could not find list renderer ${alias}`);
    }

    return renderer;
  }
};


/*
 * Registers zero within the vue system
 */
Zero.install = (vue, opts) =>
{
  const zero = new Zero(vue, opts);

  Zero.instance = zero;

  Object.defineProperty(vue.prototype, 'zero', {
    get: () => zero
  });

  zero.setup();

  console.log('[zero] Setup completed');

  // add plugins
  //__zero.plugins.filter(x => !!x.pluginPath).forEach(x =>
  //{
  //  zero.useByPath(x.pluginPath);
  //});

  //import('@/../zero.Commerce/Plugins/zero.Commerce/plugin.js').then(res =>
  //{
  //  zero.use(res.default);
  //});
};

export default Zero;