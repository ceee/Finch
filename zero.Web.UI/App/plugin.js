import Plugin from 'zero/core/plugin';


let plugin = new Plugin('zero');

plugin.mounted = () =>
{
  console.info(this.name + ' mounted');
};

export default plugin;