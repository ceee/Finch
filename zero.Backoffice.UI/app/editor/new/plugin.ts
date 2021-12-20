import { defineAsyncComponent } from 'vue';
import { ZeroPlugin, ZeroPluginOptions } from '../../core';

export default {
  name: "zero.editor",

  install(app: ZeroPluginOptions)
  {
    app.fieldType('text', defineAsyncComponent(() => import('./fields/text.vue')));
  }
} as ZeroPlugin;