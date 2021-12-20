
class EditorField
{
  path = null;

  options = {
    label: null,
    hideLabel: false,
    description: null,
    helpText: null,
    condition: null,
    disabled: false,
    tab: null,
    allTabs: false,
    vertical: true,
    coreDatabase: false,
    fieldset: null,
    fieldsetColumns: null,
    class: '',
    onChange: null
  };

  _preview = {
    icon: 'fth-filter',
    preview: x => x,
    hasValue: x => !!x
  };

  _component = null;
  _componentOptions = {};
  _required = false;
  _isReadOnly = false;

  constructor(path, options)
  {
    this.path = path;
    this.options = { ...this.options, ...options };
  }


  get component()
  {
    return this._component;
  }

  get componentOptions()
  {
    return this._componentOptions;
  }

  get isRequired()
  {
    return this._required;
  }

  get previewOptions()
  {
    return this._preview;
  }


  /**
   * Set another editor field as the base for this editor (copies properties)
   * @param {EditorField} field - Base editor field
   * @returns {EditorField}
   */
  setBase(field)
  {
    this.path = field.path;
    this.options = { ...field.options };
    this._preview = { ...field.previewOptions };
    this._component = field.component;
    this._componentOptions = field.componentOptions;
    this._required = field.isRequired;
    return this;
  }


  /**
   * 
   */
  _setComponent(component, options?)
  {
    this._component = component;
    this._componentOptions = options || {};
    return this;
  }


  /**
   * Sets the column count for this field, only available within a an editor fieldset
   * @param {number} columnCount - Column count between 1 and 12
   * @returns {EditorField}
   */
  cols(columnCount)
  {
    this.options.fieldsetColumns = columnCount < 1 ? 1 : (columnCount > 12 ? 12 : columnCount);
    return this;
  }


  /**
   * Whether the input is below the headline or next to it
   * @param {boolean} isVertical
   * @returns {EditorField}
   */
  vertical(isVertical)
  {
    this.options.vertical = isVertical;
    return this;
  }


  /**
   * Set this field to disabled
   */
  disabled()
  {
    this.options.disabled = true;
    return this;
  }


  /**
   * Set this field as required
   * @param {function|boolean} [condition] - Optionally only require this field when a condition is fulfilled or reset the required state with true/false
   */
  required(condition)
  {
    if (typeof condition === 'function')
    {
      this._required = condition;
    }
    else if (typeof condition === 'boolean')
    {
      this._required = condition;
    }
    else
    {
      this._required = true;
    }
    return this;
  }


  /**
   * Conditionally render this field (this is an alternative method to the field options 'condition')
   * @param {function} condition - function which returns a boolean and gets passed the current model
   */
  when(condition)
  {
    this.options.condition = condition;
    return this;
  }


  /**
   * The expression argument is called when the value of the field changes
   * @param {function} expression - function which is called
   */
  onChange(expression)
  {
    this.options.onChange = expression;
    return this;
  }


  /**
   * Create a preview for this field
   * This is only used in list filters, ...
   * @param {object} options - Custom options
   * @param {string} options.icon - Custom icon
   * @param {string|function} options.preview - Render the preview when this filter has been filled out
   * @param {boolean} options.hasValue - Determine if the filter has a value or not
   * @returns {EditorField}
   */
  preview(options)
  {
    this._preview = { ...this.preview, ...options };
    return this;
  }











  /**
   * Render a custom component
   * @param {object} component - The custom vue component
   * @param {object} [options] - Custom options
   * @returns {EditorField}
   */
  custom(component, options)
  {
    return this._setComponent(component, { ...options });
  }


  /**
   * Render a text input field
   * @param {number} [maxLength] - Maximum length of the input
   * @param {string|function} [placeholder] - Placeholder text (can be a translation) or function
   * @returns {EditorField}
   */
  text(maxLength, placeholder)
  {
    return this._setComponent(() => import('./fields/text.vue'), { maxLength, placeholder });
  }


