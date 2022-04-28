
/**
 * Converts new line chars to <br>
 */
export default (el, binding) =>
{
  if (binding.value !== binding.oldValue)
  {
    el.innerHTML = binding.value.split('\n').join('<br>');
  }
};