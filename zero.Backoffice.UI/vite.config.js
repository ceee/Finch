const path = require('path');
const fs = require('fs');
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

let loadedPlugins = JSON.parse(process.env.ZERO_PLUGINS || "[]");


if (!process.env.ZERO_PLUGINS)
{
  //loadedPlugins = ["../zero.Commerce/Plugin", "../zero.Stories/Plugin", "../zero.Forms/Plugin", "../../Laola/Laola.Backoffice/Plugin"];
  loadedPlugins = [];
}

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

fs.writeFile(path.resolve(__dirname, 'app/plugins.generated.js'), pluginFileContent, err =>
{
  if (err)
  {
    console.error(err)
    return
  }
  //file written successfully
})

let config = defineConfig({
  server: {
    port: process.env.PORT || 3399,
    cors: true,
    proxy: {
      '/zero/api': {
        target: 'http://localhost:2320',
        changeOrigin: true,
        secure: false,
        ws: false
      }
    }
  },
  plugins: [vue(), ...zeroPlugins],
  build: {
    manifest: true,
    outDir: 'dist/zero',
    minify: false,
    terserOptions: {
      compress: false
    },
    //alias: {
    //  'tiptap': 'tiptap/dist/tiptap.esm.js',
    //},
    rollupOptions: {
      output: {
        format: 'es',
        entryFileNames: `[name].js`,
        chunkFileNames: `[name].js`,
        assetFileNames: `[name].[ext]`
      }
    }
  }
});

//console.log('root: ' + config.root);

if (process.env.NODE_ENV === 'production')
{
  config.base = '/zero/';
  //config.alias.tiptap = 'node_modules/tiptap/dist/tiptap.esm.js';
}

export default config;