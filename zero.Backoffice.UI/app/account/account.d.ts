
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
}