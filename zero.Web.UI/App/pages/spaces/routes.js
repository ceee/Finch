import { find as _find } from 'underscore';

const alias = zero.alias.sections.spaces;
const section = _find(zero.sections, section => section.alias === alias);
let routes = [];

if (section)
{
  routes.push({
    path: ':alias/edit/:id',
    props: true,
    name: 'space-item',
    component: () => import('zero/pages/' + alias + '/spaces')
  });

  routes.push({
    path: ':alias',
    props: true,
    name: 'space',
    component: () => import('zero/pages/' + alias + '/spaces')
  });

  routes.push({
    path: ':alias/create',
    props: true,
    name: 'space-create',
    component: () => import('zero/pages/' + alias + '/spaces'),
    meta: {
      create: true
    }
  });

  //routes.push({
  //  path: 'list/:id',
  //  props: true,
  //  name: 'space_list',
  //  component: () => import('zero/pages/' + alias + '/views/list')
  //});

  //routes.push({
  //  path: ':alias',
  //  props: true,
  //  name: 'space',
  //  component: () => import('zero/pages/' + alias + '/views/editor')
  //});

  //routes.push({
  //  path: 'history',
  //  name: 'history',
  //  component: () => import('zero/pages/' + alias + '/history'),
  //  meta: {
  //    name: '@page.history.name'
  //  }
  //});
}

export default {
  section: alias,
  routes: routes
};