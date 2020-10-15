
const arrayMoveMutate = (array, from, to) =>
{
  const startIndex = to < 0 ? array.length + to : to;
  const item = array.splice(from, 1)[0];
  array.splice(startIndex, 0, item);
};

export default {

  /// <summary>
  /// Move an item in an array
  /// </summary>
  move(array, from, to)
  {
    array = array.slice();
    arrayMoveMutate(array, from, to);
    return array;
  },

  /// <summary>
  /// Replaces an item in an array
  /// </summary>
  replace(array, origin, target)
  {
    const index = array.indexOf(origin);

    if (index < 0)
    {
      return index;
    }

    array.splice(index, 1);
    array.splice(index, 0, target);
    return index;
  },


  /// <summary>
  /// Removes an item from an array
  /// </summary>
  remove(array, value)
  {
    const index = array.indexOf(value);

    if (index < 0)
    {
      return;
    }

    array.splice(index, 1);
    return index;
  }
};