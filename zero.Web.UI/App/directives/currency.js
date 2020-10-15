import Strings from 'zero/services/strings';

/// <summary>
/// Outputs a currency
/// </summary>
export default {
  name: 'currency',

  beforeMount(el, binding)
  {
    if (binding.value !== binding.oldValue)
    {
      el.innerHTML = Strings.currency(binding.value);
    }
  }
};