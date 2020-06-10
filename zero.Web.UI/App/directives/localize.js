import Vue from 'vue';
import Localization from 'zero/services/localization';

/// <summary>
/// Localizes the given property and sets the inner-text of the node to its result
/// </summary>
Vue.directive('localize', (el, binding) =>
{
  if (binding.value !== binding.oldValue || !el.innerText)
  {
    const isObject = typeof binding.value === 'object';
    let key = isObject ? binding.value.key : binding.value;
    let options = isObject ? binding.value : null;

    const result = Localization.localize(key, options);

    // set content as html
    if (binding.arg === 'html')
    {
      el.innerHTML = result;
    }
    // set title
    else if (binding.arg === 'title')
    {
      el.title = result;
    }
    // set attribute
    else if (binding.arg)
    {
      el.setAttribute(binding.arg, result);
    }
    // set content as plain text
    else
    {
      el.innerText = result;
    }
  }
});