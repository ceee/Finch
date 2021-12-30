
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

    /**
     * Select one of the predefined values
     * @param {SelectFieldOptions} options - Custom options
     */
    select(options: SelectFieldOptions): ZeroEditorField;

    /**
     * Create a tag list
     * @param {TagFieldOptions} options - Custom options
     */
    tags(options?: TagFieldOptions): ZeroEditorField;

    /**
     * Create a list of checkboxes
     * @param {ChecklistFieldOptions} options - Custom options
     */
    checklist(options: ChecklistFieldOptions): ZeroEditorField;

    /**
     * Create a list of inputs
     * @param {ChecklistFieldOptions} options - Custom options
     */
    inputlist(options: ChecklistFieldOptions): ZeroEditorField;
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
    items: SelectFieldItem[];
  }

  export interface SelectFieldOptions
  {
    items: SelectFieldItem[];
    emptyOption?: boolean | null;
  }

  export interface SelectFieldItem
  {
    value: any;
    label: string;
    disabled?: boolean;
  }

  export interface TagFieldOptions
  {
    limit?: number;
    maxTagLength?: number;
    autocompleteScope?: string;
  }

  export interface ChecklistFieldOptions
  {
    items: SelectFieldItem[];
    limit?: number;
    reverse?: boolean | null;
    labelKey?: string;
    idKey?: string;
  }

  export interface InputlistFieldOptions
  {
    limit?: number;
    maxItemLength?: number;
    addLabel?: string;
  }
}