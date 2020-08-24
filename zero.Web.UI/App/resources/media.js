import Axios from 'axios';

const base = 'media/';

export default {

  // get media by id
  getById(id)
  {
    return Axios.get(base + 'getById', { params: { id } }).then(res => Promise.resolve(res.data));
  },

  // get media by ids
  getByIds(ids)
  {
    return Axios.get(base + 'getByIds', { params: { ids } }).then(res => Promise.resolve(res.data));
  },

  // get new media model
  getEmpty()
  {
    return Axios.get(base + 'getEmpty').then(res => Promise.resolve(res.data));
  },

  // get all media items
  getAll(query)
  {
    return Axios.get(base + 'getAll', { params: query }).then(res => Promise.resolve(res.data));
  },

  // get path to thumbnail source
  getImageSource(id)
  {
    if (!id || id.indexOf('http') === 0)
    {
      return id;
    }
    return zero.apiPath + base + 'streamThumbnail/' + id;
  },

  // save a media
  save(model)
  {
    return Axios.post(base + 'save', model).then(res => Promise.resolve(res.data));
  },

  // uploads a file
  upload(file, folderId, onProgress, isTemporary)
  {
    var data = new FormData();
    data.append('file', file);
    data.append('folderId', folderId);

    return Axios.post(base + (isTemporary ? 'uploadTemporary' : 'upload'), data, {
      onUploadProgress: (progressEvent) =>
      {
        if (typeof onProgress === 'function')
        {
          var percentCompleted = Math.round((progressEvent.loaded * 100) / progressEvent.total);
          onProgress(percentCompleted);
        }
      }
    }).then(function (res)
    {
      return Promise.resolve(res.data);
    });

    //return Axios.post(base + 'save', model).then(res => Promise.resolve(res.data));
  },

  // deletes a media
  delete(id)
  {
    return Axios.delete(base + 'delete', { params: { id } }).then(res => Promise.resolve(res.data));
  }
};