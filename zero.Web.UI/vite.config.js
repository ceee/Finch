const path = require('path');
const fs = require('fs');
const { createVuePlugin } = require('vite-plugin-vue2');

let loadedPlugins = JSON.parse(process.env.ZERO_PLUGINS || "[]"); //["../zero.Commerce/Plugin", "../../Laola/Laola.Backoffice/Plugin"];
let zeroPlugins = [];

let pluginFileContent = '';
let pluginAliases = {};
let pluginNames = [];
let idx = 0;

loadedPlugins.forEach(pluginPath =>
{
  const viteConfigPath = pluginPath + "/vite.plugin.js"; 
  const pluginConfig = require(viteConfigPath);
  zeroPlugins.push(pluginConfig());

  const name = 'zeroPlugin' + idx;
  const alias = '/@zeroplugin' + (idx++) + '/';
  pluginAliases[alias] = path.resolve(__dirname, pluginPath);
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

const config = {
  port: process.env.PORT,
  cors: true,
  emitManifest: true,
  plugins: [createVuePlugin(), ...zeroPlugins],
  alias: {
    '/@zero/': path.resolve(__dirname, 'app/'),
    ...pluginAliases,
    'vue': 'vue/dist/vue.esm.js'
  },
  resolvers: [aliasResolver],
  rollupOutputOptions: {
    format: 'cjs',
    entryFileNames: `[name].js`,
    chunkFileNames: `[name].js`,
    assetFileNames: `[name].[ext]`
  },
};

export default config;