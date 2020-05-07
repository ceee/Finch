import dayjs from 'dayjs';

const BYTE_UNIT = 'B';
const UNITS = ['kB', 'MB', 'GB', 'TB', 'PB', 'EB', 'ZB', 'YB'];
const DATETIME_FORMAT = 'DD.MM.YY HH:mm';
const DATE_FORMAT = 'DD.MM.YY';

export default {
  /// <summary>
  /// Generate a GUID
  /// </summary>
  guid()
  {
    return ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
      (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
    );
  },

  /// <summary>
  /// Generate human-readable filesize from byte number
  /// </summary>
  filesize(bytes)
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

    return bytes.toFixed(1) + ' ' + UNITS[u];
  },


  date(value, format)
  {
    if (!value)
    {
      return null;
    }

    format = format || DATE_FORMAT;

    if (format === 'long')
    {
      format = DATETIME_FORMAT;
    }
    else if (format === 'short' || format === 'default')
    {
      format = DATE_FORMAT;
    }

    return dayjs(value).format(format);
  }
};