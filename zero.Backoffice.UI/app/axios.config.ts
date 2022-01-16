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
  response => response,
  error =>
  {
    if (error.response && error.response.headers['x-variant'] == 'api-response')
    {
      return Promise.resolve(error.response);
    }

    if (error.response && error.response.status === 401)
    {
      console.error('[zero.axios] Auth failed. Please login again.');
      return Promise.resolve(error.response);
      //Auth.rejectUser("@login.rejectReasons.inactive");
      //Notification.error('Authentication failed. Please login again.', 3);
    }

    return Promise.reject(error);
  }
);

Axios.interceptors.request.use(config =>
{
  if (!config.params)
  {
    config.params = {};
  }

  let locationQuery = Qs.parse(location.search.substring(1));
  let appKey = 'hofbauer';

  // set app key to system when required
  if (config.params['zero.system'] === true || locationQuery['zero.shared'] === "true")
  {
    delete config.params['zero.system'];
    appKey = 'system';
  }

  // rewrite URLs to replace {app} placeholder
  if (config.url != null && config.url.indexOf('{app}') > -1)
  {
    config.url = config.url.replace('{app}', appKey);
  }
  if (config.baseURL != null && config.baseURL.indexOf('{app}') > -1)
  {
    config.baseURL = config.baseURL.replace('{app}', appKey);
  }

  return config;
}, error => Promise.reject(error));

export default Axios;