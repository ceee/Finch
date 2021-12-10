declare module 'zero/schemas'
{
  export interface ZeroSchema
  {
    alias: string;
  }

  export interface EditorSchema extends ZeroSchema
  {
    
  }


  export interface ListSchema extends ZeroSchema
  {

  }

  declare type ZeroSchemaProp = ZeroSchema | Promise<ZeroSchema>;
}