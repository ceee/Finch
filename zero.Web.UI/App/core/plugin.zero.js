
import Plugin from './plugin.ts';
import editors from '../renderers/editors/all.js';
import lists from '../renderers/lists/all.js';
import routes from './routes.js';

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
    component: () => import('../components/pickers/linkPicker/areas/pages.vue')
  });

  zero.config.linkPicker.areas.push({
    alias: 'zero.media',
    name: '@zero.config.linkareas.media',
    component: () => import('../components/pickers/linkPicker/areas/media.vue')
  });
};

export default plugin;