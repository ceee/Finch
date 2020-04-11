import Vue from 'vue';
import VueRouter from 'vue-router';
import Localization from 'zero/services/localization';

Vue.use(VueRouter);

const routes = [];



// add defined backoffice sections (with their children) to the router

let addSection = (section, component) =>
{
  let route = {
    path: section.url,    
    component: component,
    meta: {
      section: section
    }
  };

  routes.push(route);

  return route;
};

zero.sections.forEach(section =>
{
  let route = addSection(section, () => import('zero/pages/' + section.alias + '/' + section.alias));

  if (section.alias === 'pages')
  {
    route.children = [{
      path: 'edit/:id',
      props: true,
      name: 'page',
      component: () => import('zero/pages/' + section.alias + '/page')
    }];
  }

  if (section.children.length > 0)
  {
    section.children.forEach(child =>
    {
      addSection(child, () => import('zero/pages/' + section.alias + '/' + section.alias + '/' + child.alias));
    });
  }
});



// add fallback route (this should probably by 404 page)

//routes.push({ path: '*', component: ViewDefault });



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
  else
  {
    document.title = 'zero';
  }

  next();
});


export default router;