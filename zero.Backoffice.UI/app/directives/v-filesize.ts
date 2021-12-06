
import { getFilesize } from '../utils/filesize';

/**
 * Outputs a filesize
 */
export default (el, binding) =>
{
  if (binding.value !== binding.oldValue)
  {
    el.innerText = getFilesize(binding.value);
  }
};