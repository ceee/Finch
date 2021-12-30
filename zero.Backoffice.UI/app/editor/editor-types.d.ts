import { Component } from "vue";
import { ZeroEditorFieldConfiguration } from "./editor-field";

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


  export type ZeroEditorDisplay = 'tabs' | 'boxes';


  export interface ZeroEditorField
  {
    /**
     * Model path
     */
    path: string;

    /**
     * Field configuration
     */
    configuration: ZeroEditorFieldConfiguration;

    /**
     * Type of the field which has to be registered in the zero runtime
     */
    fieldType: string | symbol;

    /**
     * Custom options which are passed to the component
     */
    options?: any;

    /**
     * Set a custom render component for this field
     */
    customComponent?: Component;

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
    component(component: Component, options?: any): ZeroEditorField;
  }
}