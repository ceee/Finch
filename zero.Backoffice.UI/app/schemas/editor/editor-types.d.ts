
declare module 'zero/schemas'
{
  export interface FieldSupportsMaxLength
  {
    maxLength?: number;
  }


  export interface FieldSupportsPlaceholder
  {
    placeholder?: string | null;
  }


  export type TextFieldOptions = FieldSupportsMaxLength | FieldSupportsPlaceholder;
  export type NumberFieldOptions = FieldSupportsMaxLength | FieldSupportsPlaceholder;


  export interface ZeroEditorField
  {
    /**
     * Set this field as optional
     * @param {function|boolean} [condition] - Optionally only require this field when a condition is fulfilled or reset the required state with true/false
     */
    setOptional(condition: Function | boolean): ZeroEditorField;

    /**
     * Whether the input next to the headline or below
     * @param {boolean} isHorizontal
     * @returns {EditorField}
     */
    setHorizontal(isHorizontal: boolean): ZeroEditorField;

    /**
     * Set this field to disabled
     */
    setReadonly(condition: Function | boolean): ZeroEditorField;

    /**
     * Conditionally hide this field
     * @param {function} condition - function which returns a boolean and gets passed the current model
     */
    setHidden(condition: Function | boolean): ZeroEditorField;

    /**
     * Set a custom component for a field
     * @param {Component} component - The component to render (can be an async component too)
     * @param {T} [options] = Custom options to pass to this editor
     */
    custom<T>(component: Component, options?: T): ZeroEditorField;

    /**
     * Set this field as required
     * @param {function|boolean} [condition] - Optionally only require this field when a condition is fulfilled or reset the required state with true/false
     */
    //text(options?: TextFieldOptions): EditorField;

    ///**
    // * Set this field as required
    // * @param {function|boolean} [condition] - Optionally only require this field when a condition is fulfilled or reset the required state with true/false
    // */
    //number(options?: NumberFieldOptions): EditorField;
  }
}