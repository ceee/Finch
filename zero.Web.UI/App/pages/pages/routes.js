
const alias = __zero.alias.sections.pages;
const section = __zero.sections.find(x => x.alias === alias);

export default section ? [
  {
    name: section.alias,
    path: section.url,
    component: () => import('./pages.vue'),
    meta: {
      name: section.name,
      alias: section.alias,
      section: section
    },
    children: [
      {
        name: 'page',
        path: 'edit/:id',
        section: alias,
        props: true,
        component: () => import('./page.vue')
      },
      {
        name: 'page-create',
        path: 'create/:type/:parent?',
        section: alias,
        props: true,
        component: () => import('./page.vue')
      },
      {
        name: 'recyclebin',
        path: 'recyclebin',
        section: alias,
        component: () => import('./recyclebin/recyclebin.vue'),
        meta: {
          name: '@recyclebin.name'
        }
      }
    ]
  } 
] : [];