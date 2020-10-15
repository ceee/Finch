import Strings from 'zero/services/strings';

/// <summary>
/// Outputs a formatted date
/// </summary>
export default {
  beforeMount(el, binding)
  {
    if (binding.value !== binding.oldValue)
    {
      if (!binding.value)
      {
        el.innerHTML = '-';
        return;
      }

      el.innerHTML = Strings.date(binding.value);
    }
  }
};