  /**
   * Render a password input field
   * @param {number} [maxLength] - Maximum length of the input
   * @param {string|function} [placeholder] - Placeholder text (can be a translation) or function
   * @returns {EditorField}
   */
  password(maxLength, placeholder)
  {
    return this._setComponent(() => import('./fields/password.vue'), { maxLength, placeholder });
  }


  /**
   * Render a password hash field
   * @param {number} [maxLength] - Maximum length of the password
   * @returns {EditorField}
   */
  passwordHash(maxLength)
  {
    return this._setComponent(() => import('./fields/password-hash.vue'), { maxLength });
  }


  /**
   * Render a currency input field
   * @param {string|function} [placeholder] - Placeholder text (can be a translation) or function
   * @returns {EditorField}
   */
  currency(placeholder)
  {
    return this._setComponent(() => import('./fields/currency.vue'), { placeholder });
  }


  /**
   * Render a number input field
   * @param {number} [maxLength] - Maximum length of the input
   * @param {string|function} [placeholder] - Placeholder text (can be a translation) or function
   * @returns {EditorField}
   */
  number(maxLength, placeholder)
  {
    return this._setComponent(() => import('./fields/number.vue'), { maxLength, placeholder });
  }


  /**
   * Render a rich-text editor field
   * @param {object} [options] - Custom options
   * @param {number} [options.maxLength=null] - Maximum characters
   * @param {string} [options.placeholder=null] - Placeholder text (can be a translation) or function
   * @param {function} [options.setup=value] - Called on RTE setup
   * @returns {EditorField}
   */
  rte(options)
  {
    return this._setComponent(() => import('./fields/rte.vue'), { ...options });
  }


  /**
   * @typedef {object} EditorSelectItem
   * @param {object} key - Key/Id of the item
   * @param {string} value - Label/Value of the item (can be a translation)
   */

  /**
   * Render a select dropdown with the specified items
   * @param {EditorSelectItem[]|function} items - Set items to pick from
   * @param {object} [options] - Custom options
   * @param {number} [options.emptyOption=false] - Adds an empty option so the field can be blank
   * @returns {EditorField}
   */
  select(items, options)
  {
    return this._setComponent(() => import('./fields/select.vue'), { items, ...options });
  }


  /**
   * Render a text area
   * @param {number} [maxLength] - Maximum length of the input
   * @returns {EditorField}
   */
  textarea(maxLength)
  {
    return this._setComponent(() => import('./fields/textarea.vue'), { maxLength });
  }


  /**
   * Render a toggle
   * @param {boolean} [negative] - Toggle with a negative color / red background
   * @param {object} [options] - Custom options
   * @param {number} [options.negative=false] - Display the toggle in red when set to TRUE
   * @param {number} [options.onContent=null] - Text next to toggle when it is set to TRUE
   * @param {number} [options.offContent=null] - Text next to toggle when it is set to FALSE
   * @returns {EditorField}
   */
  toggle(options)
  {
    this.options.vertical = false;
    return this._setComponent(() => import('./fields/toggle.vue'), { ...options });
  }


  /**
   * Renders the field value
   * @param {function} [render] - Render the output based on the given function
   * @returns {EditorField}
   */
  output(render)
  {
    this._isReadOnly = true;
    return this._setComponent(() => import('./fields/output.vue'), { render });
  }


  /**
   * Renders an input which generates an alias for a given name or an alternative custom alias
   * @param {string} [namePath] - Optional path to the name value which is used to auto-generate the alias
   * @returns {EditorField}
   */
  alias(namePath)
  {
    return this._setComponent(() => import('./fields/alias.vue'), { namePath });
  }


  /**
   * Renders an input which generates an alias for a given name or an alternative custom alias
   * @param {EditorSelectItem[]|function} items - Set items to choose from, either via an array or a promise which returns such array
   * @param {object} [options] - Custom options
   * @param {number} [options.limit=100] - Maximum items to be checked
   * @param {boolean} [options.reverse=false] - Reverse the checklist behaviour, so all items are checked by default and unchecking them adds them to the result list
   * @param {string} [options.labelKey=value] - Object key to get the label
   * @param {string} [options.idKey=key] - Object key to get the id
   * @returns {EditorField}
   */
  checkList(items, options)
  {
    return this._setComponent(() => import('./fields/checklist.vue'), { items, ...options });
  }


