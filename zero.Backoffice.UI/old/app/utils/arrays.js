
const arrayMoveMutate = (array, from, to) =>
{
  const startIndex = to < 0 ? array.length + to : to;
  const item = array.splice(from, 1)[0];
  array.splice(startIndex, 0, item);
};

/**
 * Move an item in an array
 */
export function move(array, from, to)
{
  array = array.slice();
  arrayMoveMutate(array, from, to);
  return array;
}

/**
 * Replaces an item in an array
 */
export function replace(array, origin, target)
{
  const index = array.indexOf(origin);

  if (index < 0)
  {
    return index;
  }

  array.splice(index, 1);
  array.splice(index, 0, target);
  return index;
};


/**
 * Removes an item from an array
 */
export function remove(array, value)
{
  const index = array.indexOf(value);

  if (index < 0)
  {
    return;
  }

  array.splice(index, 1);
  return index;
}