
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

  constructor(key, label, icon, action, autoclose: boolean = true)
  {
    this.key = key;
    this.label = label;
    this.icon = icon;
    this.action = action;
    this.autoclose = autoclose;
  }

  /**
   * Calls the action
   * @returns {ListColumn}
   */
  call(options, cmp)
  {
    this.action(options, cmp);
  }
}


export default ListAction;