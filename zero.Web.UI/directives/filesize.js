import Strings from '@zero/services/strings.js';

/// <summary>
/// Outputs a filesize
/// </summary>
export default {
  name: 'filesize',

  beforeMount(el, binding)
  {
    if (binding.value !== binding.oldValue)
    {
      el.innerText = Strings.filesize(binding.value);
    }
  }
};