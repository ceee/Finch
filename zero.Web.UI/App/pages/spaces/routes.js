
const alias = __zero.alias.sections.spaces;
const section = __zero.sections.find(x => x.alias === alias);

export default section ? [
  {
    name: section.alias,
    path: section.url,
    component: () => import('./spaces.vue'),
    meta: {
      name: section.name,
      alias: section.alias,
      section: section
    },
    children: [
      {
        name: 'space-item',
        path: ':alias/edit/:id',
        props: true,
        component: () => import('./spaces.vue')
      },
      {
        name: 'space',
        path: ':alias',
        props: true,
        component: () => import('./spaces.vue')
      },
      {
        name: 'space-create',
        path: ':alias/create/:scope?',
        props: true,
        component: () => import('./spaces.vue'),
        meta: {
          create: true
        }
      }
    ]
  }
] : [];