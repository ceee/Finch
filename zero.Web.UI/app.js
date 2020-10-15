import { createApp } from 'vue';
import App from '@zero/app.vue';

zero.apps = {
  shared: true
};

import registerDirectives from '@zero/directives/register.js';
//import Router from '@zero/config/router.config.js';
//import registerComponents from '@zero/components/register.js';
//import registerRenderers from '@zero/renderers/register.js';

//import '@zero/config/zero.plugins.js';
//import ZeroPlugin from '@zero/plugin.js';

const app = createApp(App);
//app.use(Router);

registerDirectives(app);
//registerComponents(app);
//registerRenderers(app);

console.info(app);

app.mount('#app');