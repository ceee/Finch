import { createApp } from 'vue';
import App from 'zero/app';

zero.apps = {
  shared: true
};

import Router from 'zero/router.config.js';
//import 'zero/components/globals';
import 'zero/directives/globals';
import 'zero/filters/globals';
//import 'zero/renderers/globals';
//import 'zero/pages/register';

//import 'zero/zero.plugins.js';

const app = createApp(App);
app.use(Router);
app.mount('#app');
