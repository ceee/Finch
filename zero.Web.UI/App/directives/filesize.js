import Vue from 'vue';
import Strings from 'zero/helpers/strings.js';

/// <summary>
/// Outputs a filesize
/// </summary>
Vue.directive('filesize', (el, binding) =>
{
  if (binding.value !== binding.oldValue)
  {
    el.innerText = Strings.filesize(binding.value);
  }
});