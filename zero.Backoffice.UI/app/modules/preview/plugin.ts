import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

export default {
  name: "zero.preview",

  install(app: ZeroPluginOptions)
  {
    app.route({ name: 'preview', path: '/preview', component: () => import('./preview.vue'), props: true });
  }
} as ZeroPlugin;