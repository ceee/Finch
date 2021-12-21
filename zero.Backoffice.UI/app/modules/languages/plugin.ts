import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { TextFieldOptions } from 'zero/editor';
import { defineAsyncComponent } from 'vue';

export default {
  name: "zero.languages",

  install(app: ZeroPluginOptions)
  {
    app.vue.component('ui-languagepicker', defineAsyncComponent(() => import('./ui-languagepicker.vue')));

    app.route({ name: 'languages', path: '/settings/languages', component: () => import('./languages.vue') });
    app.route({ name: 'languages-edit', path: '/settings/languages/edit/:id?', component: () => import('./language.vue'), props: true });

    app.schema('languages', () => import('./schemas/list'));
    app.schema('languages:edit', () => import('./schemas/editor'));

    app.fieldType('languagePicker', defineAsyncComponent(() => import('./field-languagepicker.vue')));
  }
} as ZeroPlugin;