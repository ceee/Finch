import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

//const Picker = () => import('./ui-countrypicker.vue');
//const Page = () => import('./_page.vue');

export default {
  name: "zero.applications",

  install(app: ZeroPluginOptions)
  {
    //app.vue.component('ui-countrypicker', defineAsyncComponent(Picker));
    //app.route({ path: '/countries', component: Page });

    //app.editor('country', null);
    //app.editorField('')
  }
} as ZeroPlugin;