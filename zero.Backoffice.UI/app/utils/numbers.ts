
/**
 * Generates a new near-random ID
 * @returns {string} ID
 */
export function generateId(length?: number): string
{
  var guid = ([1e7] + -1e3 + -4e3 + -8e3 + -1e11).replace(/[018]/g, c =>
    (c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> c / 4).toString(16)
  );

  if (length > 0)
  {
    return guid.replace(/-/g, '').substring(0, length);
  }

  return guid;
}


/**
 * Generates a currency string from a number
 * @returns {string} Formatted currency 
 */
export function toCurrency(value: number, decimals: number = 2, hideSymbol: boolean = false, noEncode: boolean = false): string
{
  if (isNaN(value))
  {
    value = 0;
  }

  var fixedDecimals = typeof decimals !== 'undefined';
  decimals = !fixedDecimals ? 2 : decimals;
  var hasDecimals = ~~value !== value;
  var val = (hasDecimals || fixedDecimals) ? (value / 1).toFixed(decimals) : ~~value;

  if (val === "-0." + "0".repeat(decimals))
  {
    val = "0." + "0".repeat(decimals);
  }

  return val.toString().replace(/\B(?=(\d{3})+(?!\d))/g, noEncode ? " " : "&nbsp;") + (hideSymbol === true ? "" : (noEncode ? " €" : "&nbsp;&euro;"));
  // TODO we have dynamic currencies, not fixed to €
};


/**
 * Rounds a number up
 * @returns {number} Number
 */
export function roundUp(value: number, decimals: number = 2)
{
  decimals = Math.pow(10, decimals);
  return Math.ceil(value * decimals) / decimals;
}