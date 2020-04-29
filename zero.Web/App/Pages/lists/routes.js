import { find as _find } from 'underscore';

const alias = zero.alias.sections.lists;
const section = _find(zero.sections, section => section.alias === alias);
let routes = [];

if (section)
{
  routes.push({
    path: 'edit/:id',
    props: true,
    name: 'listitem',
    component: () => import('zero/pages/' + alias + '/item')
  });

  routes.push({
    path: ':alias',
    props: true,
    name: 'list',
    component: () => import('zero/pages/' + alias + '/list')
  });

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