import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

const Page = () => import('./settings.vue');

export default {
  name: "zero.settings",

  install(app: ZeroPluginOptions)
  {
    //app.vue.component('ui-countrypicker', defineAsyncComponent(Picker));
    app.route({ path: '/settings', component: Page });

    //app.editor('country', null);
    //app.editorField('')
  }
} as ZeroPlugin;