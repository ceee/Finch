import Localization from 'zero/services/localization';
import createListField from 'zero/renderer/createListField';

let field = createListField('text');

field.onRender = (value, config) =>
{
  const hasFunc = typeof config.render === 'function';
  const hasSharedIndicator = config.shared === true && config.model.appId === zero.sharedAppId;

  let html = hasFunc ? config.render(config.model, config) : value;

  if (config.localize === true)
  {
    html = Localization.localize(html, config.tokens || {});
  }

  if (hasSharedIndicator)
  {
    //html = html + ' <i class="ui-table-field-shared is-inline fth-radio"></i>';
    html = html + ' <i class="ui-table-field-shared-2 is-inline">(shared)</i>';
  }

  if (config.as === 'html' || hasSharedIndicator)
  {
    return field.asHTML(html);
  }

  return html;
};

export default field;