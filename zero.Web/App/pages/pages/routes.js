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
        name: '@recyclebin.name'
      }
    }
  ]
};