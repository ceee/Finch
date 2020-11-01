const path = require('path');
const fs = require('fs');
const { createVuePlugin } = require('vite-plugin-vue2');

let zeroPlugins = [];
let _plugins = [
  {
    path: '../zero.Commerce/Plugin/vite.plugin.js',
    root: '../zero.Commerce/Plugin'
  },
  {
    path: '../../Laola/Laola.Backoffice/Plugin/vite.plugin.js',
    root: '../../Laola/Laola.Backoffice/Plugin'
  }
];

let pluginFileContent = '';
let pluginAliases = {};
let pluginNames = [];
let idx = 0;

_plugins.forEach(plugin =>
{
  const pluginConfig = require(plugin.path);
  zeroPlugins.push(pluginConfig());

  const name = 'zeroPlugin' + idx;
  const alias = '/@zeroplugin' + (idx++) + '/';
  pluginAliases[alias] = path.resolve(__dirname, plugin.root);
  pluginNames.push(name);
  pluginFileContent += "import " + name + " from '" + alias + "plugin.js';\n";
});

pluginFileContent += "export default [ " + pluginNames.join(', ') + " ];";

fs.writeFile(path.resolve(__dirname, 'app/core/plugins.js'), pluginFileContent, err =>
{
  if (err)
  {
    console.error(err)
    return
  }
  //file written successfully
})

//if (process.env.ZERO_PLUGINS)
//{
//  let plugins = JSON.parse(process.env.ZERO_PLUGINS);

//  plugins.forEach(plugin =>
//  {
//    const pluginConfig = require(plugin.path);

//    if (pluginConfig != null)
//    {
//      zeroPlugins.push(pluginConfig());
//    }
//  });
//}

const aliasResolver = {
  alias(id)
  {
    if (id.indexOf('zero/') === 0)
    {
      return '/@' + id;
    }
  }
  //requestToFile(publicPath, root)
  //{
  //  if (publicPath.indexOf('/@zero/') === 0)
  //  {
  //    return path.join(root, publicPath.replace('/@zero/', ''));
  //  }
  //  if (publicPath.indexOf('/@shop/') === 0)
  //  {
  //    return path.join(root, publicPath.replace('/@shop/', '../zero.Commerce/Plugins/zero.Commerce/'));
  //  }
  //}
};

export default {
  port: process.env.PORT,
  cors: true,
  emitManifest: true,
  plugins: [createVuePlugin(), ...zeroPlugins],
  alias: {
    '/@zero/': path.resolve(__dirname, 'app/'),
    ...pluginAliases
  },
  resolvers: [aliasResolver]
}