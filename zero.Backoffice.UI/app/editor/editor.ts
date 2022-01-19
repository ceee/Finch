import { Component } from "vue";
import { ZeroEditorField, ZeroSchema, ZeroEditorDisplay } from "zero/schemas";
import { ZeroEditorCanvas } from "./editor-canvas";

export class ZeroEditor extends ZeroEditorCanvas implements ZeroSchema
{
  alias: string;

  resourcePrefix?: string;

  system: boolean = false;

  display: ZeroEditorDisplay = 'tabs';

  onLabelCreate = (field: ZeroEditorField): string =>
  {
    return field.configuration.label || (this.resourcePrefix ? this.resourcePrefix + field.path : '@nolabel[' + field.path + ']');
  };

  onDescriptionCreate = (field: ZeroEditorField): string | null =>
  {
    return field.configuration.description || (this.resourcePrefix ? this.resourcePrefix + field.path + '_text' : '@nodesc[' + field.path + ']');
  };


  constructor(alias: string)
  {
    super();
    this.alias = alias;
  }
}


export class ZeroModuleEditor extends ZeroEditor
{
  _preview: ZeroModuleEditorPreview | null = null;

  constructor(alias: string)
  {
    super(alias);
  }

  preview(component: Component, buildProps?: (model: any) => any, options?: ZeroModuleEditorPreviewOptions): void
  {
    this._preview = {
      component,
      buildProps: buildProps || (_ => ({})),
      options: options || {}
    };
  }

  getPreview(): ZeroModuleEditorPreview | null
  {
    return this._preview;
  }
}

export interface ZeroModuleEditorPreviewOptions
{
  canEdit?: boolean;
  hideLabel?: boolean;
}

export interface ZeroModuleEditorPreview
{
  component: Component;
  buildProps?: (model: any) => any;
  options?: ZeroModuleEditorPreviewOptions;
}