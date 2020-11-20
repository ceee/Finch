import Vue from 'vue';
import Localization from 'zero/helpers/localization.js';

/// <summary>
/// Localizes the given property and sets the inner-text of the node to its result
/// </summary>
Vue.filter('localize', (value, options) =>
{
  return Localization.localize(value, options);
}); 