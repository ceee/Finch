import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

export default {
  name: "zero.translations",

  install(app: ZeroPluginOptions)
  {
    //app.vue.component('ui-languagepicker', defineAsyncComponent(() => import('./ui-languagepicker.vue')));

    app.route({ name: 'translations', path: '/settings/translations', component: () => import('./translations.vue') });
    app.route({ name: 'translations-edit', path: '/settings/translations/edit/:id?', component: () => import('./translation.vue'), props: true });

    app.schema('translations', () => import('./schemas/list'));
    app.schema('translations:edit', () => import('./schemas/editor'));
  }
} as ZeroPlugin;