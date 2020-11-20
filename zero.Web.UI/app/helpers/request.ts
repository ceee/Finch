import axios from 'axios';

export async function get(url, config: any = null)
{
  return await send({ method: 'get', url, ...config });
}

export async function post(url, data: any = null, config: any = null)
{
  return await send({ method: 'post', url, data, ...config });
}

export async function del(url, config: any = null)
{
  return await send({ method: 'delete', url, ...config });
}

export async function put(url, data: any = null, config: any = null)
{
  return await send({ method: 'put', url, data, ...config });
}

export async function patch(url, data: any = null, config: any = null)
{
  return await send({ method: 'patch', url, data, ...config });
}


export async function send(config)
{
  try
  {
    const result = await axios(config);
    return result.data;
  }
  catch (err)
  {
    // TODO handle errors
  }
}


export function collection(base)
{
  return {
    getById: async id => await get(base + 'getById', { params: { id } }),
    getByIds: async ids => await get(base + 'getByIds', { params: { ids } }),
    getEmpty: async () => await get(base + 'getEmpty'),
    getByQuery: async query => await get(base + 'getByQuery', { params: { query } }),
    getAll: async () => await get(base + 'getAll'),
    getPreviews: async ids => await get(base + 'getPreviews', { params: { ids } }),
    getForPicker: async () => await get(base + 'getForPicker'),
    save: async model => await post(base + 'save', model),
    delete: async id => await del(base + 'delete', { params: { id } })
  };
}