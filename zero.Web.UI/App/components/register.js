// Globally register all base components for convenience, because they
// will be used very frequently. Components are registered using the
// PascalCased version of their file name.

let components = [];
//const directories = ['buttons', 'forms', 'messages', 'tables', 'tabs'];

// TODO this file will import all vue component files with subdirectories
// if it contains a file "overlay.vue" it will create an "ui-overlay" component
// if another folder contains a file "overlay.vue" it is not used anymore or overridden.
// we don't want to add all files but only files where we define global component access in its settings

// https://webpack.js.org/guides/dependency-management/#require-context
const requireComponent = require.context(
  // Look for files in the current directory
  '.',
  // Do look in subdirectories
  true,
  // .vue files
  /[\w-]+\.vue$/
);

// For each matching file name...
requireComponent.keys().forEach((path) =>
{
  let pathParts = path.split('/');
  let fileName = pathParts[pathParts.length - 1];

  // Get the component config
  const componentConfig = requireComponent(path);
  // Get the PascalCase version of the component name
  const componentName = 'ui' + fileName
    // Remove the "./_" from the beginning
    .replace(/^\.\/_/, '')
    // Remove the file extension from the end
    .replace(/\.\w+$/, '')
    // Split up kebabs
    .split('-')
    // Upper case
    .map((kebab) => kebab.charAt(0).toUpperCase() + kebab.slice(1))
    // Concatenated
    .join('');

  if (componentName === 'uiOverlay')
  {
    return;
  }

  components.push({
    name: componentName,
    config: componentConfig.default || componentConfig
  })
});

export default function (app)
{
  components.forEach(component =>
  {
    app.component(component.name, component.config);
  });
};