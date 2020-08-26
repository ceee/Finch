import { find as _find } from 'underscore';

const alias = zero.alias.sections.media;
const section = _find(zero.sections, section => section.alias === alias);
let routes = [];

if (section)
{
  routes.push({
    path: section.url + '/folder/:id',
    props: true,
    name: 'mediafolder',
    component: () => import('zero/pages/' + alias + '/media')
  });

  routes.push({
    section: alias,
    path: 'edit/:id',
    props: true,
    name: 'mediaitem',
    component: () => import('zero/pages/' + alias + '/detail')
  });

  routes.push({
    section: alias,
    path: 'recyclebin',
    name: 'mediarecyclebin',
    component: () => import('zero/pages/' + alias + '/recyclebin'),
    meta: {
      name: '@recyclebin.name'
    }
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
  routes: routes
};