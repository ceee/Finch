
import { createApp } from 'vue';
import { createPinia } from 'pinia';
import App from './app.vue';
import directives from './directives/all';

const app = createApp(App)
  .use(createPinia());

directives.forEach(directive =>
{
  app.directive(directive.key, directive.definition);
});

app.mount('#app');