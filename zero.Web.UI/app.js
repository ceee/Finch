import Vue from 'vue';
import VueRouter from 'vue-router';
import Zero from 'zero/core/zero.js';
import App from 'zero/app';
import 'zero/components/globals';
import 'zero/directives/globals';
import 'zero/filters/globals';

zero.apps = {
  shared: true
};

//import 'zero/pages/register';

//import ZeroPlugin from 'zero/config/zero.plugin.js';

Vue.use(VueRouter);
Vue.use(Zero);

import 'zero/config/zero.plugins.js';

new Vue(App).$mount('#app'); 