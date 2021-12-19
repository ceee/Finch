import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

export default {
  name: "zero.spaces",

  install(app: ZeroPluginOptions)
  {
    //app.vue.component('ui-countrypicker', defineAsyncComponent(() => import('./ui-countrypicker.vue')));

    //app.schema('spaces:default', () => import('./schemas/list-default'));

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
        }
      ]
    });
  }
} as ZeroPlugin;