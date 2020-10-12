const path = require('path');
const VueLoaderPlugin = require('vue-loader/lib/plugin');

module.exports = {

  // entry point of the app
  context: __dirname,
  entry: {
    app: './app.js',
    setup: './Setup/setup.js'
  },

  // dev environment (gets overwritten by prod.js)
  mode: 'development',
  devtool: 'cheap-module-eval-source-map',

  // output paths
  output: {
    path: path.resolve(__dirname, 'Assets'),
    filename: '[name].js',
    publicPath: '/Assets/'
  },

  // map files to vue
  resolve: {
    extensions: ['.js', '.vue', '.json'],
    alias: {
      'vue$': path.join(__dirname, 'node_modules', 'vue/dist/vue.esm.js'),
      'deps': path.join(__dirname, 'node_modules'),
      'zero': path.join(__dirname, 'App'),
      'zerosetup': path.join(__dirname, 'Setup'),
      '@': __dirname,
      'shop': path.join(__dirname, '..', 'zero.Commerce/Plugins/zero.Commerce') // TODO dynPATH
    }
  },

  // plugins to use
  plugins: [
    new VueLoaderPlugin()
  ],

  // modules to load
  module: {
    rules: [
      {
        test: /\.vue$/,
        exclude: /node_modules/,
        loader: 'vue-loader'
      },
      {
        test: /\.js$/,
        exclude: /node_modules/,
        loader: 'babel-loader'
      },
      {
        test: /\.scss$/,
        use: ["vue-style-loader", "css-loader", "sass-loader"]
      }
    ]
  }
};