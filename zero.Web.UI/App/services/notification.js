import Strings from 'zero/services/strings';
import { find as _find, extend as _extend } from 'underscore';

let instances = [];

const success = (label, text, options) =>
{
  show(_extend({
    type: 'success',
    label: label,
    text: text
  }, options || {}));
};


const error = (label, text, options) =>
{
  show(_extend({
    type: 'error',
    label: label,
    text: text
  }, options || {}));
};


const show = (options, text) =>
{
  if (typeof options === 'string')
  {
    options = {
      label: options,
      text: text
    };
  }

  options = _extend({
    id: Strings.guid(),
    type: 'default',
    label: null,
    text: null,
    persistent: false,
    duration: 3000,
    timeout: null,
    close: () =>
    {
      clearTimeout(options.timeout);
      close(instances.indexOf(options));
    }
  }, options);

  if (!options.label && !options.text)
  {
    return;
  }

  options.timeout = setTimeout(() =>
  {
    options.close();
  }, options.duration);

  instances.push(options);
};


const close = index =>
{
  if (index > -1)
  {
    instances.splice(index, 1);
  }
  else
  {
    instances = [];
  }
};


export default {
  success,
  error,
  show,
  close
};