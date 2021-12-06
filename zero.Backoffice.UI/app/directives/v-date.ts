
import { formatDate } from '../utils/dates';

/**
 * Outputs a formatted date
 */
export default (el, binding) =>
{
  if (binding.value !== binding.oldValue)
  {
    if (!binding.value)
    {
      el.innerHTML = '-';
      return;
    }

    el.innerHTML = formatDate(binding.value);
  }
};