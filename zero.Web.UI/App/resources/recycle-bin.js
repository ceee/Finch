import Axios from 'axios';

const base = 'recycleBin/';

export default {

  getByQuery(query)
  {
    return Axios.get(base + 'getByQuery', { params: query }).then(res => Promise.resolve(res.data));
  },

  getCountByOperation(operationId)
  {
    return Axios.get(base + 'getCountByOperation', { params: { operationId } }).then(res => Promise.resolve(res.data));
  },

  delete(id)
  {
    return Axios.delete(base + 'delete', { params: { id } }).then(res => Promise.resolve(res.data));
  },

  deleteByGroup(group)
  {
    return Axios.delete(base + 'deleteByGroup', { params: { group } }).then(res => Promise.resolve(res.data));
  }
};