  /**
   * Renders a HEX color picker
   * @returns {EditorField}
   */
  colorPicker()
  {
    return this._setComponent(() => import('./fields/colorpicker.vue'));
  }


  /**
   * Renders a country picker
   * @param {number} [limit=1] - Maximum items to be selected
   * @returns {EditorField}
   */
  countryPicker(limit)
  {
    return this._setComponent(() => import('./fields/countrypicker.vue'), { limit });
  }


  /**
   * Renders a space picker
   * @param {number} [limit=1] - Maximum items to be selected
   * @returns {EditorField}
   */
  spacePicker(limit)
  {
    return this._setComponent(() => import('./fields/spacepicker.vue'), { limit });
  }


  /**
   * Renders a culture picker
   * @returns {EditorField}
   */
  culturePicker()
  {
    return this._setComponent(() => import('./fields/culturepicker.vue'));
  }


  /**
   * Renders a mail template picker
   * @param {number} [limit=1] - Maximum items to be selected
   * @returns {EditorField}
   */
  mailTemplatePicker(limit)
  {
    return this._setComponent(() => import('./fields/mailtemplatepicker.vue'), { limit });
  }


  /**
   * Renders a date picker
   * @param {object} [options] - Custom options
   * @param {string} [options.format] - Format the date output
   * @param {boolean} [options.time=false] - Allow time input 
   * @param {string|Date} [options.maxDate] - Maximum selectable date
   * @param {string|Date} [options.minDate] - Minimum selectable date
   * @param {string} [options.amPm] - Render time as AM/PM
   * @returns {EditorField}
   */
  datePicker(options)
  {
    return this._setComponent(() => import('./fields/datepicker.vue'), { ...options });
  }


  /**
   * Renders a date range picker
   * @param {object} [options] - Custom options
   * @param {string} [options.format] - Format the date output
   * @param {boolean} [options.time=false] - Allow time input 
   * @param {string|Date} [options.maxDate] - Maximum selectable date
   * @param {string|Date} [options.minDate] - Minimum selectable date
   * @param {string} [options.fromLabel] - Label next to the "from" date input
   * @param {string} [options.toLabel] - Label next to the "to" date input
   * @param {string} [options.amPm] - Render time as AM/PM
   * @param {string} [options.inline] - Don't render the range picker on an overlay
   * @returns {EditorField}
   */
  dateRangePicker(options)
  {
    return this._setComponent(() => import('./fields/daterangepicker.vue'), { ...options });
  }


  /**
   * Pick an icon from the specified icon collection
   * @param {string} [iconSetAlias] - Custom icon set alias (defined in ZeroOptions.Icons)
   * @returns {EditorField}
   */
  iconPicker(iconSetAlias)
  {
    return this._setComponent(() => import('./fields/iconPicker.vue'), { set: iconSetAlias });
  }


  /**
   * Renders a page picker
   * @param {object} [options] - Custom options
   * @param {number} [options.limit=1] - Limit of selection
   * @returns {EditorField}
   */
  pagePicker(options)
  {
    return this._setComponent(() => import('./fields/pagepicker.vue'), { ...options });
  }


  /**
   * Render a link picker
   * @param {object} [options] - Custom options
   * @param {number} [options.limit=1] - Limit of selection
   * @param {boolean} [options.title=true] - Allow input of custom link title
   * @param {boolean} [options.target=true] - Allow selection of the link target
   * @param {boolean} [options.label=false] - Allow input of a custom label for button generation
   * @param {boolean} [options.suffix=false] - Allow input of custom link URL suffix (query or hash)
   * @param {string[]} [options.areas] - Limit link areas to the specified values (built-in are zero.pages, zero.media and zero.url)
   * @returns {EditorField}
   */
  linkPicker(options)
  {
    return this._setComponent(() => import('./fields/linkpicker.vue'), { ...options });
  }


