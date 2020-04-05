import Vue from 'vue';
import VueRouter from 'vue-router';

Vue.use(VueRouter);

history.scrollRestoration = 'manual';

const routes = [

];

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