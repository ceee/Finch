
import Spaces from './spaces.vue';

const alias = __zero.alias.sections.spaces;
const section = __zero.sections.find(x => x.alias === alias);

export default section ? [
  {
    name: section.alias,
    path: section.url,
    component: Spaces,
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
        component: Spaces
      },
      {
        name: 'space',
        path: ':alias',
        props: true,
        component: Spaces
      },
      {
        name: 'space-create',
        path: ':alias/create/:scope?',
        props: true,
        component: Spaces,
        meta: {
          create: true
        }
      }
    ]
  }
] : [];