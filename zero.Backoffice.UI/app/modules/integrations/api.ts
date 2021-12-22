import { get, post, put, del, ApiRequestConfig, ApiRequestQuery } from '../../services/request';

export default {

  getTypes: (config?: ApiRequestConfig) => get('integrations/types', { ...config }),

  getType: (alias: string, config?: ApiRequestConfig) => get('integrations/types/' + alias, { ...config }),

  getEmpty: (alias: string, config?: ApiRequestConfig) => get('integrations/empty/' + alias, { ...config }),

  getByAlias: (alias: string, config?: ApiRequestConfig) => get('integrations/' + alias, { ...config }),

  create: (model: any, config?: ApiRequestConfig) => post('integrations', model, config),

  update: (model: any, config?: ApiRequestConfig) => put('integrations/' + model.typeAlias, model, config),

  activate: (alias: string, config?: ApiRequestConfig) => put('integrations/' + alias + '/activate', null, config),

  deactivate: (alias: string, config?: ApiRequestConfig) => put('integrations/' + alias + '/deactivate', null, config),

  delete: (alias: string, config?: ApiRequestConfig) => del('integrations/' + alias, null, config),
};