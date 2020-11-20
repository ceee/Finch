import { collection, get, post } from '../helpers/request.ts';


const upload = async (file, folderId, onProgress, isTemporary) =>
{
  var data = new FormData();
  data.append('file', file);
  data.append('folderId', folderId);

  return await post('media/' + (isTemporary ? 'uploadTemporary' : 'upload'), data, {
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


const getImageSource = (id, thumb, core) =>
{
  if (!id || id.indexOf('http') === 0)
  {
    return id;
  }
  if (id.indexOf('url://') === 0)
  {
    return id.substring(6) + "?preset=productListing";
  }
  return zero.apiPath + 'media/streamThumbnail/?id=' + id + (typeof thumb === 'boolean' ? '&thumb=' + (thumb ? 'true' : 'false') : '') + (core === true ? '&core=true' : '');
};


export default {
  ...collection('media/'),

  getListByQuery: async query => await get('media/getListByQuery', { params: query }),

  move: async (id, destinationId) => await post('media/move', { id, destinationId }),

  upload,

  getImageSource
};