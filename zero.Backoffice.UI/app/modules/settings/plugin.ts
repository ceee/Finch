import { ZeroPlugin, ZeroPluginOptions } from '../../core';

export default {
  name: "zero.settings",

  install(app: ZeroPluginOptions)
  {
    app.route({ name: 'settings', path: '/settings', component: () => import('./settings.vue') });
    //app.route({ name: 'demo', path: '/settings/demo', component: () => import('./demo.vue') });

    app.schema('demo', () => import('./demo'));
  }
} as ZeroPlugin;