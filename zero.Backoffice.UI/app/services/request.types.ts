
import { AxiosRequestConfig } from 'axios';

export interface ApiRequestConfig extends AxiosRequestConfig
{
  raw?: boolean;
  system?: boolean;
  scope?: string;
}

export interface ApiResponseBase
{
  success: boolean;
  status: number;
  metadata: ApiResponseMetadata;
}

export interface ApiResponseMetadata
{
  requestDate: Date;
  duration: string;
}

export interface ApiErrorBase extends ApiResponseBase
{
  errors: ApiResponseError[];
}

export interface ApiResponse<T> extends ApiResponseBase, ApiErrorBase
{
  data?: T;
  changeToken?: string;
}

export interface ApiPagedResponse<T> extends ApiResponseBase, ApiErrorBase
{
  data?: T[];
  paging?: ApiResponsePaging;
}

export interface ApiResponseError
{
  code: string;
  category: string;
  message: string;
  property: string;
}

export interface ApiResponsePaging
{
  page: number;
  pageSize: number;
  totalPages: number;
  totalItems: number;
}

export interface ApiRequestQuery
{
  /**
   * Current page index (starts at 1) 
   **/
  page?: number;
  /**
   * Items per page (defaults to 30)
   **/
  pageSize?: number;
  /**
   * Limit query to specified IDs
   **/
  ids?: string[];
  /**
   * Search query string
   **/
  search?: string;
}