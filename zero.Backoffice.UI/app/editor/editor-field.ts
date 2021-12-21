import { ZeroEditorField } from "zero/schemas";
import { extendObject } from '../utils/objects';
import { ZeroFieldType } from "./_new/types";

export declare type BooleanExpression = (model: any, value: any) => boolean;


export const createFieldProxy = (field: ZeroEditorFieldImpl) => new Proxy(field, {
  get: function (target: ZeroEditorFieldImpl, prop: string | symbol, receiver: any): any
  {
    // handle internals
    if (prop in target)
    {   
      return target[prop]; // @tsignore
    }

    // handle dynamic fields + extensions
    return (args) =>
    {
      target.custom(prop, args);
      return target;
    };
  }
})// as EditorField;


// @ts-ignore
export class ZeroEditorFieldImpl implements ZeroEditorField
{
  path: string;
  configuration: ZeroEditorFieldConfiguration;
  fieldType: string;
  options?: any;

  constructor(path: string, config?: ZeroEditorFieldConfiguration)
  {
    this.path = path;
    this.configuration = extendObject(createDefaultFieldConfiguration(), config || {}) as ZeroEditorFieldConfiguration;
  }


  /**
   * Set this field as optional
   * @param {function|boolean} [condition] - Optionally only require this field when a condition is fulfilled or reset the required state with true/false
   */
  setOptional(condition: Function | boolean): ZeroEditorField
  {
    this.configuration.optional = condition;
    return this;
  }

  /**
   * Whether the input next to the headline or below
   * @param {boolean} isHorizontal
   * @returns {EditorField}
   */
  setHorizontal(isHorizontal: boolean): ZeroEditorField
  {
    this.configuration.horizontal = !isHorizontal;
    return this;
  }


  /**
   * Set this field to disabled
   */
  setReadonly(condition: Function | boolean): ZeroEditorField
  {
    this.configuration.readonly = condition;
    return this;
  }


  /**
   * Conditionally hide this field
   * @param {function} condition - function which returns a boolean and gets passed the current model
   */
  setHidden(condition: Function | boolean): ZeroEditorField
  {
    this.configuration.hidden = condition;
    return this;
  }


  /**
   * Set a custom component for a field
   * @param {Component} component - The component to render (can be an async component too)
   * @param {T} [options] = Custom options to pass to this editor
   */
  custom<T>(fieldType: string, options?: T): ZeroEditorField
  {
    this.fieldType = fieldType;
    this.options = options;
    return this;
  }
}



export function createDefaultFieldConfiguration(): ZeroEditorFieldConfiguration
{
  return {
    optional: false,
    readonly: false,
    hidden: false,
    label: null,
    hideLabel: false,
    description: null,
    helpText: null,
    classes: null,
    horizontal: false,
    sort: 0
  } as ZeroEditorFieldConfiguration;
}


export interface ZeroEditorFieldConfiguration
{
  /**
   * Whether this field is optional or required (additional validation is done on the server)
   */
  optional?: Function | boolean;
  /**
   * Whether this field is readonly and can't be changed
   */
  readonly?: Function | boolean;
  /**
   * Conditionally hide the field
   */
  hidden?: Function | boolean;
  /**
   * A custom label for this field (otherwise it's generated via `onLabelCreate`)
   **/
  label?: string | null,
  /**
   * Hide the field label
   **/
  hideLabel?: boolean,
  /**
   * A custom description for this field (otherwise it's generated via `onDescriptionCreate`)
   **/
  description?: string | null,
  /**
   * Display a help text below the field
   **/
  helpText?: string | null,
  /**
   * Append HTML class to the generated property
   **/
  classes?: string | null,
  /**
   * Whether to render the label next to the input
   **/
  horizontal?: boolean,
  /**
   * Sort order for fields within the editor canvas
   **/
  sort?: number
}