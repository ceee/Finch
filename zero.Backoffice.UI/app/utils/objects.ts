
import { find as _find, extend as _extend } from 'underscore';


export function setObjectValue(model: object, props: string[], value: any)
{
  const clonedProps = [...props];
  const prop = clonedProps.shift();

  if (!model[prop])
  {
    model[prop] = {};
  }
  if (!clonedProps.length)
  {
    if (value && typeof value === 'object' && !Array.isArray(value))
    {
      model[prop] = { ...model[prop], ...value };
    }
    else
    {
      model[prop] = value;
    }
    return;
  }

  setObjectValue(model[prop], clonedProps, value);
};


export function extendObject(source: object, extendWith: object): object
{
  return _extend(source, extendWith);
}