
import Plugin from '@zero/core/plugin.js';

let plugin = new Plugin('zero');

plugin.mounted = () =>
{
  console.info(this.name + ' mounted');
};

export default plugin;