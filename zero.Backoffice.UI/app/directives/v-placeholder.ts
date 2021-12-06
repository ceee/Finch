
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
    let value = hasValue ? (isObject ? binding.value.placeholder : binding.value) : null;
    let result = null;

    if (isObject && typeof value === 'function')
    {
      if (typeof binding.value.model === 'undefined')
      {
        // TODO throw warning
        return;
      }
      result = value(binding.value.model);
    }
    else
    {
      result = value;
    }

    result = hasValue ? localize(result) : '';
    el.setAttribute('placeholder', result);
  }
};