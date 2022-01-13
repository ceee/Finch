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
  preview: ZeroModuleEditorPreview;

  constructor(alias: string)
  {
    super(alias);

    this.preview = {
      hideLabel: false
    } as ZeroModuleEditorPreview;
  }
}

export interface ZeroModuleEditorPreview
{
  hideLabel?: boolean;
}