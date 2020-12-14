
import Text from '../editor/fields/text.vue';
import Currency from '../editor/fields/currency.vue';
import Number from '../editor/fields/number.vue';
import Rte from '../editor/fields/rte.vue';
import Select from '../editor/fields/select.vue';
import Textarea from '../editor/fields/textarea.vue';
import Toggle from '../editor/fields/toggle.vue';
import Alias from '../editor/fields/alias.vue';
import Output from '../editor/fields/output.vue';
import Checklist from '../editor/fields/checklist.vue';
import ColorPicker from '../editor/fields/colorpicker.vue';
import CountryPicker from '../editor/fields/countrypicker.vue';
import SpacePicker from '../editor/fields/spacepicker.vue';
import MailTemplatePicker from '../editor/fields/mailtemplatepicker.vue';
import CulturePicker from '../editor/fields/culturepicker.vue';
import DatePicker from '../editor/fields/datepicker.vue';
import DateRangePicker from '../editor/fields/daterangepicker.vue';
import IconPicker from '../editor/fields/iconPicker.vue';
import LanguagePicker from '../editor/fields/language.vue';
import PagePicker from '../editor/fields/pagepicker.vue';
import InputList from '../editor/fields/inputlist.vue';
import Media from '../editor/fields/media.vue';
import Modules from '../editor/fields/modules.vue';
import Nested from '../editor/fields/nested.vue';
import State from '../editor/fields/state.vue';
import Tags from '../editor/fields/tags.vue';


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
    coreDatabase: false,
    class: ''
  };

  #preview = {
    icon: 'fth-filter',
    preview: x => x,
    hasValue: x => !!x
  };

  #component = null;
  #componentOptions = {};
  #required = false;
  #isReadOnly = false;

  constructor(path, options)
  {
    this.path = path;
    this.options = { ...this.options, ...options };
  }


  get component()
  {
    return this.#component;
  }

  get componentOptions()
  {
    return this.#componentOptions;
  }

  get isRequired()
  {
    return this.#required;
  }

  get previewOptions()
  {
    return this.#preview;
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
    this.#preview = { ...field.previewOptions };
    this.#component = field.component;
    this.#componentOptions = field.componentOptions;
    this.#required = field.isRequired;
    return this;
  }


  /**
   * 
   */
  _setComponent(component, options?)
  {
    this.#component = component;
    this.#componentOptions = options || {};
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
      this.#required = condition;
    }
    else if (typeof condition === 'boolean')
    {
      this.#required = condition;
    }
    else
    {
      this.#required = true;
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
   * @param {string} [placeholder] - Placeholder text (can be a translation)
   * @returns {EditorField}
   */
  text(maxLength, placeholder)
  {
    return this._setComponent(Text, { maxLength, placeholder });
  }


  /**
   * Render a currency input field
   * @param {string} [placeholder] - Placeholder text (can be a translation)
   * @returns {EditorField}
   */
  currency(placeholder)
  {
    return this._setComponent(Currency, { placeholder });
  }


  /**
   * Render a number input field
   * @param {number} [maxLength] - Maximum length of the input
   * @param {string} [placeholder] - Placeholder text (can be a translation)
   * @returns {EditorField}
   */
  number(maxLength, placeholder)
  {
    return this._setComponent(Number, { maxLength, placeholder });
  }


  /**
   * Render a rich-text editor field
   * @returns {EditorField}
   */
  rte()
  {
    return this._setComponent(Rte);
  }


  /**
   * @typedef {object} EditorSelectItem
   * @param {object} key - Key/Id of the item
   * @param {string} value - Label/Value of the item (can be a translation)
   */

  /**
   * Render a select dropdown with the specified items
   * @param {EditorSelectItem[]} items - Set items to pick from
   * @returns {EditorField}
   */
  select(items)
  {
    return this._setComponent(Select, { items });
  }


  /**
   * Render a text area
   * @param {number} [maxLength] - Maximum length of the input
   * @returns {EditorField}
   */
  textarea(maxLength)
  {
    return this._setComponent(Textarea, { maxLength });
  }


  /**
   * Render a toggle
   * @param {boolean} [negative] - Toggle with a negative color / red background
   * @returns {EditorField}
   */
  toggle(negative)
  {
    return this._setComponent(Toggle, { negative });
  }


  /**
   * Renders the field value
   * @param {function} [render] - Render the output based on the given function
   * @returns {EditorField}
   */
  output(render)
  {
    this.#isReadOnly = true;
    return this._setComponent(Output, { render });
  }


  /**
   * Renders an input which generates an alias for a given name or an alternative custom alias
   * @param {string} [namePath] - Optional path to the name value which is used to auto-generate the alias
   * @returns {EditorField}
   */
  alias(namePath)
  {
    return this._setComponent(Alias, { namePath });
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
    return this._setComponent(Checklist, { items, ...options });
  }


  /**
   * Renders a HEX color picker
   * @returns {EditorField}
   */
  colorPicker()
  {
    return this._setComponent(ColorPicker);
  }


  /**
   * Renders a country picker
   * @param {number} [limit=1] - Maximum items to be selected
   * @returns {EditorField}
   */
  countryPicker(limit)
  {
    return this._setComponent(CountryPicker, { limit });
  }


  /**
   * Renders a space picker
   * @param {number} [limit=1] - Maximum items to be selected
   * @returns {EditorField}
   */
  spacePicker(limit)
  {
    return this._setComponent(SpacePicker, { limit });
  }


  /**
   * Renders a culture picker
   * @returns {EditorField}
   */
  culturePicker()
  {
    return this._setComponent(CulturePicker);
  }


  /**
   * Renders a mail template picker
   * @param {number} [limit=1] - Maximum items to be selected
   * @returns {EditorField}
   */
  mailTemplatePicker(limit)
  {
    return this._setComponent(MailTemplatePicker, { limit });
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
    return this._setComponent(DatePicker, { ...options });
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
    return this._setComponent(DateRangePicker, { ...options });
  }


  /**
   * Pick an icon from the specified icon collection
   * @param {string[]} [icons] - Custom icon set with icon class names
   * @returns {EditorField}
   */
  iconPicker(icons)
  {
    return this._setComponent(IconPicker, { icons });
  }


  /**
   * Renders a page picker
   * @param {object} [options] - Custom options
   * @param {number} [options.limit=1] - Limit of selection
   * @returns {EditorField}
   */
  pagePicker(options)
  {
    return this._setComponent(PagePicker, { ...options });
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
    return this._setComponent(InputList, { limit, maxItemLength, addLabel });
  }


  /**
   * Append tags to an entity
   * @param {number} [limit=10] - Limit the tags
   * @param {number} [maxItemLength=200] - Maximum length for a tag
   * @returns {EditorField}
   */
  tags(limit, maxItemLength)
  {
    return this._setComponent(Tags, { limit, maxItemLength });
  }


  /**
   * Pick a language
   * @returns {EditorField}
   */
  languagePicker()
  {
    return this._setComponent(LanguagePicker);
  }


  /**
   * Display a module renderer which allows you to select from defined modules
   * @param {string[]} [tags] - Only allow selection of modules which match defined tags
   * @returns {EditorField}
   */
  modules(tags)
  {
    return this._setComponent(Modules, { tags });
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
   * @returns {EditorField}
   */
  nested(editor, options)
  {
    return this._setComponent(Nested, { editor, ...options });
  }


  /**
   * Render a select as a button group
   * @param {EditorSelectItem[]} items - Set items to choose from
   * @returns {EditorField}
   */
  state(items)
  {
    return this._setComponent(State, { items });
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
    return this._setComponent(Media, { ...options });
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
    return this._setComponent(Media, { ...options, fileExtensions: ['.jpg', '.jpeg', '.png', '.webp', '.svg'] });
  }


  /**
   * Create a preview for this field
   * This is only used in list filters, ...
   * @param {object} options - Custom options
   * @param {number} options.icon - Custom icon
   * @param {number} options.preview - Render the preview when this filter has been filled out
   * @param {number} options.hasValue - Determine if the filter has a value or not
   * @returns {EditorField}
   */
  preview(options)
  {
    this.#preview = { ...this.preview, ...options };
    return this;
  }
}


export default EditorField;