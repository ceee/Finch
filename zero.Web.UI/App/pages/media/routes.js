
const alias = __zero.alias.sections.media;
const section = __zero.sections.find(x => x.alias === alias);

export default section ? [
  {
    name: section.alias,
    path: section.url + '/:id?',
    component: () => import('./overview.vue'),
    props: true,
    meta: {
      name: section.name,
      alias: section.alias,
      section: section
    }
  },
  {
    name: section.alias + '-edit',
    path: section.url + '/edit/:id',
    component: () => import('./detail.vue'),
    props: true,
    meta: {
      name: section.name,
      alias: section.alias,
      section: section
    }
  }
] : [];