import { ZeroEditorField } from "zero/schemas";
import { ZeroEditorFieldImpl, ZeroEditorFieldConfiguration, createFieldProxy } from "./editor-field";


export class ZeroEditorCanvasBase
{
  fields: ZeroEditorField[] = [];

  field(path: string, config?: ZeroEditorFieldConfiguration): ZeroEditorField
  {
    const field = new ZeroEditorFieldImpl(path, config);
    const proxy = createFieldProxy(field) as ZeroEditorField;
    this.fields.push(proxy);
    return proxy;
  }
}


export class ZeroEditorCanvasBaseWithFieldset extends ZeroEditorCanvasBase
{
  fieldset(): ZeroEditorCanvasBase
  {
    return new ZeroEditorCanvasBase();
  }
}


export class ZeroEditorCanvas extends ZeroEditorCanvasBaseWithFieldset
{
  tab(alias: string, name: string): ZeroEditorTab
  {
    return new ZeroEditorTab(alias, name);
  }
}


export class ZeroEditorTab extends ZeroEditorCanvasBaseWithFieldset
{
  alias: string;
  name: string;

  constructor(alias: string, name: string)
  {
    super();
    this.alias = alias;
    this.name = name;
  }
}