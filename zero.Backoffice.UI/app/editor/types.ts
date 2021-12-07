
export interface ZeroEditor
{
  
}


export class ZeroFieldComponent // TODO this needs to extend a vue component
{

}

export class ConfigurableZeroFieldComponent<TConfiguration = any> extends ZeroFieldComponent
{

}


export class ZeroEditorCanvasBase<TModel>
{
  field<TField, TConfiguration>(path: string, component: ZeroFieldComponent | ConfigurableZeroFieldComponent<TConfiguration>): ZeroEditorField<TModel, TField, TConfiguration>
  {
    return new ZeroEditorField();
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


export class ZeroEditorField<TModel, TField, TConfiguration>
{
  when(condition: BooleanExpression<TModel, TField>): ZeroEditorField<TModel, TField, TConfiguration>
  {
    return this;
  }

  required(required: boolean | BooleanExpression<TModel, TField>): ZeroEditorField<TModel, TField, TConfiguration>
  {
    return this;
  }
}


//var editor = new ZeroEditorCanvas<any>();

//editor.field('hi', null);
//editor.fieldset();
//editor.tab('hi', 'ho').