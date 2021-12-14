import { AccountLoggedInResponse, AccountLoginModel, AccountLoginResponse, AccountUser } from 'zero/account';
import { get, post, ApiRequestConfig, ApiResponse } from '../services/request';

export default {

  getUser: async (config?: ApiRequestConfig): Promise<ApiResponse<AccountUser>> => await get('backoffice/account/user', { ...config }),

  isAuthenticated: async (config?: ApiRequestConfig): Promise<ApiResponse<AccountLoggedInResponse>> => await get('backoffice/account/loggedin', { ...config }),

  login: async (model: AccountLoginModel, config?: ApiRequestConfig): Promise<ApiResponse<AccountLoginResponse>> => await post('backoffice/account/login', model, { ...config }),

  logout: async (config?: ApiRequestConfig) => await post('backoffice/account/logout', null, { ...config })
};