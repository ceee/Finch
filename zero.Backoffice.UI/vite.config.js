const path = require('path');
const fs = require('fs');
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import pluginRewriteAll from 'vite-plugin-rewrite-all'

let loadedPlugins = JSON.parse(process.env.ZERO_PLUGINS || "[]");


if (!process.env.ZERO_PLUGINS)
{
  //loadedPlugins = ["../zero.Commerce/Plugin", "../zero.Stories/Plugin", "../zero.Forms/Plugin", "../../Laola/Laola.Backoffice/Plugin"];
  loadedPlugins = ["../plugins/zero.Commerce/Backoffice/Plugin", "../../Laola/Laola.Backoffice/Plugin"]
  //loadedPlugins = ["../plugins/zero.Commerce/Backoffice/Plugin"]
  //loadedPlugins = [];
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
  pluginFileContent += "import " + name + " from '" + alias + "/plugin';\n";
});

pluginFileContent += "export default [ " + pluginNames.join(', ') + " ];";

fs.writeFile(path.resolve(__dirname, 'app/plugins.generated.ts'), pluginFileContent, err =>
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
        target: 'http://localhost:2310',
        changeOrigin: true,
        secure: false,
        ws: false
      },
      '/uploads': {
        target: 'http://localhost:2310',
        changeOrigin: true,
        secure: false,
        ws: false
      }
    }
  },
  resolve: {
    alias: {
      vue: path.resolve(__dirname, 'node_modules/@vue/compat'),
      zero: path.resolve(__dirname, 'app'),
      zeroworkers: path.resolve(__dirname, 'app/modules/preview'),
      ...pluginAliases,
    }
  },
  plugins: [
    pluginRewriteAll(),
    vue({
      template: {
        compilerOptions: {
          isCustomElement: tag => tag == 'content',
          compatConfig: {
            MODE: 2
          }
        }
      }
    }),
    ...zeroPlugins
  ],
  build: {
    manifest: true,
    outDir: 'dist/zero',
    minify: 'esbuild',
    //terserOptions: {
    //  compress: false
    //},
    rollupOptions: {
      output: {
        format: 'es',
        entryFileNames: `[name].[hash].js`,
        chunkFileNames: `[name].[hash].js`,
        assetFileNames: `[name].[hash].[ext]`
      }
    }
  }
});

if (process.env.NODE_ENV === 'production')
{
  config.base = '/zero/';
  config.resolve.alias.dayjs = 'node_modules/dayjs/esm/index.js';
}

export default config;