import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

//const Picker = () => import('./ui-countrypicker.vue');
//const Page = () => import('./_page.vue');

export default {
  name: "zero.applications",

  install(app: ZeroPluginOptions)
  {
    app.route({ name: 'applications', path: '/settings/applications', component: () => import('./applications.vue') });
    app.route({ name: 'applications-edit', path: '/settings/applications/edit/:id', component: () => import('./application.vue'), props: true });

    app.schema('applications:edit', () => import('./schemas/editor'));
  }
} as ZeroPlugin;