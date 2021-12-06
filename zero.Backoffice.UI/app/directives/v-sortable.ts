
import Sortable from 'sortablejs';

/**
 * Outputs a formatted date
 */
export default (el, binding) =>
{
  if (binding.value === binding.oldValue)
  {
    return;
  }

  let sortable = new Sortable(el, binding.value || {});
};