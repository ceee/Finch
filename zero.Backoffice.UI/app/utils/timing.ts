
import { debounce as _debounce } from 'underscore';

/**
 * Proxy to underscore debounce
 */
export function debounce(fn, timespan)
{
  return _debounce(fn, timespan)
}