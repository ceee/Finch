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