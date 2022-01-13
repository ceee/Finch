
import { localize } from '../services/localization';

/**
 * Localizes the given property and sets the inner-text of the node to its result
 */
export default (el, binding) =>
{
  if (binding.value !== binding.oldValue || !el.innerText)
  {
    const hasValue = !!binding.value;
    const isObject = typeof binding.value === 'object';
    let key = hasValue ? (isObject ? binding.value.key : binding.value) : null;
    let options = hasValue && isObject ? binding.value : null;
    let html = isObject ? binding.value.html : binding.arg === 'html';

    const result = hasValue ? localize(key, options) : '';

    // set content as html
    if (html)
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
};