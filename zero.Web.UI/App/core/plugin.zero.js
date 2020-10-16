
import Plugin from './plugin.js';
import renderers from '../renderers/all.js';
import routes from './routes.js';

const plugin = new Plugin('zero');

// add renderers
plugin.addRenderers(renderers);

// add routes
plugin.addRoutes(routes);

export default plugin;