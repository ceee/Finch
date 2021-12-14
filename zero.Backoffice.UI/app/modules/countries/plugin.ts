import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

export default {
  name: "zero.countries",

  install(app: ZeroPluginOptions)
  {
    app.vue.component('ui-countrypicker', defineAsyncComponent(() => import('./ui-countrypicker.vue')));
    app.route({ name: 'countries', path: '/settings/countries', component: () => import('./countries.vue') });
    app.route({ name: 'countries-edit', path: '/settings/countries/edit/:id', component: () => import('./country.vue'), props: true });

    app.schema('countries', () => import('./list'));
    app.schema('country', () => import('./editor'));
  }
} as ZeroPlugin;