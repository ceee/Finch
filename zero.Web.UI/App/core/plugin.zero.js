
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
  console.info(vue, zero);
};

export default plugin;