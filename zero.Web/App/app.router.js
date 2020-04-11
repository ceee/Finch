import Vue from 'vue';
import VueRouter from 'vue-router';
import Localization from 'zeroservices/localization';
import ViewDefault from 'zeropages/page';

Vue.use(VueRouter);

const routes = [];



// add defined backoffice sections (with their children) to the router

let addSection = (section, component) =>
{
  routes.push({
    path: section.url,
    component: component,
    meta: {
      section: section
    }
  });
};

zero.sections.forEach(section =>
{
  addSection(section, () => import('zeropages/' + section.alias));

  if (section.children.length > 0)
  {
    section.children.forEach(child =>
    {
      addSection(child, () => import('zeropages/' + section.alias + '/' + child.alias));
    });
  }
});



// add fallback route (this should probably by 404 page)

routes.push({ path: '*', component: ViewDefault });



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