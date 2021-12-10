import { ZeroSchema } from "zero/schemas";

export interface Zero
{
  version: string;
  useZero: () => void;
  useRouter: () => void;
  usePlugins: () => void;

  /**
   * get a defined list or editor schema
   **/
  getSchema(alias: string): Promise<ZeroSchema | null>;
}