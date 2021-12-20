
import { Component, defineAsyncComponent } from 'vue';
import { EditorField, EditorFieldConfiguration, TextFieldOptions, EditorFieldDefinition } from 'zero/editor';

export const fieldTypes: Record<string, Function> = {};


fieldTypes.text = function (options?: TextFieldOptions)
{
  return field.custom(defineAsyncComponent(() => import('./fields/text.vue')), options);
};


export const createFieldProxy = (field: EditorFieldBase) => new Proxy(field, {
  get: function (target: EditorFieldBase, prop: string | symbol, receiver: any): any
  {
    // handle internals
    if (prop in target)
    {
      return target[prop];
    }

    // handle dynamic fields + extensions
    if (fieldTypes[prop])
    {
      const fieldDefinition = fieldTypes[prop];
      console.info({ target, prop, receiver });
      //return fieldDefinition()
      return 
    }
    
    return undefined;
  }
})// as EditorField;


export function createFieldConfiguration(): EditorFieldConfiguration
{
  return {
    required: false,
    readonly: false,
    hidden: false,
    label: null,
    hideLabel: false,
    description: null,
    helpText: null,
    classes: null,
    horizontal: false
  } as EditorFieldConfiguration;
}


export class EditorFieldBase
{
  alias: string;
  configuration: EditorFieldConfiguration;
  component?: Component;
  options?: any;

  constructor(alias: string, config?: EditorFieldConfiguration)
  {
    this.alias = alias;
    this.configuration = config || createFieldConfiguration();
  }


  /**
   * Set this field as required
   * @param {function|boolean} [condition] - Optionally only require this field when a condition is fulfilled or reset the required state with true/false
   */
  setRequired(condition: Function | boolean): EditorField
  {
    this.configuration.required = condition;
    return this;
  }

  /**
   * Whether the input next to the headline or below
   * @param {boolean} isHorizontal
   * @returns {EditorField}
   */
  setHorizontal(isHorizontal: boolean): EditorField
  {
    this.configuration.horizontal = !isHorizontal;
    return this;
  }


  /**
   * Set this field to disabled
   */
  setReadonly(condition: Function | boolean): EditorField
  {
    this.configuration.readonly = condition;
    return this;
  }


  /**
   * Conditionally hide this field
   * @param {function} condition - function which returns a boolean and gets passed the current model
   */
  setHidden(condition: Function | boolean): EditorField
  {
    this.configuration.hidden = condition;
    return this;
  }


  /**
   * Set a custom component for a field
   * @param {Component} component - The component to render (can be an async component too)
   * @param {T} [options] = Custom options to pass to this editor
   */
  custom<T>(component: Component, options?: T): EditorField
  {
    this.component = component;
    this.options = options;
    return this;
  }
}