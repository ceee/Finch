

var Helpers = {

  isServer(vNode)
  {
    return false;
    //return typeof vNode.componentInstance !== 'undefined' && vNode.componentInstance.$isServer;
  },

  isPopup(popupItem, elements)
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
  },

  validate(binding)
  {
    if (typeof binding.value !== 'function')
    {
      console.warn('v-click-outside: provided expression ' + binding.expression + ' is not a function.');
      return false;
    }

    return true;
  }
};


/// calls the passed function when a click happens outside the target element
export default {
  name: 'click-outside',

  beforeMount(el, binding, vNode)
  {
    if (!Helpers.validate(binding)) return;

    // Define Handler and cache it on the element
    function handler(e)
    {
      if (!binding.instance) return;

      // some components may have related popup item, on which we shall prevent the click outside event handler.
      var elements = e.path || (e.composedPath && e.composedPath());
      elements && elements.length > 0 && elements.unshift(e.target);

      if (el.contains(e.target) || Helpers.isPopup(binding.instance.popupItem, elements) || !el.__vueClickOutside__)
      {
        return;
      }

      el.__vueClickOutside__.callback(e);
    }

    // add Event Listeners
    el.__vueClickOutside__ = {
      handler: handler,
      callback: binding.value
    };

    setTimeout(() =>
    {
      !Helpers.isServer(vNode) && document.addEventListener('click', handler);
    }, 200);
  },

  updated(el, binding)
  {
    if (Helpers.validate(binding))
    {
      el.__vueClickOutside__.callback = binding.value;
    }
  },

  unmounted(el, binding, vNode)
  {
    !Helpers.isServer(vNode) && document.removeEventListener('click', el.__vueClickOutside__.handler);
    delete el.__vueClickOutside__;
  }
};