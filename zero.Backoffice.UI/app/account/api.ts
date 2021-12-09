import { AccountLoginModel, AccountUser } from 'zero/account';
import { get, post, ZeroRequestConfig } from '../services/request';

export default {

  getUser: async (config?: ZeroRequestConfig): Promise<AccountUser> => await get('backoffice/account/user', { ...config }),

  isAuthenticated: async (config?: ZeroRequestConfig) => await get('backoffice/account/loggedin', { ...config }),

  login: async (model: AccountLoginModel, config?: ZeroRequestConfig) => await post('backoffice/account/login', model, { ...config }),

  logout: async (config?: ZeroRequestConfig) => await post('backoffice/account/logout', null, { ...config })
};