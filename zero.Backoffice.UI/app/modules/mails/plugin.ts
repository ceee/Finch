import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

export default {
  name: "zero.mailTemplates",

  install(app: ZeroPluginOptions)
  {
    //app.vue.component('ui-countrypicker', defineAsyncComponent(() => import('./ui-countrypicker.vue')));

    app.route({ name: 'mailtemplates', path: '/settings/mailtemplates', component: () => import('./mailtemplates.vue') });
    app.route({ name: 'mailtemplates-edit', path: '/settings/mailtemplates/edit/:id?', component: () => import('./mailtemplate.vue'), props: true });

    app.schema('mailtemplates', () => import('./schemas/list'));
    app.schema('mailtemplates:edit', () => import('./schemas/editor'));
  }
} as ZeroPlugin;