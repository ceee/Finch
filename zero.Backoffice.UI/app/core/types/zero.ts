import { ZeroSchema } from "zero/schemas";

export interface Zero
{
  version: string;
  options: ZeroOptions;
  useZero: () => void;
  useRouter: () => void;
  usePlugins: () => void;

  runtimeVariables: Record<string, any>;

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