import axios from 'axios';

const getConfig = config =>
{
  config = config || {};
  if (config.scope)
  {
    config.params = config.params || {};
    config.params.scope = config.scope;
  }
  return config;
};

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
  config = getConfig(config);

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
    getById: async (id, config) => await get(base + 'getById', { ...config, params: { id } }),
    getByIds: async (ids, config) => await get(base + 'getByIds', { ...config, params: { ids } }),
    getEmpty: async config => await get(base + 'getEmpty', { ...config }),
    getByQuery: async (query, config) => await get(base + 'getByQuery', { ...config, params: { query } }),
    getAll: async (config) => await get(base + 'getAll', { ...config }),
    getPreviews: async (ids, config) => await get(base + 'getPreviews', { ...config, params: { ids } }),
    getForPicker: async (config) => await get(base + 'getForPicker', { ...config }),
    save: async (model, config) => await post(base + 'save', model, { ...config }),
    delete: async (id, config) => await del(base + 'delete', { ...config, params: { id } })
  };
}