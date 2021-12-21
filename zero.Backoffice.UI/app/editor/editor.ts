import { Component, ComponentPropsOptions } from "vue";
import { EditorSchema, ZeroEditorField } from "zero/schemas";
import { ZeroEditorCanvas } from "./editor-canvas";

export class ZeroEditor extends ZeroEditorCanvas
{
  resourcePrefix?: string;

  onLabelCreate = (field: ZeroEditorField): string =>
  {
    return field.configuration.label || (this.resourcePrefix ? this.resourcePrefix + field.path : '@nolabel[' + field.path + ']');
  };

  onDescriptionCreate = (field: ZeroEditorField): string | null =>
  {
    return field.configuration.description || (this.resourcePrefix ? this.resourcePrefix + field.path + '_text' : '@nodesc[' + field.path + ']');
  };


  constructor()
  {
    super();
  }
}