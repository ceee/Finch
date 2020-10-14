
let imports = [
  () => import('./clickoutside.js'),
  () => import('./currency.js'),
  () => import('./date.js'),
  () => import('./filesize.js'),
  () => import('./localize.js'),
  () => import('./resizable.js'),
  () => import('./sortable.js')
];

export default function (app)
{
  imports.forEach(path =>
  {
    path().then(resolved =>
    {
      const config = resolved.default || resolved;
      app.directive(config.name, config);
    });
  });
};