import Overlay from 'zero/services/overlay';
import Axios from 'axios';
import PasswordChangeOverlay from 'zero/pages/password-change';
import EventHub from 'zero/services/eventhub';

const data = {
  isAuthenticated: false,
  rejectReason: null,
  user: {
    name: null,
    email: null
  },
};

const _setIsAuthenticated = value =>
{
  data.isAuthenticated = value;
  EventHub.emit('authenticated', value);
};

const _setUser = value =>
{
  data.user = value;
  EventHub.emit('user', value);
};


// check if the user is authenticated
const isAuthenticated = () =>
{
  return data.isAuthenticated;
};


// get the currently logged-in user
const getUser = () =>
{
  return data.user;
};


// loads the current user into the cache
const loadUser = () =>
{
  Axios.get('authentication/getUser').then(res =>
  {
    _setIsAuthenticated(res.data.success && res.data.model);

    if (res.data.success)
    {
      _setUser(res.data.model);
    }
  });
};


// the cached user has been rejected by the server so we clear credentials here
const rejectUser = reason =>
{
  //this.rejectReason = reason;
  _setIsAuthenticated(false);
  _setUser(null);
};


// sets the current user and isAuthenticated to true
const setUser = user =>
{
  if (!user)
  {
    rejectUser();
    return;
  }

  _setIsAuthenticated(true);
  _setUser(user);
};


// logs the user in with the passed credentials
const login = model =>
{
  return Axios.post('authentication/loginUser', model).then(res =>
  {
    return new Promise((resolve, reject) =>
    {
      if (res.data.success)
      {
        setUser(res.data.model);
        resolve(res.data);
      }
      else
      {
        rejectUser();
        reject(res.data.errors);
      }
    });
  });
};


// logs the current user out
const logout = () =>
{
  let promise = Axios.post('authentication/logoutUser');
  rejectUser("@login.rejectReasons.logout");
  return promise;
};


// try to switch selected application for user
const switchApp = appId =>
{
  return Axios.post('authentication/switchApp', null, { params: { appId } }).then(res => Promise.resolve(res.data.success));
};


// open overlay to update password
const openPasswordOverlay = () =>
{
  return Overlay.open({
    title: '@changepasswordoverlay.title',
    closeLabel: '@ui.close',
    confirmLabel: '@changepasswordoverlay.confirm',
    component: PasswordChangeOverlay
  });
};

export default {
  isAuthenticated,
  getUser,
  loadUser,
  rejectUser,
  setUser,
  login,
  logout,
  switchApp,
  openPasswordOverlay
};