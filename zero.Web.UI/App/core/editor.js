
import EditorField from './editor-field.js';

class Editor
{
  #alias;
  #templateLabel = field => field;
  #templateDescription = field => field;

  tabs = [];
  fields = [];


  constructor(alias)
  {
    this.#alias = alias;
  }


  get alias()
  {
    return this.#alias;
  }

  /**
   * Overrides the string generation for the label
   * @param {function} callback - Called when a label is constructed
   */
  set onLabelCreate(callback)
  {
    if (typeof callback !== 'function')
    {
      console.warn(`[zero] onLabelCreate excepts a function as the first parameter`);
      return;
    }
    this.#templateLabel = callback;
  }

  /**
   * Overrides the string generation for the label description
   * @param {function} callback - Called when a description is constructed
   */
  set onDescriptionCreate(callback)
  {
    if (typeof callback !== 'function')
    {
      console.warn(`[zero] onDescriptionCreate excepts a function as the first parameter`);
      return;
    }
    this.#templateDescription = callback;
  }


  /**
   * A tab within an editor
   * @typedef {object} EditorTab
   * @param {string} alias - Alias for the tab
   * @param {string} name - Name of the tab (can be a translation)
   * @param {number|function} [count] - Output a count indicator
   * @param {boolean|function} [disabled] - Conditionally disable the tab and its content
   */

  /**
   * Add a new tab to the editor
   * @param {string} alias - Alias for the tab
   * @param {string} name - Name of the tab (can be a translation)
   * @param {number|function} [count] - Output a count indicator
   * @param {boolean|function} [disabled] - Conditionally disable the tab and its content
   * @returns {EditorTab}
   */
  tab(alias, name, count, disabled)
  {
    if (typeof disabled !== 'undefined' && typeof disabled !== 'boolean' && typeof disabled !== 'function')
    {
      console.warn(`[zero] editor.tab: the disabled property has to be of type [boolean, function, undefined]`);
      return;
    }

    if (typeof count !== 'undefined' && typeof count !== 'number' && typeof count !== 'function')
    {
      console.warn(`[zero] editor.tab: the count property has to be of type [number, function, undefined]`);
      return;
    }

    if (this.tabs.find(x => x.alias === alias))
    {
      console.warn(`[zero] the editor ${this.#alias} does already have a tab with the alias ${alias}`);
      return;
    }

    const tab = this._createTab(alias, name, disabled, count);
    this.tabs.push(tab);

    return tab;
  }


  /**
   * Add a new tab to the editor
   * @param {string} path - Model path
   * @param {object} [options] - Custom options
   * @param {string} [options.label] - A custom label for this field (otherwise it's generated via `onLabelCreate`)
   * @param {string} [options.description] - A custom description for this field (otherwise it's generated via `onDescriptionCreate`)
   * @param {string} [options.helpText] - Display a help text below the field
   * @param {boolean|function} [options.condition] - Conditionally hide the field
   * @param {boolean|function} [options.disabled=false] - Conditionally disable the field
   * @param {string|object} [options.tab] - Add this field to a tab (by passing the alias or the tab instance)
   * @param {string} [options.class] - Append HTML class to the generated property
   * @returns {EditorField}
   */
  field(path, options)
  {
    const field = new EditorField(path, options);
    return field;
  }


  /**
   * Create a new tab instance
   * @returns {EditorTab}
   */
  _createTab(alias, name, disabled, count)
  {
    return {
      alias,
      name,
      disabled,
      count,
      field: (path, options) =>
      {
        options = options || {};
        options.tab = alias;
        return this.field(path, options);
      }
    };
  }
};

export default Editor;