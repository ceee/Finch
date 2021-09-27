
import MediaApi from 'zero/api/media.js';
import Strings from 'zero/helpers/strings.js';
import Localization from 'zero/helpers/localization.js';
import EmptyImg from '../../assets/empty.png';

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

  _type = null;
  _func = () => { };
  _funcOptions = {};
  _asHtml = false;

  constructor(path, options)
  {
    this.path = path;
    this.options = { ...this.options, ...options };
  }


  get isHtml()
  {
    return this._asHtml;
  }

  get type()
  {
    return this._type;
  }

  get func()
  {
    return this._func;
  }

  get funcOptions()
  {
    return this._funcOptions;
  }


  /**
   * Set another list column as the base for this column (copies properties)
   * @param {ListColumn} column - Base list column
   * @returns {ListColumn}
   */
  setBase(column)
  {
    this.path = column.path;
    this.options = { ...column.options };
    this._asHtml = column.isHtml;
    this._type = column.type;
    this._func = column.func;
    this._funcOptions = column.funcOptions;
    return this;
  }


  /**
   * Render the output by passing the value and the whole entity
   * @param {any} value - The value to process
   * @param {any} model - The model entity
   * @returns {string}
   */
  render(value, model)
  {
    return this._func(value, this._funcOptions, model);
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
    this._type = type || 'custom';
    this._asHtml = asHtml || false;
    this._func = (value, opts, model) =>
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
    this._type = 'text';
    this._funcOptions = { localize: false, tokens: {}, wrap: false, ...options };
    this._func = (value, opts) =>
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
    this._type = 'html';
    this._asHtml = true;
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
    this._type = 'date';
    this._funcOptions = { format: 'short', ...options };
    this._func = (value, opts) => Strings.date(value, opts.format);
    return this;
  }


  /**
   * Output a currency value
   * @returns {ListColumn}
   */
  currency()
  {
    this._type = 'currency';
    this._asHtml = true;
    this._func = (value, opts) =>
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
   * @param {function} [isTrueFunc] - Decide if this boolean is true based on a function with the value as a parameter
   * @returns {ListColumn}
   */
  boolean(isTrueFunc)
  {
    this._type = 'boolean';
    this._asHtml = true;
    this._funcOptions = { isTrueFunc };
    this._func = (value, opts) =>
    {
      let isTrue = typeof opts.isTrueFunc === 'function' ? !!opts.isTrueFunc(value) : !!value;
      return `<svg class="ui-icon ui-table-field-bool" width="16" height="16" stroke-width="${isTrue ? '2.5' : '2'}" data-symbol="${isTrue ? 'fth-check' : 'fth-x'}">
        <use xlink:href="#${isTrue ? 'fth-check' : 'fth-x'}" />
      </svg>`;
    }
    return this;
  }


  /**
   * Output an image by using the value media ID
   * @returns {ListColumn}
   */
  image()
  {
    this._type = 'image';
    this._asHtml = true;
    this._func = (value, opts) => value ? `<img src="${MediaApi.getImageSource(value)}" @error="$event.target.src=${EmptyImg}" class="ui-table-field-image">` : '';
    return this;
  }


  /**
   * Outputs an icon
   * @param {string} [icon] - Fixed icon name
   * @param {string} [size=17] - Size of the icon
   * @returns {ListColumn}
   */
  icon(icon, size)
  {
    size = size || 17;
    this._type = 'icon';
    this._asHtml = true;
    this._func = (value, opts) =>
    {
      let ico = (icon || value).trim();
      let html = `<svg class="ui-icon ui-table-field-image" width="${size}" height="${size}" stroke-width="2" :data-symbol="${ico}">`;
      if (ico.indexOf('flag') !== 0)
      {
        html += `<use xlink:href="#${ico}" />`;
      }
      return html + `</svg>`;
    };    
    return this;
  }


  /**
   * Shortcut for text() with predefined label and class
   * @returns {ListColumn}
   */
  name()
  {
    this.options.label = '@ui.name';
    this.options.class = 'is-name';
    this._type = 'text';
    this._asHtml = true;
    this._func = (value, opts, model) =>
    {
      let html = '<b>' + value + '</b>';

      if (model.isActive === false)
      {
        html = value;
      }

      if (model.blueprint && model.blueprint.id)
      {
        html += ` <svg class="ui-icon" width="15" height="15" stroke-width="2" :data-symbol="fth-cloud" title="Synchronized"><use xlink:href="#fth-cloud" /></svg>`;
      }

      return html;
    };
    return this;
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