import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

export default {
  name: "zero.countries",

  install(app: ZeroPluginOptions)
  {
    app.vue.component('ui-countrypicker', defineAsyncComponent(() => import('./ui-countrypicker.vue')));

    app.fieldType('countryPicker', defineAsyncComponent(() => import('./editor-countrypicker.vue')));

    app.route({ name: 'countries', path: '/settings/countries', component: () => import('./countries.vue') });
    app.route({ name: 'countries-edit', path: '/settings/countries/edit/:id?', component: () => import('./country.vue'), props: true });

    app.schema('countries', () => import('./schemas/list'));
    app.schema('countries:edit', () => import('./schemas/editor'));
  }
} as ZeroPlugin;


declare module 'zero/schemas'
{
  export interface ZeroEditorField
  {
    /**
     * Renders a country picker
     * @param {PickerFieldOptions} [options] - Custom options
     */
    countryPicker(options?: PickerFieldOptions): ZeroEditorField;
  }
}