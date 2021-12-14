import { AccountLoggedInResponse, AccountLoginModel, AccountLoginResponse, AccountUser } from 'zero/account';
import { get, post, ApiRequestConfig, ApiResponse } from '../services/request';

export default {

  getUser: (config?: ApiRequestConfig): Promise<ApiResponse<AccountUser>> => get('backoffice/account/user', { ...config }),

  isAuthenticated: (config?: ApiRequestConfig): Promise<ApiResponse<AccountLoggedInResponse>> => get('backoffice/account/loggedin', { ...config }),

  login: (model: AccountLoginModel, config?: ApiRequestConfig): Promise<ApiResponse<AccountLoginResponse>> => post('backoffice/account/login', model, { ...config }),

  logout: (config?: ApiRequestConfig) => post('backoffice/account/logout', null, { ...config })
};