
/**
 * Limits an element to a maximum number of text lines (can be toggled with click)
 */
export default (el, binding) =>
{
  if (binding.value !== binding.oldValue)
  {
    if (!el.__zero_maxlines)
    {
      el.__zero_maxlines = true;
      el.addEventListener('click', e =>
      {
        el.classList.toggle('is-expanded');
      });
    }

    el.classList.add('ui-maxlines');
    el.style.webkitLineClamp = +binding.value > 0 ? +binding.value : null;
  }
};