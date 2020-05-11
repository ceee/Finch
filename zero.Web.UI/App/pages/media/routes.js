import { find as _find } from 'underscore';

const alias = zero.alias.sections.media;
const section = _find(zero.sections, section => section.alias === alias);
let routes = [];

if (section)
{
  routes.push({
    path: 'edit/:id',
    props: true,
    name: 'mediaitem',
    component: () => import('zero/pages/' + alias + '/detail')
  });

  //routes.push({
  //  path: 'recyclebin',
  //  name: 'recyclebin',
  //  component: () => import('zero/pages/' + alias + '/recyclebin'),
  //  meta: {
  //    name: '@page.recyclebin.name'
  //  }
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