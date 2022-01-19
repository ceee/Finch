
import { convertHtmlToText } from '../utils';


export default (el, binding) =>
{
  if (binding.value !== binding.oldValue)
  {
    el.innerHTML = convertHtmlToText(binding.value, binding.arg === 'nl');
  }
};