import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

export default {
  name: "zero.links",

  install(app: ZeroPluginOptions)
  {
    app.vue.component('ui-linkpicker', defineAsyncComponent(() => import('./ui-linkpicker.vue')));

    app.fieldType('linkPicker', defineAsyncComponent(() => import('./editor-linkpicker.vue')));

    //app.schema('countries', () => import('./schemas/list'));
    //app.schema('countries:edit', () => import('./schemas/editor'));
  }
} as ZeroPlugin;


declare module 'zero/schemas'
{
  export interface ZeroEditorField
  {
    /**
     * Renders a link picker
     * @param {LinkPickerFieldOptions} [options] - Custom options
     */
    linkPicker(options?: LinkPickerFieldOptions): ZeroEditorField;
  }


  export interface LinkPickerFieldOptions extends PickerFieldOptions
  {
    areas?: string[];
    allowTitle?: boolean;
    allowTarget?: boolean;
    allowSuffix?: boolean;
  }
}

declare module 'zero/ui'
{
  export interface UiLink
  {
    area: string;
    target: 'default' | 'self' | 'blank';
    urlSuffix?: string;
    title?: string;
    values: Record<string, unknown>;
  }

  export interface UiLinkPreview
  {
    id: string;
    icon?: string;
    text: string;
    name: string;
    hasError: boolean;
  }
}