import Vue from 'vue';
import Strings from 'zero/services/strings';

/// <summary>
/// Outputs a currency
/// </summary>
Vue.directive('currency', (el, binding) =>
{
  if (binding.value !== binding.oldValue)
  {
    el.innerHTML = Strings.currency(binding.value);
  }
});