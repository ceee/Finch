import { collection, get, post } from '../helpers/request.ts';

const base = 'media/';

const upload = async (file, folderId, onProgress, isTemporary) =>
{
  var data = new FormData();
  data.append('file', file);
  data.append('folderId', folderId);

  return await post(base + (isTemporary ? 'uploadTemporary' : 'upload'), data, {
    onUploadProgress: (progressEvent) =>
    {
      if (typeof onProgress === 'function')
      {
        var percentCompleted = Math.round((progressEvent.loaded * 100) / progressEvent.total);
        onProgress(percentCompleted);
      }
    }
  });
};


const getImageSource = (id, size, shared) =>
{
  if (!id || id.indexOf('http') === 0)
  {
    return id;
  }
  if (id.indexOf('url://') === 0)
  {
    return id.substring(6) + "?preset=productListing";
  }
  if (Array.isArray(id))
  {
    id = id[0];
  }

  if (size === true)
  {
    size = 'thumbnail';
  }
  else if (size === false)
  {
    size = 'original';
  }
  else if (!size)
  {
    size = 'preview';
  }

  return zero.apiPath + base + 'getSource/?id=' + id + '&size=' + size + (shared === true ? '&scope=shared' : '');
};


export default {
  ...collection(base),

  getListByQuery: async query => await get(base + 'getListByQuery', { params: query }),

  move: async (id, destinationId) => await post(base + 'move', { id, destinationId }),

  upload,

  getImageSource
};