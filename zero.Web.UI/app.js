import { createApp } from 'vue';
import App from './app.vue';

zero.apps = {
  shared: true
};

import registerDirectives from './directives/register.js';
import Router from './config/router.config.js';
import registerComponents from './components/register.js';
//import registerRenderers from './renderers/register.js';

//import '@zero/config/zero.plugins.js';
//import ZeroPlugin from '@zero/plugin.js';

const app = createApp(App);
app.use(Router);

registerDirectives(app);
registerComponents(app);
//registerRenderers(app);

app.mount('#app');