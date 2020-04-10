import dayjs from 'dayjs';

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


  // render empty
  if (isEmpty)
  {
    render(column.default || value);
  }
  // output text string or HTML
  else if (!column.as || column.as === 'text' || column.as === 'html')
  {
    const hasFunc = typeof column.render === 'function';
    render(hasFunc ? column.render(item, column) : value, column.as === 'html');
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
  else
  {
    console.warn('ui-table: Column display type ("as") is not supported');
  }
}