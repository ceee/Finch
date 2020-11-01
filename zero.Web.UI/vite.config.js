const path = require('path');
const { createVuePlugin } = require('vite-plugin-vue2');

let zeroPlugins = [];
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
    if (id.indexOf('zero/') === 0 || id.indexOf('shop/') === 0)
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
    '/@zero/': path.resolve(__dirname, 'app/')
  },
  resolvers: [aliasResolver]
}