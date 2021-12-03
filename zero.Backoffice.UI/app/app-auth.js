import Vue from 'vue';
import VueRouter from 'vue-router';
import Zero from 'zero/core/zero.ts';
import App from 'zero/app.vue';
import components from 'zero/components/all.js';
import * as directives from 'zero/directives/all.js';
//import 'zero/directives/globals.js';
import 'zero/config/vue.config.js';
import 'zero/config/axios.config.js';

Object.entries(components).forEach(cmp => {
  Vue.component(cmp[0], cmp[1].default || cmp[1]);
});

Object.entries(directives).forEach(cmp => {
  Vue.directive(cmp[0], cmp[1]);
});

Vue.component('MyAddButton', () => import('zero/components/buttons/add-button-2.vue'));

Vue.use(VueRouter);
Vue.use(Zero);

App.router = Zero.instance.router;

var app = new Vue(App);

app.$mount('#app');