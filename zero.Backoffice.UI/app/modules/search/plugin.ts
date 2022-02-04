import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

export default {
  name: "zero.search",

  install(app: ZeroPluginOptions)
  {
    app.vue.component('app-search', defineAsyncComponent(() => import('./button.vue')));
  }
} as ZeroPlugin;