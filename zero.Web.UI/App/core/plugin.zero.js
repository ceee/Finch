
import Plugin from './plugin.js';
import editors from '../editors/all.js';
import routes from './routes.js';

const plugin = new Plugin('zero');

// add renderers
plugin.addEditors(editors);

// add routes
plugin.addRoutes(routes);

export default plugin;