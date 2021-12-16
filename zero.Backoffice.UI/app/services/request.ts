
import axios from 'axios';
import { paths } from '../options';
import { ApiRequestConfig } from './request.types';

export * from './request.types';

export function get(url: string, config?: ApiRequestConfig)
{
  return send({ method: 'get', url, ...config });
}

export function post(url: string, data: any, config?: ApiRequestConfig)
{
  return send({ method: 'post', url, data, ...config });
}

export function del(url: string, data: any, config?: ApiRequestConfig)
{
  return send({ method: 'delete', url, data, ...config });
}

export function put(url: string, data: any, config?: ApiRequestConfig)
{
  return send({ method: 'put', url, data, ...config });
}

export function patch(url: string, data: any, config?: ApiRequestConfig)
{
  return send({ method: 'patch', url, data, ...config });
}


export async function send(config: ApiRequestConfig)
{
  if (!config.raw)
  {
    config.baseURL = paths.api;
  }
  if (config.system)
  {
    config.params['zero.system'] = true;
  }

  const result = await axios(config);
  return result.data;

  //try
  //{
  //  const result = await axios(config);
  //  return result.data;
  //}
  //catch (err)
  //{
  //  console.error('axios err: ' + err);
  //  // TODO handle errors
  //}
}


//export function collection(base)
//{
//  return {
//    getById: async (id, changeVector, config) => await get(base + 'getById', { ...config, params: { id, changeVector } }),
//    getByIds: async (ids, config) => await get(base + 'getByIds', { ...config, params: { ids } }),
//    getEmpty: async config => await get(base + 'getEmpty', { ...config }),
//    getByQuery: async (query, config) => await get(base + 'getByQuery', { ...config, params: { query } }),
//    getAll: async (config) => await get(base + 'getAll', { ...config }),
//    getPreviews: async (ids, config) => await get(base + 'getPreviews', { ...config, params: { ids } }),
//    getForPicker: async (config) => await get(base + 'getForPicker', { ...config }),
//    getRevisions: async (id, query, config) => await get(base + 'getRevisions', { ...config, params: { id, query } }),
//    save: async (model, config) => await post(base + 'save', model, { ...config }),
//    delete: async (id, config) => await del(base + 'delete', { ...config, params: { id } })
//  };
//}


//export function download(response)
//{
//  let filename = response.headers["zero-filename"] || 'download.bin';

//  // code from: https://github.com/kennethjiang/js-file-download/blob/master/file-download.js

//  var blob = response.data;
//  if (typeof window.navigator.msSaveBlob !== 'undefined')
//  {
//    // IE workaround for "HTML7007: One or more blob URLs were
//    // revoked by closing the blob for which they were created.
//    // These URLs will no longer resolve as the data backing
//    // the URL has been freed."
//    window.navigator.msSaveBlob(blob, filename);
//  }
//  else
//  {
//    var blobURL = (window.URL && window.URL.createObjectURL) ? window.URL.createObjectURL(blob) : window.webkitURL.createObjectURL(blob);
//    var tempLink = document.createElement('a');
//    tempLink.style.display = 'none';
//    tempLink.href = blobURL;
//    tempLink.setAttribute('download', filename);

//    // Safari thinks _blank anchor are pop ups. We only want to set _blank
//    // target if the browser does not support the HTML5 download attribute.
//    // This allows you to download files in desktop safari if pop up blocking
//    // is enabled.
//    if (typeof tempLink.download === 'undefined')
//    {
//      tempLink.setAttribute('target', '_blank');
//    }

//    document.body.appendChild(tempLink);
//    tempLink.click();

//    // Fixes "webkit blob resource error 1"
//    setTimeout(function ()
//    {
//      document.body.removeChild(tempLink);
//      window.URL.revokeObjectURL(blobURL);
//    }, 200)
//  }
//};