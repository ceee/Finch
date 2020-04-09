import Axios from 'axios';
import Auth from 'zeroservices/auth';

if (!zero || !zero.path)
{
  throw Exception('window.zero and zero.path (= base path) have to be configured');
}

Axios.defaults.baseURL = zero.path + 'api/';
Axios.defaults.withCredentials = true;

Axios.interceptors.response.use(response => response, error =>
{
  if (error.response)
  {
    if (error.response.status === 401)
    {
      Auth.rejectUser();
      //Notification.error('Authentication failed. Please login again.', 3);
    }
  }

  return Promise.reject(error);
});

export default Axios;