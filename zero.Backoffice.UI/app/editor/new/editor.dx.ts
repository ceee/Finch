import { Component } from "vue";

declare module 'zero/editor'
{
  export interface EditorFieldConfiguration
  {
    /**
     * Whether this field is required (additional validation is done on the server)
     */
    required?: Function | boolean;
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
    horizontal?: boolean
  }


  export interface EditorFieldDefinition<T>
  {
    component: Component;
    options?: T;
  //  return this._setComponent(() => import('./fields/text.vue'), { maxLength, placeholder });
  //this._component = component;
  //this._componentOptions = options || {};
  }


  export interface EditorField
  {
    /**
     * Set this field as required
     * @param {function|boolean} [condition] - Optionally only require this field when a condition is fulfilled or reset the required state with true/false
     */
    setRequired(condition: Function | boolean): EditorField;

    /**
     * Whether the input next to the headline or below
     * @param {boolean} isHorizontal
     * @returns {EditorField}
     */
    setHorizontal(isHorizontal: boolean): EditorField;

    /**
     * Set this field to disabled
     */
    setReadonly(condition: Function | boolean): EditorField;

    /**
     * Conditionally hide this field
     * @param {function} condition - function which returns a boolean and gets passed the current model
     */
    setHidden(condition: Function | boolean): EditorField;

    /**
     * Set a custom component for a field
     * @param {Component} component - The component to render (can be an async component too)
     * @param {T} [options] = Custom options to pass to this editor
     */
    custom<T>(component: Component, options?: T): EditorField;
  }
}