
let imports = [
  () => import(`./application.js`),
  () => import('./country.js'),
  () => import('./language.js'),
  () => import('./media.js'),
  () => import('./translation.js'),
  () => import('./user.js'),
  () => import('./userRole.js')
];

export default function (renderers)
{
  imports.forEach(path =>
  {
    path().then(resolved =>
    {
      const config = resolved.default || resolved;
      const alias = config.alias;
      renderers[alias] = config;
    });
  });
};