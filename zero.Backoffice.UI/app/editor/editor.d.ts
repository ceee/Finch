

declare module 'zero/editor'
{
  export interface FieldSupportsMaxLength
  {
    maxLength?: number;
  }

  export interface FieldSupportsPlaceholder
  {
    placeholder?: string;
  }

  export type TextFieldOptions = FieldSupportsMaxLength | FieldSupportsPlaceholder;
  export type NumberFieldOptions = FieldSupportsMaxLength | FieldSupportsPlaceholder;


  export interface EditorField
  {
    text(options?: TextFieldOptions): void;
    number(options?: NumberFieldOptions): void;
  }
}