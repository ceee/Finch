import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

export default {
  name: "zero.pagemodules",

  install(app: ZeroPluginOptions)
  {
    app.vue.component('ui-modules', defineAsyncComponent(() => import('./ui-modules.vue')));
    app.fieldType('modules', defineAsyncComponent(() => import('./editor-modules.vue')));
  }
} as ZeroPlugin;


declare module 'zero/schemas'
{
  export interface ZeroEditorField
  {
    /**
     * Renders modules
     * @param {ModulesFieldOptions} [options] - Custom options
     */
    modules(options?: ModulesFieldOptions): ZeroEditorField;
  }

  export interface ModulesFieldOptions extends PickerFieldOptions
  {
    
  }
}