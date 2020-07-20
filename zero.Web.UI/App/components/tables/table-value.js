import dayjs from 'dayjs';
import { warn } from 'zero/services/debug';
import MediaApi from 'zero/resources/media';

export default function (el, binding)
{
  const item = binding.value.item;
  const column = binding.value.column;
  const value = item[column.field];
  const isEmpty = typeof value === 'undefined' || value === null;

  let render = (value, asHtml) =>
  {
    if (asHtml)
    {
      el.innerHTML = value;
    }
    else
    {
      el.innerText = value;
    }
  };

  // set multiline for cell
  el.classList.toggle('is-multiline', column.multiline === true);
  el.classList.toggle('is-bold', column.bold === true);


  // render empty
  //if (isEmpty)
  //{
  //  render(column.default || value);
  //}
  // output text string or HTML
  if (!column.as || column.as === 'text' || column.as === 'html')
  {
    const hasFunc = typeof column.render === 'function';
    const hasSharedIndicator = column.shared === true && item.appId === zero.sharedAppId;

    let html = hasFunc ? column.render(item, column) : value;
    let isHtml = column.as === 'html' || hasSharedIndicator;

    if (hasSharedIndicator)
    {
      //html = html + ' <i class="ui-table-field-shared is-inline fth-radio"></i>';
      html = html + ' <i class="ui-table-field-shared-2 is-inline">(shared)</i>';
    }

    render(html, isHtml);
  }
  // formatted date with optional time
  else if (column.as === 'date' || column.as === 'datetime')
  {
    let format = column.as === 'datetime' ? 'DD.MM.YY HH:mm' : 'DD.MM.YY';
    render(dayjs(value).format(format));
  }
  // render formatted price
  else if (column.as === 'price')
  {
    let price = isNaN(value) ? 0 : value;
    let hasDecimals = ~~price !== price;

    price = hasDecimals ? (price / 1).toFixed(2) : ~~price;

    render(price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, "&nbsp;") + "&nbsp;&euro;", true);
  }
  // render checkbox
  else if (column.as === 'bool')
  {
    render('<span class="ui-table-field-bool' + (value === true ? ' is-checked' : '') + (column.colored ? ' is-colored' : '') + '"></span>', true);
  }
  // render an image
  else if (column.as === 'image')
  {
    render(value ? `<img src="${MediaApi.getImageSource(value)}" class="ui-table-field-image">` : '', true);
  }
  // render global flag
  else if (column.as === 'shared')
  {
    render(value === zero.sharedAppId ? '<i class="ui-table-field-shared fth-radio"></i>' : '', true);
  }
  else
  {
    warn(`ui-table: Column display type ("${column.as}") is not supported`, this);
  }
}