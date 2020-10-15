import { createApp } from 'vue';

import App from './app.vue';
import Router from './config/router.config.js';
import registerDirectives from './directives/register.js';
import registerComponents from './components/register.js';
import registerRenderers from './renderers/register.js';

zero.renderers = zero.renderers || {};

//import '@zero/config/zero.plugins.js';
//import ZeroPlugin from '@zero/plugin.js';

const app = createApp(App);
app.use(Router);

registerDirectives(app);
registerComponents(app);
registerRenderers(zero.renderers);

//registerRenderers(app);

app.mount('#app');