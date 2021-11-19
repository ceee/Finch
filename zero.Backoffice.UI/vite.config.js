const path = require('path');
const fs = require('fs');
const { createVuePlugin } = require('vite-plugin-vue2');

let loadedPlugins = JSON.parse(process.env.ZERO_PLUGINS || "[]");


if (!process.env.ZERO_PLUGINS)
{
  loadedPlugins = ["../zero.Commerce/Plugin", "../zero.Stories/Plugin", "../zero.Forms/Plugin", "../../Laola/Laola.Backoffice/Plugin"];
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

fs.writeFile(path.resolve(__dirname, 'app/core/plugins.js'), pluginFileContent, err =>
{
  if (err)
  {
    console.error(err)
    return
  }
  //file written successfully
})

const myPlugin = () => ({
  name: 'configure-server',
  configureServer(server)
  {
    var watcher = server.watcher;
    watcher
      .on('add', path => console.log(`watch: ${path}`));
    var files = watcher.getWatched();
    console.log('watcher:');
    console.log(files);

    // https://github.com/paulmillr/chokidar#api

    //server.middlewares.use((req, res, next) =>
    //{
    //  console.log(req.url);
    //  next();
    //  // custom handle request...
    //})
  }
})

/**
 * @type {import('vite').UserConfig}
 */
let config = {
  server: {
    port: process.env.PORT || 3399,
    cors: true
  },
  plugins: [createVuePlugin(), ...zeroPlugins, myPlugin()],
  alias: {
    '@zero': path.resolve(__dirname, 'app/'),
    'zero': path.resolve(__dirname, 'app/'),
    ...pluginAliases,
    'vue': 'vue/dist/vue.esm.js',
    'tiptap': 'tiptap/dist/tiptap.esm.js',
    'zerox': path.resolve(__dirname, 'app/zerox.js')
  },
  build: {
    manifest: true,
    outDir: 'dist/zero',
    minify: false,
    terserOptions: {
      compress: false
    },
    rollupOptions: {
      output: {
        format: 'es',
        entryFileNames: `[name].js`,
        chunkFileNames: `[name].js`,
        assetFileNames: `[name].[ext]`
      }
    }
  }
};

console.log('root: ' + config.root);

if (process.env.NODE_ENV === 'production')
{
  config.base = '/zero/';
  config.alias.tiptap = 'node_modules/tiptap/dist/tiptap.esm.js';
  config.alias.underscore = 'node_modules/underscore/underscore-esm.js';
  config.alias.axios = 'node_modules/axios/dist/axios.js';
  config.alias.dayjs = 'node_modules/dayjs/esm/index.js';
}

export default config;