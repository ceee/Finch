import { ZeroPlugin, ZeroPluginOptions } from '../../core';
import { defineAsyncComponent } from 'vue';

export default {
  name: "zero.spaces",

  install(app: ZeroPluginOptions)
  {
    //app.vue.component('ui-countrypicker', defineAsyncComponent(() => import('./ui-countrypicker.vue')));

    //app.schema('spaces:default', () => import('./schemas/list-default'));
    app.schema('pages:zero.folder', () => import('./schemas/folder-editor'));

    app.linkArea('zero.pages', '@zero.config.linkareas.pages', () => import('./partials/linkpicker.vue'));

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
        },
        {
          name: 'pages-create',
          path: 'create/:flavor/:parent?',
          props: true,
          component: () => import('./page.vue')
        },
      ]
    });

    app.link
  }
} as ZeroPlugin;