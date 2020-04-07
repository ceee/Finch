import Vue from 'vue';
import Localization from 'zeroservices/localization';

/// <summary>
/// Localizes the given property and sets the inner-text of the node to its result
/// </summary>
Vue.directive('localize', (el, binding) =>
{
  if (binding.value !== binding.oldValue)
  {
    const isObject = typeof binding.value === 'object';
    let key = isObject ? binding.value.key : binding.value;
    let options = isObject ? binding.value : null;

    el.innerText = Localization.localize(key, options);
  }
});