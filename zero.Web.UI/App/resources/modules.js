import Axios from 'axios';

const base = 'modules/';

export default {

  getModuleTypes(tags)
  {
    return Axios.get(base + 'getModuleTypes', { params: { tags } }).then(res => Promise.resolve(res.data));
  },

  getModuleType(alias)
  {
    return Axios.get(base + 'getModuleType', { params: { alias } }).then(res => Promise.resolve(res.data));
  },

  getEmpty(alias)
  {
    return Axios.get(base + 'getEmpty', { params: { alias } }).then(res => Promise.resolve(res.data));
  },
};