
import MediaApi from 'zero/resources/media.js';
import Strings from 'zero/helpers/strings.js';
import Localization from 'zero/helpers/localization.js';

class ListColumn
{
  path = null;
  options = {
    label: null,
    hideLabel: false,
    width: null,
    canSort: true,
    class: ''
  };

  #type = null;
  #func = () => { };
  #funcOptions = {};
  #asHtml = false;

  constructor(path, options)
  {
    this.path = path;
    this.options = { ...this.options, ...options };
  }


  get isHtml()
  {
    return this.#asHtml;
  }

  get type()
  {
    return this.#type;
  }


  /**
   * Render the output by passing the value and the whole entity
   * @param {any} value - The value to process
   * @param {any} model - The model entity
   * @returns {string}
   */
  render(value, model)
  {
    return this.#func(value, this.#funcOptions, model);
  }


  /**
   * Render a custom component
   * @param {Function} renderFunc - The function which renders the output (parameter is value)
   * @param {boolean} [asHtml] - Render as a plain string or as HTML
   * @param {string} [type] - Pass the type name which will be added in the HTML as [field-type="type"]
   * @returns {ListColumn}
   */
  custom(renderFunc, asHtml, type)
  {
    this.#type = type || 'custom';
    this.#asHtml = asHtml || false;
    this.#func = (value, opts, model) =>
    {
      return renderFunc(value, model, opts);
    };
    return this;
  }


  /**
   * Render a text output
   * @param {object} [options] - Custom options
   * @param {boolean} [options.localize] - Localize the value in case it is a translation
   * @param {string} [options.tokens] - For localized values you can pass in replacement tokens here
   * @param {boolean} [options.wrap] - Allow text wrapping / multi-line
   * @returns {ListColumn}
   */
  text(options)
  {
    this.#type = 'text';
    this.#funcOptions = { localize: false, tokens: {}, wrap: false, ...options };
    this.#func = (value, opts) =>
    {
      let result = value;
      if (opts.localize)
      {
        result = Localization.localize(value, opts.tokens || {});
      }
      return result;
    }
    return this;
  }


  /**
   * Render a text (as HTML) output
   * @param {object} [options] - Custom options
   * @param {boolean} [options.localize] - Localize the value in case it is a translation
   * @param {string} [options.tokens] - For localized values you can pass in replacement tokens here
   * @param {boolean} [options.wrap] - Allow text wrapping / multi-line
   * @returns {ListColumn}
   */
  html(options)
  {
    this.#type = 'html';
    this.#asHtml = true;
    return this.text(options);
  }


  /**
   * Output a date
   * @param {object} [options] - Custom options
   * @param {string} [options.format] - Date format for the output (can also be "long" for a full date and "short" for full date-time)
   * @returns {ListColumn}
   */
  date(options)
  {
    this.#type = 'date';
    this.#funcOptions = { format: 'short', ...options };
    this.#func = (value, opts) => Strings.date(value, opts.format);
    return this;
  }


  /**
   * Output a currency value
   * @returns {ListColumn}
   */
  currency()
  {
    this.#type = 'currency';
    this.#asHtml = true;
    this.#func = (value, opts) =>
    {
      let price = isNaN(value) ? 0 : value;
      let hasDecimals = ~~price !== price;

      price = hasDecimals ? (price / 1).toFixed(2) : ~~price;

      return price.toString().replace(/\B(?=(\d{3})+(?!\d))/g, "&nbsp;") + "&nbsp;&euro;";
    };
    return this;
  }


  /**
   * Output a boolean value by displaying a checkmark
   * @param {object} [options] - Custom options
   * @param {string} [options.colored] - Green color for check and red color for cross
   * @returns {ListColumn}
   */
  boolean(options)
  {
    this.#type = 'boolean';
    this.#asHtml = true;
    this.#funcOptions = { colored: false, ...options };
    this.#func = (value, opts) => '<span class="ui-table-field-bool' + (!!value ? ' is-checked' : '') + (opts.colored ? ' is-colored' : '') + '"></span>';
    return this;
  }


  /**
   * Output an image by using the value media ID
   * @returns {ListColumn}
   */
  image()
  {
    this.#type = 'image';
    this.#asHtml = true;
    this.#func = (value, opts) => value ? `<img src="${MediaApi.getImageSource(value)}" class="ui-table-field-image">` : '';
    return this;
  }


  /**
   * Shortcut for text() with predefined label and class
   * @returns {ListColumn}
   */
  name()
  {
    this.options.label = '@ui.name';
    this.options.class = 'is-bold';
    return this.text();
  }


  /**
   * Shortcut for boolean() with predefined label and width
   * @returns {ListColumn}
   */
  active()
  {
    this.options.label = '@ui.active';
    this.options.width = 150;
    return this.boolean();
  }


  /**
   * Shortcut for date() with predefined label and width
   * @returns {ListColumn}
   */
  created()
  {
    this.options.label = '@ui.createdDate';
    this.options.width = 150;
    return this.date();
  }
}


export default ListColumn;