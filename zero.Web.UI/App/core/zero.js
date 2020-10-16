
// ref: 
// https://github.com/vuejs/vue-router/blob/dev/src/index.js

import Axios from 'axios';
import ZeroPlugin from './plugin.zero.js';
import EventHub from '../services/eventhub.js';
import VueRouter from 'vue-router';
import Vue from 'vue';
//import CommercePlugin from '../../../zero.Commerce/Plugins/zero.Commerce/plugin.js'; // TODO dynPath
import options from './options.js';
import routerConfig from '../config/router.config.js'

class Zero
{
  static install;

  config = { ...options };

  #vue = null;
  #plugins = [];
  #routes = [];
  #router = null;


  constructor(vue, opts)
  {
    this.config = { ...options, ...opts };
    this.#vue = vue;

    this.use(ZeroPlugin);
    //this.use(CommercePlugin);
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

    console.info(this.#routes);

    //const result = await Axios.get('zerovue/config');
    //this.config = { ...this.config, ...result.data };

    //console.info(this.#vue.router);

    //EventHub.$emit('zero.setup');
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
    plugin.routes.forEach(route =>
    {
      this.#routes.push(route);
    });

    console.log(`[zero] Installed %c${plugin.name}%cplugin`, 'font-style:italic;');
  }
};


/*
 * Registers zero within the vue system
 */
Zero.install = (vue, opts) =>
{
  const zero = new Zero(vue, opts);

  Object.defineProperty(vue.prototype, 'zero', {
    get: () => zero
  });

  zero.setup();

  Vue.mixin({
    router: zero.router
  });
};


export default Zero;