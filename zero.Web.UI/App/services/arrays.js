
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
  }
};