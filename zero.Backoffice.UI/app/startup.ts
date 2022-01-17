
import { useAccountStore } from './account/store';
import { useUiStore } from './ui/store';
import { useTranslationStore } from './stores/translations';
import { useAppStore } from './modules/applications/store';
import accountApi from './account/api';
import { Zero } from './core';


export default async function startup(zero: Zero)
{
  const accountStore = useAccountStore();

  let userResponse = null;

  try
  {
    userResponse = await accountApi.getUser();
  }
  catch
  {
    userResponse = { success: false };
  }

  const authenticated = userResponse.success;
  accountStore.user = authenticated ? userResponse.data : undefined;

  if (userResponse.success)
  {
    await useAppStore().setup();
    await useUiStore().setup(false);
    await useTranslationStore().setup();
  }
  else
  {
    await useUiStore().setup(true);
    await useTranslationStore().setup();
  }

  zero.runPlugins();

  return {
    authenticated
  };
}