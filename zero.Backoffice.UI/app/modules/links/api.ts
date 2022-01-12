import { post, ApiRequestConfig, ApiResponse } from '../../services/request';
import { UiLink, UiLinkPreview } from 'zero/ui';

export default {

  convert: (items: UiLink[], config?: ApiRequestConfig): Promise<ApiResponse<UiLinkPreview[]>> => post('backoffice/links/convert', items, config),

};