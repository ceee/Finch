
import * as dayjs from 'dayjs';

const DATE_FORMAT = 'DD.MM.YY';
const TIME_FORMAT = 'HH:mm';
const DATETIME_FORMAT = DATE_FORMAT + ' ' + TIME_FORMAT;

/**
 * Formats a date string
 * @returns {string} Formatted date
 */
export function formatDate(value: string, format?: string): string
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
  else if (format === 'time')
  {
    format = TIME_FORMAT;
  }

  return dayjs(value).format(format);
}