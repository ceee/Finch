import { defineAsyncComponent } from "vue";
import { ZeroPluginOptions } from "../../core";


export default function createFields(app: ZeroPluginOptions): void
{
  app.fieldType('text', defineAsyncComponent(() => import('./text.vue')));
  app.fieldType('number', defineAsyncComponent(() => import('./number.vue')));
  app.fieldType('toggle', defineAsyncComponent(() => import('./toggle.vue')));
  app.fieldType('rte', defineAsyncComponent(() => import('./rte.vue')));
  app.fieldType('output', defineAsyncComponent(() => import('./output.vue')));
  app.fieldType('state', defineAsyncComponent(() => import('./state.vue')));
  app.fieldType('textarea', defineAsyncComponent(() => import('./textarea.vue')));
}