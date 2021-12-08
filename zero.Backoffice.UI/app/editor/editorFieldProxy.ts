import { EditorField } from "zero/editor";

export const fieldTypes: Record<string, any> = {};

export const proxy = new Proxy(fieldTypes, {

  get(target, handler)
  {
    console.info({ target, handler });
    return target[handler];
  }

}) as EditorField;

//var field = editor.field('name').text("hallo");