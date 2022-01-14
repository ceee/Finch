import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

export default {
  name: "zero.mailTemplates",

  install(app: ZeroPluginOptions)
  {
    app.vue.component('ui-mailtemplatepicker', defineAsyncComponent(() => import('./picker/ui-mailtemplatepicker.vue')));
    app.fieldType('mailTemplatePicker', defineAsyncComponent(() => import('./picker/editor-mailtemplatepicker.vue')));

    app.route({ name: 'mailtemplates', path: '/settings/mailtemplates', component: () => import('./mailtemplates.vue') });
    app.route({ name: 'mailtemplates-edit', path: '/settings/mailtemplates/edit/:id?', component: () => import('./mailtemplate.vue'), props: true });

    app.schema('mailtemplates', () => import('./schemas/list'));
    app.schema('mailtemplates:edit', () => import('./schemas/editor'));
  }
} as ZeroPlugin;


declare module 'zero/schemas'
{
  export interface ZeroEditorField
  {
    /**
     * Renders a mail template picker
     * @param {PickerFieldOptions} [options] - Custom options
     */
    mailTemplatePicker(options?: PickerFieldOptions): ZeroEditorField;
  }
}