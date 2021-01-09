
import EditorField from './editor-field.ts';
import Infos from '../editor/editor-infos.vue';

class Editor
{
  #alias;
  #prefix;

  /**
   * Overrides the string generation for the label
   */
  templateLabel = field => this.#prefix + field;
  /**
   * Overrides the string generation for the description
   */
  templateDescription = field => this.#prefix + field + '_text';

  tabs = [];
  fields = [];

  options = {
    disabled: false,
    boxes: false,
    coreDatabase: true
  };


  constructor(alias, prefix)
  {
    this.#alias = alias;
    this.#prefix = prefix || '';
  }


  get alias()
  {
    return this.#alias;
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
   * Add a new tab to the editor or returns the tab in case it was already added
   * @param {string} alias - Alias for the tab
   * @param {string} name - Name of the tab (can be a translation)
   * @param {number|function} [count] - Output a count indicator
   * @param {boolean|function} [disabled] - Conditionally disable the tab and its content
   * @param {string} [classes] - Append HTML class to the generated tab
   * @param {object} [component] - Render a custom vue component instead of editor fields
   * @returns {EditorTab}
   */
  tab(alias, name, count, disabled, classes, component)
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

    let tab = this.tabs.find(x => x.alias === alias);

    if (!tab)
    {
      tab = this._createTab(alias, name, disabled, count, classes, component);
      this.tabs.push(tab);
    }

    return tab;
  }


  /**
   * Add a new field to the editor or returns the field in case it was already added
   * @param {string} path - Model path
   * @param {object} [options] - Custom options
   * @param {string} [options.label] - A custom label for this field (otherwise it's generated via `onLabelCreate`)
   * @param {string} [options.hideLabel] - Hide the field label and make the content full-width
   * @param {string} [options.description] - A custom description for this field (otherwise it's generated via `onDescriptionCreate`)
   * @param {string} [options.helpText] - Display a help text below the field
   * @param {boolean|function} [options.condition] - Conditionally hide the field
   * @param {boolean|function} [options.disabled=false] - Conditionally disable the field
   * @param {boolean} [options.coreDatabase] - Operate on the core database for this field (default is set by Editor.options.coreDatabase)
   * @param {string|object} [options.tab] - Add this field to a tab (by passing the alias or the tab instance)
   * @param {string} [options.classes] - Append HTML class to the generated property
   * @returns {EditorField}
   */
  field(path, options)
  {
    options = options || {};

    let field = this.fields.find(x => x.path === path);

    if (this.tabs.length < 1)
    {
      this.tab('content', '@ui.tab_content', x => 0, x => false, null, null);
    }

    if (!field)
    {
      if (typeof options.coreDatabase === 'undefined')
      {
        options.coreDatabase = this.options.coreDatabase;
      }
      if (!options.tab)
      {
        options.tab = 'content';
      }
      field = new EditorField(path, options);
      this.fields.push(field);
    }
    else
    {
      field.options = { ...field.options, ...options };
    }

    return field;
  }


  /**
   * Adds an info tab to the editor which outputs data for IZeroEntity.
   * This won't work as expected for other entities as they probably do not have required properties defined.
   * @returns {EditorTab}
   */
  infoTab()
  {
    return this.tab('infos', '@ui.tab_infos', x => 0, false, 'is-blank', Infos);
  }


  /**
   * Conditionally disable this editor
   * @param {function|boolean} [condition] - Optionally only disable this editor when a condition is fulfilled or reset the required state with true/false
   */
  disabled(condition)
  {
    this.options.disabled = condition;
    return this;
  }


  /**
   * Get fields for the specified tab
   * @param {string|EditorTab} [tab] - Pass the tab or its alias
   * @returns {EditorField[]}
   */
  getFields(tab)
  {
    const alias = typeof tab === 'undefined' ? null : (typeof tab === 'string' ? tab : tab.alias);
    return this.fields.filter(x => !alias ? true : x.options.tab === alias);
  }


  /**
   * Removes a field by path name
   * @param {string} path - Model path
   */
  removeField(path)
  {
    const field = this.fields.find(x => x.path === path);

    if (field != null)
    {
      const index = this.fields.indexOf(field);
      this.fields.splice(index, 1);
    }
  }


  /**
   * Removes a tab from the editor
   * @param {EditorTab|string} tab - The tab to remove
   */
  removeTab(tab)
  {
    const alias = typeof tab === 'object' ? tab.alias : tab;
    const foundTab = this.tabs.find(x => x.alias === alias);

    if (foundTab != null)
    {
      const index = this.tabs.indexOf(foundTab);
      this.tabs.splice(index, 1);

      this.fields.filter(x => x.tab === alias).forEach(field => this.removeField(field.path));
    }
  }


  /**
   * Set another editor as the base for this editor (copies fields and tabs)
   * @param {Editor} editor - Base editor
   * @returns {Editor}
   */
  setBase(editor)
  {
    this.fields = editor.fields.map(x => new EditorField(x.path).setBase(x));
    this.tabs = editor.tabs.map(x => this._createTab(x.alias, x.name, x.disabled, x.count, x.component));
    return this;
  }


  /**
   * Create a new tab instance
   * @returns {EditorTab}
   */
  _createTab(alias, name, disabled, count, classes, component)
  {
    return {
      alias,
      name,
      disabled,
      count,
      class: classes,
      component,
      field: (path, options) =>
      {
        options = options || {};
        options.tab = alias;
        return this.field(path, options);
      },
      removeField: path => this.removeField(path)
    };
  }

};

export default Editor;