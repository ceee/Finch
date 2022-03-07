import { post, ApiResponse, ApiRequestConfig } from '../../services/request';

export default {

  createPreviewToken: (key: string, config?: ApiRequestConfig): Promise<ApiResponse<any>> => post('backoffice/ui/previews/token', { key }, { ...config }),

};