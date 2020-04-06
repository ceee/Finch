import Vue from 'vue';
import Localization from 'zeroservices/localization';

/// <summary>
/// Localizes the given property and sets the inner-text of the node to its result
/// </summary>
Vue.directive('localize', {

  bind(el, binding)
  {
    binding.def.localize(el, binding);
  },

  update(el, binding)
  {
    if (binding.value !== binding.oldValue)
    {
      binding.def.localize(el, binding);
    }
  },

  localize(el, binding)
  {
    el.innerText = Localization.localize(binding.value, binding.arg === 'force');
  }
});