
import { App, defineAsyncComponent } from 'vue';

export default function (app: App)
{
  app.component('ui-editor', defineAsyncComponent(() => import('./ui-editor.vue')));
  app.component('ui-editor-infos', defineAsyncComponent(() => import('./ui-editor-infos.vue')));
  app.component('ui-editor-header', defineAsyncComponent(() => import('./ui-editor-header.vue')));
};