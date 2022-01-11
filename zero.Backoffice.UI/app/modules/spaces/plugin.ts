import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

export default {
  name: "zero.spaces",

  install(app: ZeroPluginOptions)
  {
    //app.vue.component('ui-countrypicker', defineAsyncComponent(() => import('./ui-countrypicker.vue')));

    app.schema('spaces:default', () => import('./schemas/list-default'));

    //app.route({
    //  name: 'spaces',
    //  path: '/spaces',
    //  component: () => import('./spaces.vue'),
    //  children: [
    //    {
    //      name: 'spaces-edit',
    //      path: ':alias/edit/:id?',
    //      props: true,
    //      component: () => import('./spaces.vue')
    //    },
    //    {
    //      name: 'spaces-view',
    //      path: ':alias',
    //      props: true,
    //      component: () => import('./spaces.vue')
    //    }
    //  ]
    //});
  }
} as ZeroPlugin;