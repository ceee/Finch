

export default {

  setup()
  {
    const renderers = require.context('./Renderers', true, /[\w-]+\.js/);

    renderers.keys().forEach(path =>
    {
      let pathParts = path.split('/');
      let fileName = pathParts[pathParts.length - 1];

      const componentConfig = renderers(path);
      const alias = componentConfig.default.alias;

      zero.renderers[alias] = componentConfig.default;
    });
  }
};


