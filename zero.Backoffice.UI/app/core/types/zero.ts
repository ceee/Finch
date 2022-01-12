import { Emitter, EventType } from "mitt";
import { Component } from "vue";
import { ZeroSchema } from "zero/schemas";

export interface Zero
{
  version: string;
  /**
   * access the event hub
   **/
  events: Emitter<Record<EventType, unknown>>;
  options: ZeroOptions;
  useZero: () => void;
  useRouter: () => void;
  usePlugins: () => void;

  runtimeVariables: Record<string, any>;

  /**
   * get a defined field type component
   **/
  getFieldTypeComponent(alias: string): Component | undefined;

  /**
   * get a defined list or editor schema
   **/
  getSchema(alias: string): Promise<ZeroSchema | null>;
}

export interface ZeroOptions
{
  paths: ZeroPathOptions;
}

export interface ZeroPathOptions
{
  root: string;
  api: string;
}