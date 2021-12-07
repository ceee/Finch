
export interface ZeroEditor
{
  
}



export declare type BooleanExpression<TModel, TField> = (model: TModel, value: TField) => boolean;



export interface ZeroEditorField<TModel, TField>
{
  when(condition: BooleanExpression<TModel, TField>): ZeroEditorField<TModel, TField>;

  required(required: boolean): ZeroEditorField<TModel, TField>;

  required(condition: BooleanExpression<TModel, TField>): ZeroEditorField<TModel, TField>;
}


var field = {
  when(condition)
  {
    return this;
  },
  required(required: boolean)
  {
    return this;
  },
  required(condition: BooleanExpression<any, any>)
  {
    return this;
  }
} as ZeroEditorField<any, any>;