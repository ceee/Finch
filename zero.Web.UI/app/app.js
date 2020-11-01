import Vue from 'vue';
import VueRouter from 'vue-router';
import Zero from 'zero/core/zero.ts';
import App from 'zero/app.vue';
import 'zero/components/globals.js';
import 'zero/directives/globals.js';
import 'zero/filters/globals.js';

Vue.use(VueRouter);
Vue.use(Zero);

App.router = Zero.instance.router;

var app = new Vue(App);

app.$mount('#app');