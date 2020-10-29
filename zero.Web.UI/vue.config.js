
//console.log(process.env);
// TODO
// maybe we can make an HTTP request here to get plugin paths and install them
// as well as set other fields in config as aliases.

module.exports = {

  // disable hashes in filenames
  filenameHashing: false,

  // delete HTML related webpack plugins
  chainWebpack: config =>
  {
    config.plugins.delete('html')
    config.plugins.delete('preload')
    config.plugins.delete('prefetch')
  },

  // output path
  outputDir: "Assets",

  publicPath: "/zero/vue-cli",

  // generated output
  pages: {
    app: "app.js"
  },

  devServer: {
    sockPath: '/zero/vue-cli/sockjs-node',
    public: 'http://localhost:2310/zero/vue-cli'
  },

  // webpack configuration
  chainWebpack: config =>
  {
    const path = require('path');

    config.resolve.alias.set('zero', path.resolve(__dirname, 'App'));
    config.resolve.alias.set('zerosetup', path.resolve(__dirname, 'Setup'));
    config.resolve.alias.set('@', __dirname);
    config.resolve.alias.set('shop', path.resolve(__dirname, '../zero.Commerce/Plugins/zero.Commerce'));
    config.resolve.alias.set('deps', path.join(__dirname, 'node_modules'));
  }
}
