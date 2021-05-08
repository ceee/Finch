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
  const resolvedPluginConfig = pluginConfig();
  zeroPlugins.push(resolvedPluginConfig);

  const name = 'zeroPlugin' + idx;
  const alias = '@zeroplugin' + (idx++);
  pluginAliases[alias] = path.resolve(__dirname, pluginPath);

  if (typeof resolvedPluginConfig.alias === 'object')
  {
    for (const key in resolvedPluginConfig.alias)
    {
      pluginAliases[key] = resolvedPluginConfig.alias[key];
    }
  }

  pluginNames.push(name);
  pluginFileContent += "import " + name + " from '" + alias + "/plugin.js';\n";
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

const config = {
  server: {
    port: process.env.PORT,
    cors: true
  },
  plugins: [createVuePlugin(), ...zeroPlugins],
  alias: {
    '@zero': path.resolve(__dirname, 'app/'),
    'zero': path.resolve(__dirname, 'app/'),
    ...pluginAliases,
    'vue': 'vue/dist/vue.esm.js',
    'tiptap': 'tiptap/dist/tiptap.esm.js'
  },
  build: {
    manifest: false,
    rollupOptions: {
      format: 'cjs',
      entryFileNames: `[name].js`,
      chunkFileNames: `[name].js`,
      assetFileNames: `[name].[ext]`
    },

  }
};

export default config;