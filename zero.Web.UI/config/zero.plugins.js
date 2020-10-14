
zero.plugins = zero.plugins || [];

// TODO correct path
let plugins = require.context('@/../zero.Commerce/Plugins/zero.Commerce', true, /plugin\.js$/); // TODO dynPATH

plugins.keys().forEach(path =>
{
  const routesDefinition = plugins(path);
  const definition = routesDefinition.default || routesDefinition;

  definition.name = 'commerce'; // TODO
  zero.plugins.push(definition);
});

//plugins = require.context('@/../zero.Debug', true, /plugin\.js$/); // TODO dynPATH

//plugins.keys().forEach(path =>
//{
//  const routesDefinition = plugins(path);
//  const definition = routesDefinition.default || routesDefinition;

//  definition.name = 'project'; // TODO
//  zero.plugins.push(definition);
//});


// run setup action directly

zero.plugins.forEach(plugin =>
{
  if (typeof plugin.setup === 'function')
  {
    plugin.setup();
  }
});