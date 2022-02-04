import { get, post, put, del, ApiRequestConfig, ApiRequestQuery } from '../../services/request';

export default {
  query: (term: string, query?: ApiRequestQuery, config?: ApiRequestConfig) => get("search", { ...config, params: { ...query, search: term } })
};