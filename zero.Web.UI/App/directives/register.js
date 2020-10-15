
export default function (app)
{
  const requireComponent = require.context('.', true, /[\w-]+\.js/);

  requireComponent.keys().forEach(path =>
  {
    let pathParts = path.split('/');
    let fileName = pathParts[pathParts.length - 1].split('.')[0];

    if (fileName === 'register')
    {
      return;
    }

    const componentConfig = requireComponent(path);
    const config = componentConfig.default || componentConfig;
    const name = config.name || fileName;

    app.directive(name, config);
  });
};