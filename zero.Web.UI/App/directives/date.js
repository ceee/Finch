import Vue from 'vue';
import Strings from 'zero/helpers/strings.js';

/// <summary>
/// Outputs a formatted date
/// </summary>
Vue.directive('date', (el, binding) =>
{
  if (binding.value !== binding.oldValue)
  {
    if (!binding.value)
    {
      el.innerHTML = '-';
      return;
    }

    el.innerHTML = Strings.date(binding.value);
  }
});