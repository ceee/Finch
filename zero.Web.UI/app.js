import { createApp } from 'vue';
import App from 'zero/app';

zero.apps = {
  shared: true
};

import Router from 'zero/router.config.js';
import registerComponents from 'zero/components/register';
import registerDirectives from 'zero/directives/register';
import registerRenderers from 'zero/renderers/register';

import 'zero/zero.plugins.js';
//import ZeroPlugin from 'zero/plugin.js';

const app = createApp(App);
app.use(Router);

registerDirectives(app);
registerComponents(app);
registerRenderers(app);

app.mount('#app');

//console.info(ZeroPlugin);