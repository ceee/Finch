
import { Component, ComponentPropsOptions } from 'vue';
import { proxy, fieldTypes } from '../editorFieldProxy';
import TestCmp from './_testcmp.vue';

export interface ZeroEditor
{
  
}


export interface ZeroFieldType
{
  component: Promise<Component>;
  options: any;
}




//const testField = {
//  component: () => import('./_testcmp.vue'),
//  options: {
//    maxLength: {
//      type: Number,
//      default: null
//    },
//    placeholder: {
//      type: [String, Function],
//      default: null
//    }
//  }

//} as ZeroFieldType;


export class ZeroEditorCanvasBase<TModel>
{
  field<TField>(path: string, component: Component): ZeroEditorField<TModel, TField>
  {
    return new ZeroEditorField(path, component);
  }
}


export class ZeroEditorCanvasBaseWithFieldset<TModel> extends ZeroEditorCanvasBase<TModel>
{
  fieldset(): ZeroEditorCanvasBase<TModel>
  {
    return new ZeroEditorCanvasBase();
  }
}


export class ZeroEditorCanvas<TModel> extends ZeroEditorCanvasBaseWithFieldset<TModel>
{
  tab(alias: string, name: string): ZeroEditorTab<TModel>
  {
    return new ZeroEditorTab(alias, name);
  }
}


export class ZeroEditorTab<TModel> extends ZeroEditorCanvasBaseWithFieldset <TModel>
{
  alias: string;
  name: string;

  constructor(alias: string, name: string)
  {
    super();
    this.alias = alias;
    this.name = name;
  }
}


export declare type BooleanExpression<TModel, TField> = (model: TModel, value: TField) => boolean;


export class ZeroEditorField<TModel, TField>
{
  path: string;
  component: Component;

  constructor(path: string, component: Component)
  {
    this.path = path;
    this.component = component;
  }

  when(condition: BooleanExpression<TModel, TField>): ZeroEditorField<TModel, TField>
  {
    return this;
  }

  required(required: boolean | BooleanExpression<TModel, TField>): ZeroEditorField<TModel, TField>
  {
    return this;
  }

  configure(opts: ComponentPropsOptions)
  {

  }
}


export function test()
{
  fieldTypes.text = (maxLength?: number, placeholder?: string | Function) => console.log(`text() called with maxLength: ${maxLength}, placeholder: ${placeholder}`);

  proxy.text(17, 'Enter your text...');

  //var editor = new ZeroEditorCanvas<any>();

  //editor.field('hi', TestCmp)

  //console.info(TestCmp.);
}