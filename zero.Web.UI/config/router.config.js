
import { createRouter, createWebHistory } from "vue-router";
import Localization from '@zero/services/localization.js';
import { isArray as _isArray, find as _find, map as _map, filter as _filter } from 'underscore';
import { warn } from '@zero/services/debug.js';

const routes = [];


// add defined backoffice sections (with their children) to the router

let addSection = (section, component, parent) =>
{
  let route = {
    path: section.url,    
    component: component,
    name: (parent ? parent.alias + '-' : '') + section.alias,
    meta: {
      name: [ section.name, (parent ? parent.name : null) ],
      alias: section.alias,
      section: section,
      parent: parent
    }
  };

  routes.push(route);

  return route;
};

zero.sections.forEach(section =>
{
  if (!section.isExternal)
  {
    addSection(section, () => import('@zero/pages/' + section.alias + '/' + section.alias));

    if (section.children.length > 0)
    {
      section.children.forEach(child =>
      {
        addSection(child, () => import('@zero/pages/' + section.alias + '/' + section.alias + '/' + child.alias), section);
      });
    }
  }
});


// add additional routes

routes.push({
  path: '/preview',
  component: () => import('@zero/pages/preview'),
  name: 'preview',
  meta: {
    name: '@preview.name'
  }
});


// find internal route definitions per section

let addRoutesPerContext = (context, isPlugin) =>
{
  context.keys().forEach((path) =>
  {
    const routesDefinition = context(path);
    const definition = routesDefinition.default || routesDefinition;
    let _routes = _isArray(definition) ? definition : definition.routes;

    _routes.forEach(route =>
    {
      const parentAlias = route.section || definition.section;
      const isChild = typeof parentAlias === 'string';
      const parentRoute = isChild ? _find(routes, r => r.meta.alias === parentAlias) : null;

      // could not append routes to a section
      if (isChild && !parentRoute)
      {
        if (_routes.length > 0)
        {
          warn(`router: Could not find section "${definition.section}" in route definition file ${path}`);
        }
      }
      // append routes to a section
      else if (isChild)
      {
        if (!parentRoute.children)
        {
          parentRoute.children = [];
        }
        parentRoute.children.push(route);
      }
        // add routes to root
      else
      {
        routes.push(route);
      }
    });
  });
};



// add internal route extensions
//addRoutesPerContext(require.context('zero/pages', true, /routes\.js$/));

// add plugin route extensions
//try
//{
//  addRoutesPerContext(require.context('@/../zero.Commerce/Plugins/zero.Commerce', true, /routes\.js$/), true); // TODO dynPATH
//}
//catch (exc)
//{
//  // TODO error will throw when user is not logged in as "section" in routes.js is null
//}
//addRoutesPerContext(require.context('@/plugins', true, /routes\.js$/), true); // TODO use zero.pluginPath, but this fails




// add fallback route (this should probably by 404 page)

routes.push({ name: '404', path: '/:pathMatch(.*)*', component: () => import('@zero/pages/notfound') });



// set meta title before routing

const beforeEach = (to, from, next) =>
{
  let isGuarded = false;


  // set document title + call next
  let callback = () =>
  {
    let title = Localization.localize('@zero.name');

    if (to.meta.name && to.meta.alias !== zero.alias.sections.dashboard)
    {
      let name = to.meta.name;
      let nameParts = _isArray(name) ? name : [name];
      let translations = [];

      nameParts.forEach(part =>
      {
        if (part)
        {
          translations.push(Localization.localize(part));
        }
      });

      title += ': ' + translations.join(' - ');
    }

    document.title = title;

    next();
  };

  // dirty form guard
  if (from.matched.length && from.matched[0].instances)
  {
    let instance = from.matched[0].instances.default;

    if (instance.$refs['form'] && typeof instance.$refs.form.beforeRouteLeave === 'function')
    {
      isGuarded = true;
      instance.$refs.form.beforeRouteLeave(to, from, res =>
      {
        if (res === false)
        {
          next(false);
        }
        else
        {
          callback();
        }
      });
    }
  }

  if (!isGuarded)
  {
    callback();
  }
};



// create the router with history mode
export default createRouter({
  history: createWebHistory(zero.path),
  routes: routes,
  linkActiveClass: 'is-active',
  linkExactActiveClass: 'is-active-exact',
  beforeEach: beforeEach,
  scrollBehavior(to, from, savedPosition)
  {
    return savedPosition ? savedPosition : { x: 0, y: 0 };
  }
});