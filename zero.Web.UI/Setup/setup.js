import Vue from 'vue';
import Setup from './setup.vue';
//import 'filter/generic.js'
//import 'directive/filedrop.js'

import GlobalComponents from '@zero/components/globals.js';
import Directives from '@zero/directives/globals.js';

new Vue(Setup).$mount('#application'); 