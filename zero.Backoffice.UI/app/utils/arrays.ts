
import { groupBy as _groupBy, difference as _difference } from 'underscore';

export function arrayMoveRef(array: any[], from: number, to: number): any[]
{
  const startIndex = to < 0 ? array.length + to : to;
  const item = array.splice(from, 1)[0];
  array.splice(startIndex, 0, item);
};

/**
 * Move an item in an array
 * @returns {any[]} Modified array
 */
export function arrayMove(array: any[], from: number, to: number): any[]
{
  array = array.slice();
  arrayMoveRef(array, from, to);
  return array;
}

/**
 * Replaces an item in an array
 * @returns {number} Index where the item was replaced
 */
export function arrayReplace(array: any[], origin: any, target: any): number
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
 * @returns {number} Index where the item has been removed
 */
export function arrayRemove(array: any[], value: any): number
{
  const index = array.indexOf(value);

  if (index < 0)
  {
    return;
  }

  array.splice(index, 1);
  return index;
}


/**
 * Groups an array by a property key
 * @returns {any} Array groups
 */
export function arrayGroupBy(array: any[], key: string): any
{
  return _groupBy(array, key);
}


/**
 * Converts an object path to an array of path parts
 * @returns {string[]} Object paths as array
 */
export function selectorToArray(selector: string): string[]
{
  if (!selector)
  {
    return [];
  }
  selector = selector.replace(/\[(\w+)\]/g, '.$1');
  selector = selector.replace(/^\./, '');
  return selector.split('.');
};


/**
 * Removes duplicate items from an array
 * @returns {any[]} Unique array
 */
export function arrayUnique(array: any[]): any[]
{
  return [...new Set(array)];
}


/**
 * Returns the different items from two arrays
 * @returns {any[]} Difference
 */
export function arrayDifference(array: any[], other: any[]): any[]
{
  return _difference(array, other);
}

export function arrayContainsAll(array: any[], other: any[]): boolean
{
  return other.every(x => array.indexOf(x) > -1);
}
