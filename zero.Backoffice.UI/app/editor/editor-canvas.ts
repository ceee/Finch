import { ZeroEditorField } from "zero/schemas";
import { ZeroEditorFieldImpl, ZeroEditorFieldConfiguration, createFieldProxy } from "./editor-field";


export class ZeroEditorCanvasBase
{
  fields: ZeroEditorField[] = [];
  sort: number = 0;
  isFake: boolean = false;

  field(path: string, config?: ZeroEditorFieldConfiguration): ZeroEditorField
  {
    const field = new ZeroEditorFieldImpl(path, config);

    if (!config || typeof config.sort === 'undefined')
    {
      field.configuration.sort = (this.fields.length + 1) * 10;
    }

    const proxy = createFieldProxy(field) as ZeroEditorField;
    this.fields.push(proxy);
    return proxy;
  }
}


interface OnFieldsetCreate
{
  (set: ZeroEditorCanvasBase): void;
}


export class ZeroEditorCanvasBaseWithFieldset extends ZeroEditorCanvasBase
{
  fieldsets: ZeroEditorCanvasBase[] = [];

  fieldset(onCreate: OnFieldsetCreate): void
  {
    const fieldset = new ZeroEditorCanvasBase();
    fieldset.sort = (this.fieldsets.length + 1) * 10;
    this.fieldsets.push(fieldset);
    onCreate(fieldset);
  }
}


export class ZeroEditorCanvas extends ZeroEditorCanvasBaseWithFieldset
{
  tabs: ZeroEditorTab[] = [];

  tab(alias: string, name: string): ZeroEditorTab
  {
    const tab = new ZeroEditorTab(alias, name);
    tab.sort = (this.tabs.length + 1) * 10;
    this.tabs.push(tab);
    return tab;
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