
import Dashboard from './dashboard.vue';

const alias = __zero.alias.sections.dashboard;
const section = __zero.sections.find(x => x.alias === alias);

export default section ? [
  {
    name: section.alias,
    path: section.url,
    component: Dashboard,
    meta: {
      name: section.name,
      alias: section.alias,
      section: section
    }
  }
] : [];