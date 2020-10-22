import Vue from 'vue';
import VueRouter from 'vue-router';
import Zero from 'zero/core/zero.js';
import App from 'zero/app';
import 'zero/components/globals';
import 'zero/directives/globals';
import 'zero/filters/globals';

Vue.use(VueRouter);
Vue.use(Zero);

App.router = Zero.instance.router;

var app = new Vue(App);

app.$mount('#app');