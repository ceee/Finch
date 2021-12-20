import { EditorField, EditorFieldConfiguration } from "zero/editor";
import { EditorFieldBase, createFieldProxy } from "./editorField";


export class Editor
{
  fields: EditorField[];

  constructor()
  {
    this.fields = [];
  }

  field(alias: string, config?: EditorFieldConfiguration): EditorField
  {
    const field = new EditorFieldBase(alias, config);
    const proxy = createFieldProxy(field);
    this.fields.push(proxy);
    return proxy;
  }
}


//export interface Editor
//{
//  field(alias: string, config?: EditorFieldConfiguration): EditorField;
//}


export declare type EditorBuilderExpression = (editor: Editor) => void;


export function createEditor(): Editor
{
  return new Editor();
}