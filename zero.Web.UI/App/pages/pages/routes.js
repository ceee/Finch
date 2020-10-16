
import Page from './page.vue';
import Pages from './pages.vue';
import RecycleBin from './recyclebin/recyclebin.vue';

const alias = __zero.alias.sections.pages;
const section = __zero.sections.find(x => x.alias === alias);

export default section ? [
  {
    name: section.alias,
    path: section.url,
    component: Pages,
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
        component: Page
      },
      {
        name: 'page-create',
        path: 'create/:type/:parent?',
        section: alias,
        props: true,
        component: Page
      },
      {
        name: 'recyclebin',
        path: 'recyclebin',
        section: alias,
        component: RecycleBin,
        meta: {
          name: '@recyclebin.name'
        }
      }
    ]
  } 
] : [];