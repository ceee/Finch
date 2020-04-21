import { map as _map, find as _find } from 'underscore';

const alias = zero.alias.sections.settings;
const section = _find(zero.sections, section => section.alias === alias);
let routes = [];

const detailPages = [];
detailPages[zero.alias.settings.users] = 'user';
detailPages[zero.alias.settings.countries] = 'country';

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
      routes.push({
        path: area.url + '/edit/:id',
        name: alias + '-' + area.alias + '-edit',
        component: () => import(`zero/pages/${alias}/${detailPages[area.alias]}`),
        props: true,
        meta: {
          name: [area.name, section.name]
        }
      });
    }
  }));
}

export default { routes };