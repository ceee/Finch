
export var isServer = vNode =>
{
  return typeof vNode.componentInstance !== 'undefined' && vNode.componentInstance.$isServer;
};

export var isPopup = (popupItem, elements) =>
{
  if (!popupItem || !elements)
  {
    return false;
  }

  for (var i = 0, len = elements.length; i < len; i++)
  {
    try
    {
      if (popupItem.contains(elements[i]))
      {
        return true;
      }
      if (elements[i].contains(popupItem))
      {
        return false;
      }
    }
    catch (e)
    {
      return false;
    }
  }

  return false;
};

export var validate = (binding) =>
{
  if (typeof binding.value !== 'function')
  {
    console.warn('v-click-outside: provided expression ' + binding.expression + ' is not a function.');
    return false;
  }

  return true;
};