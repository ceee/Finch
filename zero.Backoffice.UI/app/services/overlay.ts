
import emitter from './eventhub';
import { extendObject } from '../utils/objects';
import { generateId } from '../utils/numbers';


export const event_showOverlay = 'zero.overlay.show';
export const event_closeOverlays = 'zero.overlay.closeall';
export const event_closeOverlay = 'zero.overlay.close';
export const event_finalizeOverlay = 'zero.overlay.finalize';

export type OverlayDisplay = 'dialog' | 'editor';
export type OverlayTheme = 'default' | 'light' | 'dark';

export interface OverlayOptions
{
  alias?: string,
  display?: OverlayDisplay,
  width?: number,
  autoclose?: boolean,
  softdismiss?: boolean,
  component?: any,
  theme?: OverlayTheme,
  model?: any
}

export interface OverlayInstance extends OverlayOptions
{
  id: string
}


export function confirmDelete(title?: string, text?: string)
{
  return open({
    alias: 'confirm',
    model: {
      title: typeof title === 'string' ? title : '@deleteoverlay.title',
      text: typeof text === 'string' ? title : '@deleteoverlay.text',
      confirmLabel: '@deleteoverlay.confirm',
      confirmType: 'danger',
      closeLabel: '@deleteoverlay.close',
    },
    autoclose: false,
    softdismiss: true,
    component: () => import('../ui/overlays/confirm.vue')
  });
}


export function confirm(title: string, text?: string)
{
  return open({
    alias: 'confirm',
    model: {
      title,
      text,
      confirmLabel: '@ui.confirm',
      confirmType: 'default',
      closeLabel: '@ui.close',
    },
    autoclose: false,
    softdismiss: true,
    component: () => import('../ui/overlays/confirm.vue')
  });
}


export function messsage(title: string, text?: string)
{
  return open({
    alias: 'message',
    model: {
      title: title,
      text: text,
    },
    autoclose: true,
    softdismiss: true,
    component: () => import('../ui/overlays/message.vue')
  });
}


export function closeAll()
{
  emitter.emit(event_closeOverlays);
}


export function open(options: OverlayOptions)
{
  options = extendObject({
    alias: 'default',
    display: 'dialog',
    width: options.display === 'editor' ? 560 : 460,
    //hide: this.close,
    autoclose: true,
    softdismiss: options.display !== 'editor',
    theme: 'default',
    model: null,
    component: null
  } as OverlayOptions, options) as OverlayOptions;

  const instance = {
    id: generateId(),
    ...options,
    close(force?: boolean)
    {
      emitter.emit(event_finalizeOverlay, { eventType: 'close', instance, force: force });
    },
    confirm(value: any)
    {
      emitter.emit(event_finalizeOverlay, { eventType: 'confirm', instance, value });
    }
  } as OverlayInstance;

  emitter.emit(event_showOverlay, instance);


  return new Promise((resolve, reject) =>
  {
    emitter.on(event_finalizeOverlay, (opts: any) =>
    {
      opts.close = () =>
      {
        emitter.emit(event_closeOverlay, instance);
      };
      resolve(opts);
    });
  });

  //return new Promise((resolve, reject) =>
  //{
  //  options.close = () =>
  //  {
  //    this.close(options);
  //    reject(options);
  //    // TODO should we move to resolve here, so we don't trigger errors in case the implementation does not catch them?
  //    // this will at least need some tests if the .then callback does not catch null values
  //  };
  //  options.hide = options.close;

  //  options.confirm = data =>
  //  {
  //    if (options.autoclose)
  //    {
  //      this.close(options);
  //    }
  //    resolve(data, options);
  //  };
  //});

  //options = extendObject({

  //  type: 'default',
  //  label: null,
  //  text: null,
  //  persistent: false,
  //  duration: 3000
  //}, options) as OverlayOptions;
}