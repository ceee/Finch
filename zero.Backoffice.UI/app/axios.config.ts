import Axios from 'axios';
//import Auth from 'zero/helpers/auth.js';
import Qs from 'qs';

//if (!__zero || !__zero.apiPath)
//{
//  throw Exception('window.zero and zero.apiPath (= base path to the backoffice API) have to be configured');
//}

//Axios.defaults.baseURL = '/zero/api/';
Axios.defaults.withCredentials = true;

Axios.defaults.paramsSerializer = (params) =>
{
  return Qs.stringify(params, { allowDots: true });
};

Axios.interceptors.response.use(
  response =>
  {
    //if (response.headers['x-variant'] == 'api-response')
    //{
    //  return response.data;
    //}

    return response;
  },
  error =>
  {
    if (error.response && error.response.headers['x-variant'] == 'api-response')
    {
      return Promise.resolve(error.response);
    }

    if (error.response && error.response.status === 401)
    {
      console.error('[zero.axios] Auth failed. Please login again.');
      //Auth.rejectUser("@login.rejectReasons.inactive");
      //Notification.error('Authentication failed. Please login again.', 3);
    }

    return Promise.reject(error);
  }
);

Axios.interceptors.request.use(config =>
{
  if (location.search)
  {
    var query = Qs.parse(location.search.substring(1));
    if (query.scope)
    {
      if (!config.params)
      {
        config.params = {};
      }
      config.params['scope'] = query.scope;
    }
  }
  return config;
}, error => Promise.reject(error));

export default Axios;