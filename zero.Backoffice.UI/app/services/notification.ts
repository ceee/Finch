
import emitter from './eventhub';
import { extendObject } from '../utils/objects';
import { generateId } from '../utils/numbers';


export const event_showNotification = 'zero.notification.show';
export const event_hideNotifications = 'zero.notification.hide';

export type NotificationType = 'default' | 'success' | 'error' | 'warning';

export interface NotificationOptions
{
  type: NotificationType,
  label: string,
  text?: string,
  persistent?: boolean,
  duration?: number
}

export interface NotificationMessage extends NotificationOptions
{
  id: string
}


export function show(label: string, text?: string, options?: NotificationOptions)
{
  return sendInternal('default', label, text, options);
}

export function success(label: string, text?: string, options?: NotificationOptions)
{
  return sendInternal('success', label, text, options);
}

export function warning(label: string, text?: string, options?: NotificationOptions)
{
  return sendInternal('warning', label, text, options);
}

export function error(label: string, text?: string, options?: NotificationOptions)
{
  return sendInternal('error', label, text, options);
}

export function hideAll(label: string, text?: string, options?: NotificationOptions)
{
  emitter.emit(event_hideNotifications);
}


function sendInternal(type: NotificationType, label: string, text?: string, options?: NotificationOptions)
{
  options = options || {} as NotificationOptions;
  options.label = label;
  options.text = text;
  options.type = type;
  return send(options);
}


export function send(options: NotificationOptions)
{
  options = extendObject({

    type: 'default',
    label: null,
    text: null,
    persistent: false,
    duration: 3000
    //close: () =>
    //{
    //  clearTimeout(options.timeout);
    //  me.close(this.instances.indexOf(options));
    //}
  }, options) as NotificationOptions;

  //options.timeout = setTimeout(() =>
  //{
  //  options.close();
  //}, options.duration);

  //this.instances.push(options);

  emitter.emit(event_showNotification, {
    id: generateId(),
    type: options.type,
    label: options.label,
    text: options.text,
    persistent: options.persistent,
    duration: options.duration
  } as NotificationMessage);
}