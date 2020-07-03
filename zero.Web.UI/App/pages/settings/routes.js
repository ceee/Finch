import { map as _map, find as _find, isArray as _isArray } from 'underscore';

const alias = zero.alias.sections.settings;
const section = _find(zero.sections, section => section.alias === alias);
let routes = [];

const detailPages = [];
detailPages[zero.alias.settings.users] = [
  { view: 'user' },
  { view: 'role', path: 'role' },
  { view: 'role', path: 'role/create', isCreate: true }
];
detailPages[zero.alias.settings.countries] = [
  { view: 'country' },
  { view: 'country', path: 'create', isCreate: true }
];
detailPages[zero.alias.settings.translations] = [
  { view: 'translations' },
  { view: 'translations', path: 'create', isCreate: true }
];
detailPages[zero.alias.settings.languages] = [
  { view: 'language' },
  { view: 'language', path: 'create', isCreate: true }
];
detailPages[zero.alias.settings.applications] = [
  { view: 'application' },
  { view: 'application', path: 'create', isCreate: true }
];

if (section)
{
  zero.settingsAreas.forEach(group => group.items.forEach(area => 
  {
    // add settings area page
    if (!area.isPlugin)
    {
      routes.push({
        path: area.url,
        name: alias + '-' + area.alias,
        component: () => import(`zero/pages/${alias}/${area.alias}`),
        meta: {
          name: [area.name, section.name]
        }
      });
    }

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
        const isCreate = typeof detail.isCreate === 'boolean' ? detail.isCreate : false;

        routes.push({
          path: area.url + '/' + path + (!isCreate ? '/:id' : '/:scope?'),
          name: alias + '-' + area.alias + '-' + path.replace('/', '-'),
          component: () => import(`zero/pages/${alias}/${detail.view}`),
          props: true,
          meta: {
            create: isCreate,
            name: [area.name, section.name]
          }
        });
      });
    }
  }));
}

export default { routes };