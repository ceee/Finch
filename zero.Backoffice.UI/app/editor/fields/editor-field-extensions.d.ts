
declare module 'zero/schemas'
{
  export interface ZeroEditorField
  {
    /**
     * Render a text input
     * @param {TextFieldOptions} [options] - Custom options
     */
    text(options?: TextFieldOptions): ZeroEditorField;

    /**
     * Render a number input
     * @param {NumberFieldOptions} [options] - Custom options
     */
    number(options?: NumberFieldOptions): ZeroEditorField;

    /**
     * Render a toggle
     * @param {ToggleFieldOptions} [options] - Custom options
     */
    toggle(options?: ToggleFieldOptions): ZeroEditorField;

    /**
     * Render a rich-text-editor
     * @param {RteFieldOptions} [options] - Custom options
     */
    rte(options?: RteFieldOptions): ZeroEditorField;
  }


  export interface ToggleFieldOptions
  {
    negative?: boolean | null;
    onContent?: string | null;
    offContent?: string | null;
  }

  export type RteFieldOptions = FieldSupportsMaxLength | FieldSupportsPlaceholder | FieldSupportsSetup;

  export interface FieldSupportsSetup
  {
    setup: boolean;
  }
}