  /**
   * Create a list of strings
   * @param {number} [limit=10] - Limit the inputs
   * @param {number} [maxItemLength=200] - Maximum length for an item input
   * @param {string} [addLabel] - Label for the add button
   * @returns {EditorField}
   */
  inputList(limit, maxItemLength, addLabel)
  {
    return this._setComponent(() => import('./fields/inputlist.vue'), { limit, maxItemLength, addLabel });
  }


  /**
   * Append tags to an entity
   * @param {number} [limit=10] - Limit the tags
   * @param {number} [maxItemLength=200] - Maximum length for a tag
   * @returns {EditorField}
   */
  tags(limit, maxItemLength)
  {
    return this._setComponent(() => import('./fields/tags.vue'), { limit, maxItemLength });
  }


  /**
   * Pick a language
   * @returns {EditorField}
   */
  languagePicker()
  {
    return this._setComponent(() => import('./fields/language.vue'));
  }


  /**
   * Display a module renderer which allows you to select from defined modules
   * @param {string[]} [tags] - Only allow selection of modules which match defined tags
   * @returns {EditorField}
   */
  modules(tags)
  {
    return this._setComponent(() => import('./fields/modules.vue'), { tags });
  }


  /**
   * Display a module renderer which allows you to select from defined modules
   * @param {Editor} [editor] - Use the specified editor for each item
   * @param {object} [options] - Custom options
   * @param {number} [options.limit=100] - Limit the creation of items
   * @param {string} [options.title] - Headline in the editor overlay
   * @param {string} [options.addLabel] - Label for the add button
   * @param {function} [options.itemLabel] - Function which generates the label for the current item
   * @param {function} [options.itemDescription] - Function which generates the description for the current item
   * @param {string|function} [options.itemIcon] - Static icon or function which generates the icon for the current item
   * @param {object} [options.template] - Template which is used when adding an item
   * @param {object} [options.width=820] - Width of the overlay panel
   * @returns {EditorField}
   */
  nested(editor, options)
  {
    return this._setComponent(() => import('./fields/nested.vue'), { editor, ...options });
  }


  /**
   * Render a select as a button group
   * @param {EditorSelectItem[]} items - Set items to choose from
   * @returns {EditorField}
   */
  state(items)
  {
    return this._setComponent(() => import('./fields/state.vue'), { items });
  }


  /**
   * Render a media upload + picker
   * @param {object} [options] - Custom options
   * @param {number} [options.limit=1] - Limit the media select count
   * @param {boolean} [options.disallowSelect=false] - Disallow the selection (only upload) of media items
   * @param {boolean} [options.disallowUpload=false] - Disallow upload (only selection) of media items
   * @param {string[]} [options.fileExtensions] - Allow upload + selection only for the specified file extensions
   * @param {number} [options.maxFileSize=10] - Maximum allowed file size for uploads in Mibibytes
   * @returns {EditorField}
   */
  media(options)
  {
    return this._setComponent(() => import('./fields/media.vue'), { ...options });
  }


  /**
   * Render a media upload + picker
   * @param {object} [options] - Custom options
   * @param {number} [options.limit=1] - Limit the media select count
   * @param {boolean} [options.disallowSelect=false] - Disallow the selection (only upload) of media items
   * @param {boolean} [options.disallowUpload=false] - Disallow upload (only selection) of media items
   * @param {string[]} [options.fileExtensions] - Allow upload + selection only for the specified file extensions
   * @param {number} [options.maxFileSize=10] - Maximum allowed file size for uploads in Mibibytes
   * @returns {EditorField}
   */
  image(options)
  {
    return this._setComponent(() => import('./fields/media.vue'), { ...options, fileExtensions: ['.jpg', '.jpeg', '.png', '.webp', '.svg'] });
  }


  /**
   * Render a video (YouTube/vimeo) picker
   * @param {number} [limit=1] - Limit the videos
   * @returns {EditorField}
   */
  video(limit)
  {
    return this._setComponent(() => import('./fields/video.vue'), { limit });
  }
}


export default EditorField;