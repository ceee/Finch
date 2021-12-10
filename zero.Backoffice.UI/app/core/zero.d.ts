import { ZeroSchema } from "zero/schemas";
import { Zero } from "./types/zero";

declare module '@vue/runtime-core'
{
  export interface ComponentCustomProperties
  {
    /**
      * zero instance
      */
    zero: Zero
  }
}

declare type Lazy<T> = () => Promise<T>;
declare type ZeroSchemaProp = ZeroSchema | Lazy<ZeroSchema>;