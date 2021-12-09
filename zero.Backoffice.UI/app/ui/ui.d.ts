
declare module 'zero/ui'
{
  export interface UiStoreState
  {
    sections: UiSection[];
    settingGroups: UiSettingsGroup[];
    iconSets: UiIconSet[];
  }

  export interface UiSection
  {
    alias: string;
    name: string;
    icon: string;
    url: string;
    isExternal: boolean;
    children: UiSection[];
  }

  export interface UiSettingsGroup
  {
    name: string;
    items: UiSettingsItem[];
  }

  export interface UiSettingsItem
  {
    alias: string;
    name: string;
    description: string;
    icon: string;
    url: string;
    isPlugin: boolean;
  }

  export interface UiIconSet
  {
    alias: string;
    name: string;
    prefix: string;
    icons: string[];
  }
}