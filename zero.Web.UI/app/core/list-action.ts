
import MediaApi from 'zero/resources/media.js';
import Strings from 'zero/services/strings.js';
import Localization from 'zero/services/localization.js';

class ListAction
{
  key;
  label;
  icon;
  autoclose = true;
  action = () =>
  {
    console.warn(`[zero] A list action needs a "action" callback`);
  };

  constructor(key, label, icon, action)
  {
    this.key = key;
    this.label = label;
    this.icon = icon;
    this.action = action;
  }

  /**
   * Calls the action
   * @returns {ListColumn}
   */
  call(options)
  {
    console.info(options);
    this.action(options);
  }
}


export default ListAction;