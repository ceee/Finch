
import { App } from 'vue';
import registerGeneric from './generic/register';
import registerTabs from './tabs/register';

export default function (app: App)
{
  registerGeneric(app);
  registerTabs(app);
};