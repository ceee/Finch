import Vue from 'vue';

Vue.config.errorHandler = (err, vm, info) =>
{
  console.error(err);
};

Vue.config.warnHandler = (msg, vm, trace) =>
{
  console.error(`[zero warn]: ${msg}${trace}`);
};