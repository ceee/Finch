import Vue from 'vue';
import VueRouter from 'vue-router';
import Localization from 'zeroservices/localization';

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

  //if (section.alias != 'pages')
  //{
  //  route.component = component;
  //}
  //else
  //{
  //  route.components = {
  //    default: component,
  //    footer: () => import('zeropages/' + section.alias + '/user')
  //  };
  //}

  routes.push(route);

  return route;
};

zero.sections.forEach(section =>
{
  let route = addSection(section, () => import('zeropages/' + section.alias + '/' + section.alias));

  if (section.alias === 'pages')
  {
    route.children = [{
      path: ':page',
      props: true,
      name: 'page',
      component: () => import('zeropages/' + section.alias + '/page')
    }];

  }

  if (section.children.length > 0)
  {
    section.children.forEach(child =>
    {
      addSection(child, () => import('zeropages/' + section.alias + '/' + section.alias + '/' + child.alias));
    });
  }
});

console.table(routes);



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