import { map as _map, find as _find } from 'underscore';

const alias = zero.alias.sections.settings;
const section = _find(zero.sections, section => section.alias === alias);
let routes = [];

if (section)
{
  zero.settingsAreas.forEach(group => group.items.forEach(area => 
  {
    routes.push({
      path: area.url,
      name: alias + '-' + area.alias,
      component: () => import(`zero/pages/${alias}/${area.alias}`),
      meta: {
        name: [area.name, section.name]
      }
    });
  }));
}

export default { routes };