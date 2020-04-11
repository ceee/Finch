import Vue from 'vue';
import VueRouter from 'vue-router';

import ViewSettings from 'zeropages/settings';
import ViewSettingsUser from 'zeropages/settings/user';
import ViewDefault from 'zeropages/page';
import ViewLists from 'zeropages/lists';
import MyPluginRoutes from './Plugins/MyPlugin/Routes'

Vue.use(VueRouter);

history.scrollRestoration = 'manual';

const routes = [
  { path: '/settings/user', component: ViewSettingsUser },
  { path: '/settings*', component: ViewSettings },
  { path: '/lists', component: ViewLists },
  { path: '*', component: ViewDefault }
];

MyPluginRoutes.forEach(route =>
{
  routes.push(route);
});

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

//router.beforeEach((to, from, next) =>
//{
//  document.title = to.meta.title !== null ? to.meta.title + " | fifty" : "fifty";
//  EventHub.$emit('dialog-close');
//  next();
//});

export default router;