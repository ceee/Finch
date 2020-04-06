import Vue from 'vue';
import Localization from 'zeroservices/localization';

/// <summary>
/// Localizes the given property and sets the inner-text of the node to its result
/// </summary>
Vue.filter('localize', (value, force) =>
{
  return Localization.localize(value, force === true);
}); 