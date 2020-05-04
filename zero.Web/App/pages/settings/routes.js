import { map as _map, find as _find, isArray as _isArray } from 'underscore';

const alias = zero.alias.sections.settings;
const section = _find(zero.sections, section => section.alias === alias);
let routes = [];

const detailPages = [];
detailPages[zero.alias.settings.users] = [
  { view: 'user' },
  { view: 'role', path: 'role' }
];
detailPages[zero.alias.settings.countries] = 'country';
detailPages[zero.alias.settings.translations] = 'translations';

if (section)
{
  zero.settingsAreas.forEach(group => group.items.forEach(area => 
  {
    // add settings area page
    routes.push({
      path: area.url,
      name: alias + '-' + area.alias,
      component: () => import(`zero/pages/${alias}/${area.alias}`),
      meta: {
        name: [area.name, section.name]
      }
    });

    // add details page
    if (detailPages[area.alias])
    {
      var config = detailPages[area.alias];
      var details = [];

      if (typeof config === 'string')
      {
        details.push({
          view: config,
          path: 'edit'
        });
      }
      else if (_isArray(config))
      {
        details = config;
      }
      else if (typeof config === 'object')
      {
        details.push(config);
      }

      details.forEach(detail =>
      {
        const path = detail.path || 'edit';

        routes.push({
          path: area.url + '/' + path + '/:id',
          name: alias + '-' + area.alias + '-' + path,
          component: () => import(`zero/pages/${alias}/${detail.view}`),
          props: true,
          meta: {
            name: [area.name, section.name]
          }
        });
      });
    }
  }));
}

export default { routes };