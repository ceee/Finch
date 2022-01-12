import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

export default {
  name: "zero.spaces",

  install(app: ZeroPluginOptions)
  {
    app.schema('pages:zero.folder', () => import('./schemas/folder-editor'));

    app.linkArea('zero.pages', '@zero.config.linkareas.pages', () => import('./partials/linkpicker.vue'));

    app.route({
      name: 'pages',
      path: '/pages',
      component: () => import('./pages.vue'),
      children: [
        {
          name: 'pages-edit',
          path: 'edit/:id?',
          props: true,
          component: () => import('./page.vue')
        },
        {
          name: 'pages-create',
          path: 'create/:flavor/:parent?',
          props: true,
          component: () => import('./page.vue')
        },
      ]
    });

    app.vue.component('ui-pagepicker', defineAsyncComponent(() => import('./picker/ui-pagepicker.vue')));
    app.fieldType('pagePicker', defineAsyncComponent(() => import('./picker/field-pagepicker.vue')));
  }
} as ZeroPlugin;


declare module 'zero/schemas'
{
  export interface ZeroEditorField
  {
    /**
     * Renders a page picker
     * @param {PagePickerFieldOptions} [options] - Custom options
     */
    pagePicker(options?: PagePickerFieldOptions): ZeroEditorField;
  }

  export interface PagePickerFieldOptions extends PickerFieldOptions
  {
    rootId?: string | ((model: any) => string);
    disabledIds?: string[] | ((model: any) => string[]);
  }
}