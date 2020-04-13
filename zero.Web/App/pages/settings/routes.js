import { map as _map } from 'underscore';

const alias = zero.alias.sections.settings;
const areas = zero.settingsAreas;

export default {
  routes: _map(areas, area => 
  {
    return {
      path: area.url,
      name: alias + '-' + area.alias,
      component: () => import(`zero/pages/${alias}/${area.alias}`)
    };
  })
};