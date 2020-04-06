import Vue from 'vue';
import Setup from './setup.vue';
//import 'filter/generic.js'
//import 'directive/filedrop.js'

import GlobalComponents from 'zerocomponents/globals.js';
import Directives from 'zerodirectives/globals.js';

new Vue(Setup).$mount('#application'); 