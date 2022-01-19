import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

export default {
  name: "zero.countries",

  install(app: ZeroPluginOptions)
  {
    app.vue.component('ui-mediapicker', defineAsyncComponent(() => import('./components/ui-mediapicker.vue')));
    app.vue.component('ui-videopicker', defineAsyncComponent(() => import('./components/ui-videopicker.vue')));

    app.fieldType('media', defineAsyncComponent(() => import('./components/field-mediapicker.vue')));
    app.fieldType('image', defineAsyncComponent(() => import('./components/field-imagepicker.vue')));
    app.fieldType('video', defineAsyncComponent(() => import('./components/field-videopicker.vue')));

    app.route({ name: 'media', path: '/media/:parentId?', component: () => import('./pages/overview/overview.vue'), props: true });
    app.route({ name: 'media-edit', path: '/media/edit/:id?', component: () => import('./pages/detail/detail.vue'), props: true });

    //app.schema('countries', () => import('./schemas/list'));
    app.schema('media:edit', () => import('./schemas/editor'));
  }
} as ZeroPlugin;


declare module 'zero/schemas'
{
  export interface ZeroEditorField
  {
    /**
     * Renders a media picker
     * @param {MediaFieldOptions} [options] - Custom options
     */
    media(options?: MediaFieldOptions): ZeroEditorField;

    /**
     * Renders an image picker
     * @param {MediaBaseFieldOptions} [options] - Custom options
     */
    image(options?: MediaBaseFieldOptions): ZeroEditorField;

    /**
     * Renders an video picker
     * @param {PickerFieldOptions} [options] - Custom options
     */
    video(options?: PickerFieldOptions): ZeroEditorField;
  }

  export type MediaTypeFor = 'all' | 'images' | 'videos' | 'documents';

  export interface MediaBaseFieldOptions extends PickerFieldOptions
  {
    disableSelect?: boolean;
    disableUpload?: boolean;
    fileExtensions?: Array<string>;
  }

  export interface MediaFieldOptions extends MediaBaseFieldOptions
  {
    for?: MediaTypeFor;
  }
}