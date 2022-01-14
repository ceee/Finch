
declare module 'zero/schemas'
{
  export interface ZeroSchema
  {
    alias: string;
  }

  export interface ZeroSchemaExtension
  {
    alias: string;
    extension: (schema: ZeroSchema) => void;
  }
}