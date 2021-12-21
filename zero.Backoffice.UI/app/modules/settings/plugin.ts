import { ZeroPlugin, ZeroPluginOptions } from '../../core';

export default {
  name: "zero.settings",

  install(app: ZeroPluginOptions)
  {
    app.route({ name: 'settings', path: '/settings', component: () => import('./settings.vue') });
  }
} as ZeroPlugin;