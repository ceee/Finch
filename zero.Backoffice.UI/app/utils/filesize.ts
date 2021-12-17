

const BYTE_UNIT = 'B';
const UNITS = ['kB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];

/**
 * Generate human-readable filesize from byte number
 * @returns {string} Readable filesize
 */
export function getFilesize(bytes: number, decimalPlaces: number = 1): string
{
  if (typeof bytes !== 'number')
  {
    return '0 ' + BYTE_UNIT;
  }

  var thresh = 1024;
  if (Math.abs(bytes) < thresh)
  {
    return bytes + ' ' + BYTE_UNIT;
  }
  var u = -1;
  do
  {
    bytes /= thresh;
    ++u;
  } while (Math.abs(bytes) >= thresh && u < UNITS.length - 1);

  return bytes.toFixed(decimalPlaces) + ' ' + UNITS[u];
}