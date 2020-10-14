
class Plugin
{
  #name;
  #onMounted;

  routes = [];
  renderers = {};


  constructor(name)
  {
    this.#name = name;
  }


  get name()
  {
    return this.#name;
  }

  set mounted(callback)
  {
    this.#onMounted = callback;
  }
  

  /*
   * Add a new vue form renderer to the global configuration 
   */
  addRenderer(alias, renderer)
  {
    this.renderers[alias] = renderer;
  }

  /*
   * Add a new route to vue-router 
   */
  addRoute(route)
  {
    this.routes.push(route);
  }

  /*
   * Add a new route to vue-router 
   */
  addRoutes(routes)
  {
    routes.forEach(route =>
    {
      this.routes.push(route);
    });
  }
};

export default Plugin;