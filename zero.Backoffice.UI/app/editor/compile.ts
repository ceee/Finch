import { Component } from "vue";
import { ZeroEditorField, ZeroEditorDisplay } from "zero/schemas";
import { Zero } from "../core";
import { ZeroEditor } from "./editor";
import { createBlueprintConfig } from "./editor-blueprint";
import { ZeroEditorCanvasBase, ZeroEditorTab } from "./editor-canvas";
import { ZeroEditorFieldConfiguration } from "./editor-field";
import { localize } from '../services/localization';


export interface ZeroCompiledEditor
{
  alias: string;
  blueprint: any;
  display: ZeroEditorDisplay;
  tabs: ZeroCompiledEditorTab[];
}


export interface ZeroCompiledEditorTab
{
  alias: string;
  name: string;
  sort: number;
  class: string | null;
  count: (model: any) => number | null;
  disabled: (model: any) => boolean;
  hidden: (model: any) => boolean;
  fieldsets: ZeroCompiledEditorFieldset[];
  component: Component | null;
}


export interface ZeroCompiledEditorFieldset
{
  fields: ZeroCompiledEditorField[];
  hasColumns: boolean;
}


export interface ZeroCompiledEditorField
{
  path: string;
  configuration: ZeroEditorFieldConfiguration;
  fieldType: string;
  options: any | null;
  component: Component;

  optional: (model: any) => boolean;
  readonly: (model: any) => boolean;
  hidden: (model: any) => boolean;
  label: string;
  hideLabel: boolean;
  description: string | null;
  helpText: string | null;
  classes: string | null;
  horizontal: boolean;
  sort: number;
  columns: number;
}


export function compileField(zero: Zero, editor: ZeroEditor, field: ZeroEditorField): ZeroCompiledEditorField | undefined
{
  const component = field.customComponent || zero.getFieldTypeComponent(field.fieldType);

  if (!component)
  {
    return undefined;
  }

  let model = {
    configuration: field.configuration,
    fieldType: field.fieldType,
    options: field.options,
    path: field.path,
    component,

    optional(model: any)
    {
      if (typeof field.configuration.optional === 'boolean')
      {
        return field.configuration.optional;
      }
      if (typeof field.configuration.optional === 'function')
      {
        return field.configuration.optional(model);
      }
      return false;
    },

    readonly(model: any)
    {
      if (typeof field.configuration.readonly === 'boolean')
      {
        return field.configuration.readonly;
      }
      if (typeof field.configuration.readonly === 'function')
      {
        return field.configuration.readonly(model);
      }
      return false;
    },

    hidden(model: any)
    {
      if (typeof field.configuration.hidden === 'boolean')
      {
        return field.configuration.hidden;
      }
      if (typeof field.configuration.hidden === 'function')
      {
        return field.configuration.hidden(model);
      }
      return false;
    },

    label: editor.onLabelCreate(field),  
    description: localize(editor.onDescriptionCreate(field), { hideEmpty: true }),
    hideLabel: field.configuration.hideLabel,
    helpText: field.configuration.helpText,
    classes: field.configuration.classes,
    horizontal: field.configuration.horizontal,
    sort: field.configuration.sort,
    columns: 12

  } as ZeroCompiledEditorField;

  return model;
}


export function compileEditor(zero: Zero, editor: ZeroEditor): ZeroCompiledEditor | null
{
  if (!editor)
  {
    return null;
  }

  let model = {
    alias: editor.alias,
    blueprint: null,
    display: editor.display,
    tabs: []
  } as ZeroCompiledEditor;

  let tabs = [...editor.tabs.sort((a, b) => a.sort - b.sort)];

  // add default tab if necessary
  if (!tabs.length && (editor.fields.length || editor.fieldsets.length))
  {
    let tab = new ZeroEditorTab('content', '@ui.tab_content');
    tab.fieldsets.push(new ZeroEditorCanvasBase());
    tabs.push(tab);
  }

  tabs.forEach((editorTab, tabIndex) =>
  {
    let tab = {
      name: editorTab.name,
      sort: editorTab.sort,
      alias: editorTab.alias,
      class: null,
      count: (model: any) => null,
      disabled: (model: any) => false,
      hidden: (model: any) => false,
      fieldsets: [],
      component: null
    } as ZeroCompiledEditorTab;

    let fieldsets = [];

    // add fieldsets (which are not part of a tab) to the first tab
    if (tabIndex == 0 && editor.fieldsets.length)
    {
      fieldsets.push(...editor.fieldsets);
    }

    // push registered fieldsets
    fieldsets.push(...editorTab.fieldsets.sort((a, b) => a.sort - b.sort));

    // add default fieldset if none are defined
    if (!fieldsets.length)
    {
      const fakeFieldset = new ZeroEditorCanvasBase();
      fakeFieldset.isFake = false;
      fieldsets.push(fakeFieldset);
    }

    fieldsets.forEach((editorFieldset, fieldsetIndex) =>
    {
      let fieldset = {
        sort: editorFieldset.sort,
        fields: [],
        hasColumns: !editorFieldset.isFake
      } as ZeroCompiledEditorFieldset;

      let fields = [];

      // add fields (which are not part of a fieldset) to the first fieldset
      if (fieldsetIndex == fieldsets.length - 1)
      {
        if (editor.fields.length)
        {
          fields.push(...editor.fields);
        }
        if (editorTab.fields.length)
        {
          fields.push(...editorTab.fields);
        }
      }

      // push registered fields
      // @ts-ignore
      fields.push(...editorFieldset.fields.sort((a, b) => a.configuration.sort - b.configuration.sort));

      fields.forEach(editorField =>
      {
        const field = compileField(zero, editor, editorField);

        if (field == null)
        {
          console.error('Could not find registered field type [' + editorField.fieldType + ']');
        }
        else
        {
          fieldset.fields.push(field);
        }
      });


      if (fieldset.fields.length)
      {
        tab.fieldsets.push(fieldset);
      }
    });


    if (tab.fieldsets.length)
    {
      model.tabs.push(tab);
    }
  });


  model.blueprint = createBlueprintConfig(model.alias, model);

  return model;
}