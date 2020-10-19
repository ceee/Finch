
class Plugin
{
  #name;
  #onInstall;

  routes = [];
  editors = [];
  lists = [];


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
  addEditor(config)
  {
    this.editors.push(config);
  }


  /*
   * Adds new form renderers to the global configuration 
   * Can either by an array (where a key of the child object is `alias`) or an object where the key is the renderer alias
   */
  addEditors(editors)
  {
    let items = !Array.isArray(editors) ? Object.values(editors) : editors;
    items.forEach(item => this.addEditor(item));
  }


  /*
   * Add a new vue list renderer to the global configuration 
   */
  addList(config)
  {
    this.lists.push(config);
  }


  /*
   * Adds new list renderers to the global configuration
   * Can either by an array (where a key of the child object is `alias`) or an object where the key is the renderer alias
   */
  addLists(lists)
  {
    let items = !Array.isArray(lists) ? Object.values(lists) : lists;
    items.forEach(item => this.addList(item));
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