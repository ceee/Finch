const alias = zero.alias.sections.pages;

export default {
  section: alias,
  routes: [
    {
      path: 'edit/:id',
      props: true,
      name: 'page',
      component: () => import('zero/pages/' + alias + '/page')
    },
    {
      path: 'recyclebin',
      name: 'recyclebin',
      component: () => import('zero/pages/' + alias + '/recyclebin'),
      meta: {
        name: '@page.recyclebin.name'
      }
    },
    {
      path: 'history',
      name: 'history',
      component: () => import('zero/pages/' + alias + '/history'),
      meta: {
        name: '@page.history.name'
      }
    }
  ]
};