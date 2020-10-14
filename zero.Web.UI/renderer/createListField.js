import { warn } from '@zero/services/debug.js';


/**
 * Creates a new field for list rendering.
 * 
 * @param {string} name This is the name.
 * @param {object} opts These are the options.
 */
let createListField = function (name, opts)
{
  return function ()
  {
    return {
      onRender: () =>
      {
        // render nothing if not defined
      },
      asHTML: response =>
      {
        return {
          __html: true,
          response: response
        }
      }
    };
  };
};


export default createListField;