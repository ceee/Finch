import { warn } from 'zero/services/debug';
import MediaApi from 'zero/resources/media';

export default function (el, binding)
{
  const item = binding.value.item;
  const column = binding.value.column.column;
  const value = item[column.path];

  if (column.isHtml)
  {
    el.innerHTML = column.render(value, item);
  }
  else
  {
    el.innerText = column.render(value, item);
  }

  //let render = (value, asHtml) =>
  //{
  //  if (asHtml)
  //  {
  //    el.innerHTML = value;
  //  }
  //  else
  //  {
  //    el.innerText = value;
  //  }
  //};

  // set multiline for cell
  //el.classList.toggle('is-multiline', column.multiline === true);
  //el.classList.toggle('is-bold', column.bold === true);
}