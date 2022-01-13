import { ZeroEditor } from "../editor";

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

    /**
     * Create a nested editor
     * @param {NestedFieldOptions} options - Custom options
     */
    nested(options: NestedFieldOptions): ZeroEditorField;

    /**
    * Create a date picker
    * @param {DatePickerFieldOptions} options - Custom options
    */
    datePicker(options?: DatePickerFieldOptions): ZeroEditorField;

    /**
    * Create an icon picker
    * @param {DatePickerFieldOptions} options - Custom options
    */
    iconPicker(options?: IconPickerFieldOptions): ZeroEditorField;
  }


  export interface IconPickerFieldOptions
  {
    set?: string;
    colors?: boolean;
  }

  export interface DatePickerFieldOptions
  {
    format?: string;
    pickTime?: boolean;
    maxDate?: string | Date;
    minDate?: string | Date;
    amPm?: boolean;
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
    html?: boolean;
    render?: (value: any, model: any) => string;
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

  export interface NestedFieldOptions
  {
    editor: string | ZeroEditor;
    limit?: number;
    title?: string;
    itemLabel?: ((value: any, model: any) => string);
    itemDescription?: ((value: any, model: any) => string);
    itemIcon?: string | ((value: any, model: any) => string);
    width?: number;
    template?: any;
    addLabel?: string;
  }

  export interface PickerFieldOptions
  {
    limit?: number;
  }
}