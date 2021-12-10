import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

export default {
  name: "zero.countries",

  install(app: ZeroPluginOptions)
  {
    app.vue.component('ui-countrypicker', defineAsyncComponent(() => import('./ui-countrypicker.vue')));
    app.route({ path: '/settings/countries', component: () => import('./countries.vue') });

    app.schema('countries', () => import('./list'));
    app.schema('country', () => import('./editor'));
  }
} as ZeroPlugin;