import Strings from 'zero/helpers/strings.js';
import Localization from 'zero/helpers/localization.js';
import Sortable from 'sortablejs';
import * as ClickOutsideHelpers from './clickoutside.js';
import { Resizable as ResizableHelper } from './resizable.js';


/**
 * Localizes the given property and sets the inner-text of the node to its result
 */
export var localize = (el, binding) =>
{
  if (binding.value !== binding.oldValue || !el.innerText)
  {
    const hasValue = !!binding.value;
    const isObject = typeof binding.value === 'object';
    let key = hasValue ? (isObject ? binding.value.key : binding.value) : null;
    let options = hasValue && isObject ? binding.value : null;

    const result = hasValue ? Localization.localize(key, options) : '';

    // set content as html
    if (binding.arg === 'html')
    {
      el.innerHTML = result;
    }
    // set title
    else if (binding.arg === 'title')
    {
      el.title = result;
    }
    // set attribute
    else if (binding.arg)
    {
      el.setAttribute(binding.arg, result);
    }
    // set content as plain text
    else
    {
      el.innerText = result;
    }
  }
};


/**
 * Sets the placeholder to the desired string or executes a function which gets the placeholder string
 */
export var placeholder = (el, binding) =>
{
  if (binding.value !== binding.oldValue || !el.innerText)
  {
    const hasValue = !!binding.value;
    const isObject = typeof binding.value === 'object';
    let value = hasValue ? (isObject ? binding.value.placeholder : binding.value) : null;
    let result = null;

    if (isObject && typeof value === 'function')
    {
      if (typeof binding.value.model === 'undefined')
      {
        // TODO throw warning
        return;
      }
      result = value(binding.value.model);
    }
    else
    {
      result = value;
    }

    result = hasValue ? Localization.localize(result) : '';
    el.setAttribute('placeholder', result);
  }
};



/**
 * Outputs a formatted date
 */
export var date = (el, binding) =>
{
  if (binding.value !== binding.oldValue)
  {
    if (!binding.value)
    {
      el.innerHTML = '-';
      return;
    }

    el.innerHTML = Strings.date(binding.value);
  }
};


/** 
 * Enables sorting of a list
 */
export var sortable = (el, binding) =>
{
  if (binding.value === binding.oldValue)
  {
    return;
  }

  let sortable = new Sortable(el, binding.value || {});
};


/**
 * Outputs a filesize
 */
export var filesize = (el, binding) =>
{
  if (binding.value !== binding.oldValue)
  {
    el.innerText = Strings.filesize(binding.value);
  }
};


/**
 * Outputs a currency
 */
export var currency = (el, binding) =>
{
  if (binding.value !== binding.oldValue)
  {
    el.innerHTML = Strings.currency(binding.value);
  }
};


/**
 * resize an element
 */
export var resizable = {
  bind(el, binding)
  {
    let resizable = new ResizableHelper(el, binding.value);
    resizable.listen();
  }
};


/**
 * Limits an element to a maximum number of text lines (can be toggled with click)
 */
export var maxLines = (el, binding) =>
{
  if (binding.value !== binding.oldValue)
  {
    if (!el.__zero_maxlines)
    {
      el.__zero_maxlines = true;
      el.addEventListener('click', e =>
      {
        el.classList.toggle('is-expanded');
      });
    }

    el.classList.add('ui-maxlines');
    el.style.webkitLineClamp = +binding.value > 0 ? +binding.value : null;
  }
};


/**
 * resize an element
 */
export var clickOutside = {
  bind(el, binding, vNode)
  {
    if (!ClickOutsideHelpers.validate(binding)) return;

    // Define Handler and cache it on the element
    function handler(e)
    {
      if (!vNode.context) return;

      // some components may have related popup item, on which we shall prevent the click outside event handler.
      var elements = e.path || (e.composedPath && e.composedPath());
      elements && elements.length > 0 && elements.unshift(e.target);

      if (el.contains(e.target) || ClickOutsideHelpers.isPopup(vNode.context.popupItem, elements) || !el.__vueClickOutside__)
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
      !ClickOutsideHelpers.isServer(vNode) && document.addEventListener('click', handler);
    }, 200);
  },

  update(el, binding)
  {
    if (ClickOutsideHelpers.validate(binding))
    {
      el.__vueClickOutside__.callback = binding.value;
    }
  },

  unbind(el, binding, vNode)
  {
    !ClickOutsideHelpers.isServer(vNode) && document.removeEventListener('click', el.__vueClickOutside__.handler);
    delete el.__vueClickOutside__;
  }
};