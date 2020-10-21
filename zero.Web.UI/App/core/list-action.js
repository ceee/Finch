
import MediaApi from 'zero/resources/media';
import Strings from 'zero/services/strings';
import Localization from 'zero/services/localization';

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