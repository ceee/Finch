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

    // set content as html
    if (binding.arg === 'html')
    {
      el.innerHTML = Localization.localize(key, options);
    }
    // set attribute
    else if (binding.arg)
    {
      el.setAttribute(binding.arg, Localization.localize(key, options));
    }
    // set content as plain text
    else
    {
      el.innerText = Localization.localize(key, options);
    }
  }
});