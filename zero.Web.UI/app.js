import Vue from 'vue';
import App from 'zero/app';

zero.apps = {
  shared: true
};

import 'zero/components/globals';
import 'zero/directives/globals';
import 'zero/filters/globals';
import 'zero/renderers/globals';
//import 'zero/pages/register';

import 'zero/zero.plugins.js';

new Vue(App).$mount('#app'); 