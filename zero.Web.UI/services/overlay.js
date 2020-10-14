import AppConfirm from '@zero/components/overlays/confirm.vue';
import Strings from '@zero/services/strings.js';
import EventHub from '@zero/services/eventhub.js';
import { find as _find, extend as _extend } from 'underscore';

let currentInstance = null;
let instances = [];

// sets a new active dropdown so the old one gets auto-closed
const setDropdown = instance =>
{
  if (currentInstance != null)
  {
    currentInstance.hide();
  }

  currentInstance = instance;
};

// open a deletion confirm dialog with the given options
const confirmDelete = (title, text) =>
{
  let options = _extend({
    title: typeof title === 'string' ? title : '@deleteoverlay.title',
    text: text || '@deleteoverlay.text',
    confirmLabel: '@deleteoverlay.confirm',
    confirmType: 'danger',
    closeLabel: '@deleteoverlay.close',
    component: AppConfirm,
    autoclose: false,
    softdismiss: false
  }, typeof title === 'object' ? title : {});

  return open(options);
};

// open a confirm dialog with the given options
const confirm = (title, text) =>
{
  let options = _extend({
    title: title,
    text: text,
    component: AppConfirm,
    autoclose: true,
    softdismiss: false
  }, typeof title === 'object' ? title : {});

  return open(options);
};


// opens an overlay
const open = options =>
{
  const defaultWidth = options.display === 'editor' ? 560 : 460;

  options = _extend({
    id: Strings.guid(),
    display: 'dialog',
    width: defaultWidth,
    hide: close,
    autoclose: true,
    softdismiss: options.display !== 'editor',
    closeLabel: '@ui.close',
    confirmLabel: '@ui.confirm',
    confirmType: 'default'
  }, options);

  options.theme = 'default';

  instances.push(options);
  EventHub.emit('overlay:open', instances);

  return new Promise((resolve, reject) =>
  {
    options.close = () =>
    {
      close(options);
      reject(options);
    };
    options.confirm = data =>
    {
      if (options.autoclose)
      {
        close(options);
      }
      resolve(data, options);
    };
  });
};


// closes an overlay
const close = instance =>
{
  if (instances.length < 1)
  {
    return;
  }

  if (!instance)
  {
    instances.pop();
    EventHub.emit('overlay:close', instances);
    return;
  }

  if (typeof instance === 'string')
  {
    instance = _find(instances, item => item.id === instance);
  }

  if (instance)
  {
    const index = instances.indexOf(instance);
    instances.splice(index, 1);
    EventHub.emit('overlay:close', instances);
  }
};


export default {
  setDropdown,
  confirmDelete,
  confirm,
  open,
  close
};