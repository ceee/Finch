
import { App, defineAsyncComponent } from 'vue';

export default function (app: App)
{
  app.component('ui-editor', defineAsyncComponent(() => import('./ui-editor.vue')));
  app.component('ui-editor-component', defineAsyncComponent(() => import('./ui-editor-component.vue')));
  app.component('ui-editor-meta', defineAsyncComponent(() => import('./ui-editor-meta.vue')));
  app.component('ui-editor-header', defineAsyncComponent(() => import('./ui-editor-header.vue')));
  app.component('ui-editor-page', defineAsyncComponent(() => import('./ui-editor-page.vue')));
};