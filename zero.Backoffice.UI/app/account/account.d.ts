
declare module 'zero/account'
{
  export interface AccountUser
  {
    id: string;
    currentAppId: string;
    isSuper: boolean;
    avatarId: string;
    email: string;
    name: string;
    isActive: boolean;
    createdDate: string;
    flavor?: string;
  }

  export interface AccountLoginResponse
  {
    user: AccountUser;
    apiKey: string;
  }

  export interface AccountStoreState
  {
    user?: AccountUser;
  }

  export interface AccountLoginModel
  {
    email: string;
    password: string;
    isPersistent: boolean;
  }

  export interface AccountLoggedInResponse
  {
    loggedIn: boolean;
  }
}