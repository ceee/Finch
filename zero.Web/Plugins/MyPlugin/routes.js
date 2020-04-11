
import View from './myplugin';
import ViewSettings from './myplugin-settings';

export default [
  { path: '/myplugin', component: View, name: 'MyPlugin' },
  { path: '/myplugin/settings', component: ViewSettings, name: 'MyPluginSettings' }
];