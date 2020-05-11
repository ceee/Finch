import Axios from 'axios';
import Auth from 'zero/services/auth';

if (!zero || !zero.apiPath)
{
  throw Exception('window.zero and zero.apiPath (= base path to the backoffice API) have to be configured');
}

Axios.defaults.baseURL = zero.apiPath;
Axios.defaults.withCredentials = true;

Axios.interceptors.response.use(response => response, error =>
{
  if (error.response)
  {
    if (error.response.status === 401)
    {
      Auth.rejectUser("@login.rejectReasons.inactive");
      //Notification.error('Authentication failed. Please login again.', 3);
    }
  }

  return Promise.reject(error);
});

export default Axios;