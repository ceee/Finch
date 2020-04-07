import Vue from 'vue';

/// <summary>
/// This directive is used to bind an element (button or link) to a dropdown
/// </summary>
Vue.directive('dropdown', {

  bind(el, binding)
  {
    console.info(el, binding);
  },

  update(el, binding)
  {
    
  }
});