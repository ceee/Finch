
export interface ZeroEditor
{
  
}


export class ZeroFieldComponent
{

}


export class ZeroEditorCanvasFieldBase<TModel>
{
  field<TField>(path: string, component: ZeroFieldComponent): ZeroEditorField<TModel, TField>
  {
    return new ZeroEditorField();
  }
}


export class ZeroEditorCanvasBase<TModel> extends ZeroEditorCanvasFieldBase<TModel>
{
  field<TField>(path: string, component: ZeroFieldComponent): ZeroEditorField<TModel, TField>
  {
    return new ZeroEditorField();
  }

  fieldset(): ZeroEditorCanvasFieldBase<TModel>
  {
    return new ZeroEditorCanvasFieldBase();
  }
}


export class ZeroEditorCanvas<TModel> extends ZeroEditorCanvasBase<TModel>
{
  tab(alias: string, name: string): ZeroEditorTab<TModel>
  {
    return new ZeroEditorTab(alias, name);
  }
}


export class ZeroEditorTab<TModel> extends ZeroEditorCanvasBase <TModel>
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
  when(condition: BooleanExpression<TModel, TField>): ZeroEditorField<TModel, TField>
  {
    return this;
  }

  required(required: boolean | BooleanExpression<TModel, TField>): ZeroEditorField<TModel, TField>
  {
    return this;
  }
}


//var editor = new ZeroEditorCanvas<any>();

//editor.field('hi', null);
//editor.fieldset();
//editor.tab('hi', 'ho').