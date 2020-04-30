import Axios from 'axios';

export default {

  // get space by alias
  getByAlias(alias)
  {
    return Axios.get('spaces/getByAlias', { params: { alias }}).then(res => Promise.resolve(res.data));
  },

  // get all spaces
  getCollections()
  {
    return Axios.get('spaces/getAll').then(res => Promise.resolve(res.data));
  },

  // get all list items in a collection
  getAll(alias, query)
  {
    query.alias = alias;

    return Axios.get('spaces/getAll', { params: query }).then(res => Promise.resolve(res.data));
  }
};