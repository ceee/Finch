
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
     * Render a text area
     * @param {TextFieldOptions} [options] - Custom options
     */
    textarea(options?: TextFieldOptions): ZeroEditorField;

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

    /**
     * Output a value
     * @param {OutputFieldOptions} [options] - Custom options
     */
    output(options?: OutputFieldOptions): ZeroEditorField;

    /**
     * Select one of the predefined values
     * @param {StateFieldOptions} options - Custom options
     */
    state(options: StateFieldOptions): ZeroEditorField;
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

  export interface OutputFieldOptions
  {
    render: (value: any, model: any) => string;
  }

  export interface StateFieldOptions
  {
    items: StateFieldItem[];
  }

  export interface StateFieldItem
  {
    value: any;
    label: string;
    disabled?: boolean;
  }
}