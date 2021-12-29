
import { toCurrency } from '../utils/numbers';

/**
 * Outputs a currency
 */
export default (el, binding) =>
{
  if (binding.value !== binding.oldValue)
  {
    el.innerHTML = toCurrency(binding.value);
  }
};