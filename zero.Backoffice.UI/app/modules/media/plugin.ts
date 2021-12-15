import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

export default {
  name: "zero.countries",

  install(app: ZeroPluginOptions)
  {
    //app.vue.component('ui-countrypicker', defineAsyncComponent(() => import('./ui-countrypicker.vue')));

    app.route({ name: 'media', path: '/media/:parentId?', component: () => import('./overview.vue'), props: true });
    app.route({ name: 'media-edit', path: '/media/edit/:id?', component: () => import('./detail.vue'), props: true });

    //app.schema('countries', () => import('./schemas/list'));
    //app.schema('countries:edit', () => import('./schemas/editor'));
  }
} as ZeroPlugin;