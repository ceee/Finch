
import Plugin from './plugin.ts';
import editors from '../renderers/editors/all.js';
import lists from '../renderers/lists/all.js';
import routes from './routes.js';
import LinkAreaPages from '../components/pickers/linkPicker/areas/pages.vue';
import LinkAreaMedia from '../components/pickers/linkPicker/areas/media.vue';

const plugin = new Plugin('zero');

// add renderers
plugin.addEditors(editors);

// add renderers
plugin.addLists(lists);

// add routes
plugin.addRoutes(routes);

plugin.install = (vue, zero) =>
{
  zero.config.linkPicker.areas.push({
    alias: 'zero.pages',
    name: '@zero.config.linkareas.pages',
    component: LinkAreaPages
  });

  zero.config.linkPicker.areas.push({
    alias: 'zero.media',
    name: '@zero.config.linkareas.media',
    component: LinkAreaMedia
  });
};

export default plugin;