
declare module 'zero/ui'
{
  export interface UiStoreState
  {
    preferences: UiPreferences;

    sections: UiSection[];
    settingGroups: UiSettingsGroup[];
    iconSets: UiIconSet[];
    flavors: Record<string, UiFlavorProvider>;
    blueprints: string[];
  }

  export interface UiPreferences
  {
    theme: 'default' | 'dark' | 'light';
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

  export interface UiFlavorProvider
  {
    canUseWithoutFlavors: boolean;
    defaultFlavor?: string;
    flavors: UiFlavorConfig[];
  }

  export interface UiFlavorConfig
  {
    alias?: string;
    name: string;
    description: string;
    icon: string;
  }
}