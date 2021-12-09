import { EditorField } from "zero/editor";

export const fieldTypes: Record<string, any> = {};

export const proxy = new Proxy(fieldTypes, {

  get(target, handler)
  {
    console.info({ target, handler });
    return target[handler];
  }

}) as EditorField;

//fieldTypes.number = (maxLength?: number, placeholder?: string | Function) => console.log(`number() called with maxLength: ${maxLength}, placeholder: ${placeholder}`);
//proxy.number({ maxLength: 17, placeholder: 'Enter your number...' });

//var field = editor.field('name').text("hallo");