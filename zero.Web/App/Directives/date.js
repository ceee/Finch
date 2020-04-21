import Vue from 'vue';
import dayjs from 'dayjs';

const DATETIME_FORMAT = 'DD.MM.YY HH:mm';
const DATE_FORMAT = 'DD.MM.YY';

/// <summary>
/// Outputs a formatted date
/// </summary>
Vue.directive('date', (el, binding) =>
{
  if (binding.value !== binding.oldValue)
  {
    if (!binding.value)
    {
      el.innerHTML = '';
      return;
    }

    el.innerHTML = dayjs(binding.value).format(DATE_FORMAT);
  }
});