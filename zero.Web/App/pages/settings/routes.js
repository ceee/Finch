import { map as _map, find as _find } from 'underscore';

const alias = zero.alias.sections.settings;
const section = _find(zero.sections, section => section.alias === alias);
const areas = zero.settingsAreas;

export default {
  routes: _map(areas, area => 
  {
    return {
      path: area.url,
      name: alias + '-' + area.alias,
      component: () => import(`zero/pages/${alias}/${area.alias}`),
      meta: {
        name: [area.name, section.name ]
      }
    };
  })
};