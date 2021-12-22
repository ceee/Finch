import { ZeroEditorField } from "zero/schemas";
import { ZeroEditorCanvas } from "./editor-canvas";

export class ZeroEditor extends ZeroEditorCanvas
{
  alias: string;

  resourcePrefix?: string;

  system: boolean = false;

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