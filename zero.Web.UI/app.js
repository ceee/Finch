import { createApp } from 'vue';
import App from 'zero/app';

zero.apps = {
  shared: true
};

import Router from 'zero/router.config.js';
//import 'zero/components/globals';
import registerDirectives from 'zero/directives/register';
//import 'zero/filters/globals';
//import 'zero/renderers/globals';
//import 'zero/pages/register';

//import 'zero/zero.plugins.js';

const app = createApp(App);
app.use(Router);
registerDirectives(app);

console.info(app);
app.mount('#app');
