import Vue from 'vue';
import VueRouter from 'vue-router';
import Localization from 'zero/services/localization';
import { isArray as _isArray, find as _find, map as _map, filter as _filter } from 'underscore';
import { warn } from 'zero/services/debug';

Vue.use(VueRouter);

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
  addSection(section, () => import('zero/pages/' + section.alias + '/' + section.alias));

  if (section.children.length > 0)
  {
    section.children.forEach(child =>
    {
      addSection(child, () => import('zero/pages/' + section.alias + '/' + section.alias + '/' + child.alias), section);
    });
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

    // append routes to a section
    if (typeof definition.section === 'string')
    {
      const route = _find(routes, r => r.meta.alias === definition.section);

      if (!route)
      {
        if (_routes.length > 0)
        {
          warn(`router: Could not find section "${definition.section}" in route definition file ${path}`);
        }
      }
      else
      {
        if (!route.children)
        {
          route.children = [];
        }
        _routes.forEach(r =>
        {
          route.children.push(r);
        });
      }
    }
    // add routes to root
    else
    {
      _routes.forEach(r =>
      {
        routes.push(r);
      });
    }
  });
};



// add internal route extensions
addRoutesPerContext(require.context('zero/pages', true, /routes\.js$/));

// add plugin route extensions
//addRoutesPerContext(require.context('@/plugins', true, /routes\.js$/), true); // TODO use zero.pluginPath, but this fails




// add fallback route (this should probably by 404 page)

routes.push({ name: '404', path: '*', component: () => import('zero/pages/notfound') });




// create the router with history mode

const router = new VueRouter({
  mode: 'history',
  routes: routes,
  base: zero.path,
  linkActiveClass: 'is-active',
  linkExactActiveClass: 'is-active-exact',
  scrollBehavior(to, from, savedPosition)
  {
    return savedPosition ? savedPosition : { x: 0, y: 0 };
  }
});



// set meta title before routing

router.beforeEach((to, from, next) =>
{
  let title = Localization.localize('@zero.name');

  if (to.meta.name && to.meta.alias != zero.alias.sections.dashboard)
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
});


export default router;