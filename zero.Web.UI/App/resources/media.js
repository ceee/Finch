import Axios from 'axios';
import Strings from 'zero/services/strings.js';

var api = base =>
{
  return {
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

    // get all media items (including folders)
    getListByQuery(query)
    {
      return Axios.get(base + 'getListByQuery', { params: query }).then(res => Promise.resolve(res.data));
    },

    // get path to thumbnail source
    getImageSource(id, thumb, enforceAppId)
    {
      if (!id || id.indexOf('http') === 0)
      {
        return id;
      }
      if (id.indexOf('url://') === 0)
      {
        return id.substring(6) + "?preset=productListing";
      }
      return zero.apiPath + base + 'streamThumbnail/?id=' + id + (typeof thumb === 'boolean' ? '&thumb=' + (thumb ? 'true' : 'false') : '') + (enforceAppId === false ? '&enforceAppId=false' : '');
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

    move(id, destinationId)
    {
      return Axios.post(base + 'move', { id, destinationId }).then(res => Promise.resolve(res.data));
    },

    // deletes a media
    delete (id)
    {
      return Axios.delete(base + 'delete', { params: { id } }).then(res => Promise.resolve(res.data));
    }
  };
};

const localApi = api('media/');
const sharedApi = api('media/');

export default {
  scope(isShared)
  {
    return isShared ? sharedApi : localApi;
  },
  ...localApi
};