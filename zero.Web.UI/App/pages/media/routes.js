
import Media from './detail.vue';
import MediaOverview from './overview.vue';

const alias = __zero.alias.sections.media;
const section = __zero.sections.find(x => x.alias === alias);

export default section ? [
  {
    name: section.alias,
    path: section.url + '/:scope?/:id?',
    component: MediaOverview,
    props: true,
    meta: {
      name: section.name,
      alias: section.alias,
      section: section
    }
  },
  {
    name: section.alias + '-edit',
    path: section.url + '/:scope/edit/:id',
    component: Media,
    props: true,
    meta: {
      name: section.name,
      alias: section.alias,
      section: section
    }
  }
] : [];