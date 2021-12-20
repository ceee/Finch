
export interface EditorFieldMeta
{
  canEdit: boolean;
}


export interface EditorFieldProps<T>
{
  value: T;
  model: any;
  meta: EditorFieldMeta;
  disabled: boolean;
  system: boolean;
}