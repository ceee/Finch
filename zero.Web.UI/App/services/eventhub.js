
let handlers = new Map();

/*
 * Adds a new handler for an event of a specfic type
 */
const on = (type, handler) =>
{
  const typeHandlers = handlers.get(type);
  const added = typeHandlers && typeHandlers.push(handler);

  if (!added)
  {
    handlers.set(type, [handler]);
  }
};


/*
 * Removes a specifc event handler 
 */
const off = (type, handler) =>
{
  const typeHandlers = handlers.get(type);

  if (typeHandlers)
  {
    typeHandlers.splice(typeHandlers.indexOf(handler) >>> 0, 1);
  }
};


/*
 * Adds a new handler for an event of a specfic type
 */
const emit = (type, event) =>
{
  (handlers.get(type) || []).slice().map(handler => handler(event));
  (handlers.get('*') || []).slice().map(handler => handler(type, event));
};


export default {
  on,
  off,
  emit
};