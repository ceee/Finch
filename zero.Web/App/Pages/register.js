// Globally register all pages for convenience, because they
// will be used very frequently. Page components are registered using the
// PascalCased version of their file name.

import Vue from 'vue';


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
  const componentName = 'app' + fileName
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

  // Globally register the component
  Vue.component(componentName, componentConfig.default || componentConfig);
});