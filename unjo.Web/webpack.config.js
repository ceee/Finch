const path = require('path');
const VueLoaderPlugin = require('vue-loader/lib/plugin');

module.exports = {

  // entry point of the app
  context: __dirname,
  entry: { app: './app.js' },

  // dev environment (gets overwritten by prod.js)
  mode: 'development',
  devtool: 'cheap-module-eval-source-map',

  // output paths
  output: {
    path: path.resolve(__dirname, 'wwwroot/Assets'),
    filename: 'app.js',
    publicPath: '/Assets/'
  },

  // map files to vue
  resolve: {
    extensions: ['.js', '.vue', '.json'],
    alias: {
      'vue$': 'vue/dist/vue.esm.js',
      'unjo': path.join(__dirname, 'App'),
      '@': __dirname
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
        use: ["style-loader", "css-loader", "sass-loader"]
      }
    ]
  }
};