import { warn } from 'zero/services/debug';


let createListRenderer = function (name, opts)
{
  var config = {
    labelTemplate: null,
    link: null,
    actions: [],
    columns: {}
  };


  // generate a label
  let label = func =>
  {
    if (typeof func === 'string')
    {
      config.labelTemplate = field => func + "fields." + field;
    }
    else if (typeof func === 'function')
    {
      config.labelTemplate = func;
    }
    else
    {
      warn(`createListRenderer: label() parameter has to be either a string (which renders [@my-string].fields.my-field) or a function which returns a string`);
    }
  };


  // generate a link
  let link = func =>
  {
    if (typeof func === 'string')
    {
      config.link = (item, $router) => $router.push({
        name: func,
        params: { id: item.id }
      });
    }
    else if (typeof func === 'function')
    {
      config.link = func;
    }
    else
    {
      warn(`createListRenderer: link() parameter has to be either a string (which calls $router.push({ name: [my-string], params: { id: [item.id] } }) or a function which is called on click`);
    }
  };


  // adds a list action
  let action = function (key, label, icon, action)
  {
    config.actions.push({
      key,
      label,
      icon,
      action
    });
  };


  // adds a column
  let column = function (field)
  {
    return {
      image: () => { },
      text: () => { },
      checkbox: () => { }
    };
  };


  return { 
    label,
    link,
    action,
    column
  };
};


export default createListRenderer;