
import { createApp } from 'vue';
import { createPinia } from 'pinia';
import App from './app.vue';
import { createZeroPlugin } from './core';

const app = createApp(App)
  .use(createPinia())
  .use(createZeroPlugin());

app.mount('#app');