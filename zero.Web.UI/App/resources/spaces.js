import Axios from 'axios';

export default {

  // get space by alias
  getByAlias(alias)
  {
    return Axios.get('spaces/getByAlias', { params: { alias }}).then(res => Promise.resolve(res.data));
  },

  // get all spaces
  getAll()
  {
    return Axios.get('spaces/getAll').then(res => Promise.resolve(res.data));
  },

  // get all list items in a space
  getList(alias, query)
  {
    query.alias = alias;

    return Axios.get('spaces/getList', { params: query }).then(res => Promise.resolve(res.data));
  },

  // get space item
  getContent(alias, contentId)
  {
    return Axios.get('spaces/getContent', { params: { alias, contentId } }).then(res => Promise.resolve(res.data));
  },

  // save a space item
  save(model)
  {
    return Axios.post('spaces/save', model).then(res => Promise.resolve(res.data));
  },

  // deletes an item
  delete(alias, id)
  {
    return Axios.delete('spaces/delete', { params: { alias, id } }).then(res => Promise.resolve(res.data));
  }
};