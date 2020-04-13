import Vue from 'vue';
import VueRouter from 'vue-router';
import Localization from 'zero/services/localization';
import { isArray as _isArray, find as _find } from 'underscore';
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

const sectionRoutes = require.context('zero/pages', true, /routes\.js$/);

sectionRoutes.keys().forEach((path) =>
{
  const routesDefinition = sectionRoutes(path);
  const definition = routesDefinition.default || routesDefinition;
  let _routes = _isArray(definition) ? definition : definition.routes;

  // append routes to a section
  if (typeof definition.section === 'string')
  {
    const route = _find(routes, r => r.meta.alias === definition.section);

    if (!route)
    {
      warn(`router: Could not find section "${definition.section}" in route definition file ${path}`);
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



// add fallback route (this should probably by 404 page)

routes.push({ path: '*', component: () => import('zero/pages/notfound') });



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
  if (to.meta.section)
  {
    document.title = Localization.localize(to.meta.section.name) + ' - zero';
  }
  else if (to.meta.name)
  {
    document.title = Localization.localize(to.meta.name) + ' - zero';
  }
  else
  {
    document.title = 'zero';
  }

  next();
});


export default router;