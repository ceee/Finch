import { find as _find } from 'underscore';

const alias = zero.alias.sections.pages;
const section = _find(zero.sections, section => section.alias === alias);
let routes = [];

if (section)
{
  routes.push({
    path: 'edit/:id',
    props: true,
    name: 'page',
    component: () => import('@zero/pages/' + alias + '/page.vue')
  });

  routes.push({
    path: 'create/:type/:parent?',
    props: true,
    name: 'page-create',
    component: () => import('@zero/pages/' + alias + '/page.vue')
  });

  routes.push({
    path: 'recyclebin',
    name: 'recyclebin',
    component: () => import('@zero/pages/' + alias + '/recyclebin/recyclebin.vue'),
    meta: {
      name: '@recyclebin.name'
    }
  });

  routes.push({
    path: 'history',
    name: 'history',
    component: () => import('@zero/pages/' + alias + '/history.vue'),
    meta: {
      name: '@page.history.name'
    }
  });
}

export default {
  section: alias,
  routes: routes
};