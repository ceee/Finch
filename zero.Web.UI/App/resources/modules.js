import Axios from 'axios';

const base = 'modules/';

export default {

  getModuleTypes()
  {
    return Axios.get(base + 'getModuleTypes').then(res => Promise.resolve(res.data));
  },

  getModuleType(alias)
  {
    return Axios.get(base + 'getModuleType', { params: { alias } }).then(res => Promise.resolve(res.data));
  }
};