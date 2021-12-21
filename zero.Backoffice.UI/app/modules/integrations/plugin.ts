import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

export default {
  name: "zero.integrations",

  install(app: ZeroPluginOptions)
  {
    //app.vue.component('ui-languagepicker', defineAsyncComponent(() => import('./ui-languagepicker.vue')));

    app.route({ name: 'integrations', path: '/settings/integrations', component: () => import('./integrations.vue') });
    app.route({ name: 'integrations-edit', path: '/settings/integrations/edit/:id?', component: () => import('./integration.vue'), props: true });

    app.schema('integrations', () => import('./schemas/list'));
    app.schema('integrations:edit', () => import('./schemas/editor'));
  }
} as ZeroPlugin;