import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

export default {
  name: "zero.users",

  install(app: ZeroPluginOptions)
  {
    //app.vue.component('ui-countrypicker', defineAsyncComponent(() => import('./ui-countrypicker.vue')));

    app.route({ name: 'users', path: '/settings/users', component: () => import('./users.vue') });
    app.route({ name: 'users-edit', path: '/settings/users/edit/:id?', component: () => import('./user.vue'), props: true });

    app.schema('users', () => import('./schemas/list'));
    app.schema('users:edit', () => import('./schemas/editor'));
  }
} as ZeroPlugin;