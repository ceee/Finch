
declare module 'zero/schemas'
{
  export interface ZeroEditorField
  {
    /**
     * Set this field as required
     * @param {function|boolean} [condition] - Optionally only require this field when a condition is fulfilled or reset the required state with true/false
     */
    number(options?: NumberFieldOptions): ZeroEditorField;
  }
}