import Vue from 'vue';
import VueRouter from 'vue-router';
import Zero from 'zero/core/zero.ts';
import App from 'zero/app.vue';
import components from 'zero/components/all.js';
import 'zero/directives/globals.js';
import 'zero/filters/globals.js';

Object.entries(components).forEach(cmp =>
{
  Vue.component(cmp[0], cmp[1].default || cmp[1]);
});

Vue.use(VueRouter);
Vue.use(Zero);

App.router = Zero.instance.router;

var app = new Vue(App);

app.$mount('#app');