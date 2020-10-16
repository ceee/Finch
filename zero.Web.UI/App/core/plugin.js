
class Plugin
{
  #name;
  #onInstall;

  routes = [];
  renderers = [];


  constructor(name)
  {
    this.#name = name;
  }


  get name()
  {
    return this.#name;
  }

  get install()
  {
    return this.#onInstall;
  }

  set install(callback)
  {
    this.#onInstall = callback;
  }


  /*
   * Add a new vue form renderer to the global configuration 
   */
  addRenderer(alias, config)
  {
    config.alias = alias;
    this.renderers.push(config);
  }

  /*
   * Adds new form renderers to the global configuration 
   * Can either by an array (where a key of the child object is `alias`) or an object where the key is the renderer alias
   */
  addRenderers(renderers)
  {
    let items = !Array.isArray(renderers) ? Object.values(renderers) : renderers;
    items.forEach(item => this.addRenderer(item.alias, item));
  }

  /*
   * Add a new route to vue-router 
   */
  addRoute(route)
  {
    this.routes.push(route);
  }

  /*
   * Adds new routes to vue-router 
   */
  addRoutes(routes)
  {
    routes.forEach(route => this.addRoute(route));
  }
};

export default Plugin;