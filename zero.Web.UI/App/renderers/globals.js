
zero.renderers = zero.renderers || {};

// https://webpack.js.org/guides/dependency-management/#require-context
const requireComponent = require.context(
  // Look for files in the current directory
  '.',
  // Do look in subdirectories
  true,
  // .js files
  /[\w-]+\.js/
);

// For each matching file name...
requireComponent.keys().forEach((path) =>
{
  let pathParts = path.split('/');
  let fileName = pathParts[pathParts.length - 1];

  if (fileName === 'globals.js')
  {
    return;
  }

  const componentConfig = requireComponent(path);
  const alias = componentConfig.default.alias;

  zero.renderers[alias] = componentConfig.default;
});