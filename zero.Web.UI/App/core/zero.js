
// ref: 
// https://github.com/vuejs/vue-router/blob/dev/src/index.js

import ZeroPlugin from './plugin.zero.js';
import CommercePlugin from '../../../zero.Commerce/Plugins/zero.Commerce/plugin.js'; // TODO dynPath
import options from './options.js';

class Zero
{
  static install;

  config = { ...options };

  #vue = null;
  #plugins = [];


  constructor(vue, opts)
  {
    this.config = { ...options, ...opts };
    this.#vue = vue;

    this.use(ZeroPlugin);
    this.use(CommercePlugin);
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
   * Locates zero plugins and install them
   */
  setup()
  {
    
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
};


export default Zero;