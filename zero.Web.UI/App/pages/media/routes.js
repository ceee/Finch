
import Media from './detail.vue';
import Medias from './media.vue';

const alias = __zero.alias.sections.media;
const section = __zero.sections.find(x => x.alias === alias);

export default section ? [
  {
    name: section.alias,
    path: section.url,
    component: Medias,
    meta: {
      name: section.name,
      alias: section.alias,
      section: section
    },
    children: [
      {
        name: 'mediaitem',
        path: 'edit/:id',
        props: true,
        component: Media
      }
    ]
  },
  {
    name: 'mediafolder',
    path: section.url + '/folder/:id',
    props: true,
    component: Medias,
    meta: {
      name: section.name
    }
  }
] : [];