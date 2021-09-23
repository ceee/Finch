
import Strings from 'zero/helpers/strings.js';
import EditorField from './editor-field.ts';

class Editor
{
  _alias;
  _prefix;

  _preview = {
    icon: null,
    template: null,
    hideLabel: false
  };

  /**
   * Overrides the string generation for the label
   */
  templateLabel = field => this._prefix + field;
  /**
   * Overrides the string generation for the description
   */
  templateDescription = field => this._prefix + field + '_text';

  tabs = [];
  fields = [];

  options = {
    disabled: false,
    display: 'tabs',
    coreDatabase: false
  };


  constructor(alias, prefix)
  {
    this._alias = alias;
    this._prefix = prefix || '';
  }


  get alias()
  {
    return this._alias;
  }

  get previewOptions()
  {
    return this._preview;
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
    if (typeof disabled !== 'undefined' && disabled != null && typeof disabled !== 'boolean' && typeof disabled !== 'function')
    {
      console.warn(`[zero] editor.tab: the disabled property has to be of type [boolean, function, undefined]`);
      return;
    }

    if (typeof count !== 'undefined' && count != null && typeof count !== 'number' && typeof count !== 'function')
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
   * @param {boolean} [options.allTabs] - Add this field to all defined tabs (could be an information property for instance)
   * @param {string} [options.classes] - Append HTML class to the generated property
   * @returns {EditorField}
   */
  field(path, options)
  {
    options = options || {};

    //let field = this.fields.find(x => x.path === path); 
    // TODO we need another method to retrieve existing fields
    // but we can't do it this way anymore as sometimes a field is defined multiple times with a condition

    if (this.tabs.length < 1)
    {
      this.tab('content', '@ui.tab_content', x => 0, x => false, null, null);
    }

    //if (!field)
    //{
      if (typeof options.coreDatabase === 'undefined')
      {
        options.coreDatabase = this.options.coreDatabase;
      }
      if (!options.tab)
      {
        options.tab = 'content';
      }
      let field = new EditorField(path, options);
      this.fields.push(field);
    //}
    //else
    //{
    //  field.options = { ...field.options, ...options };
    //}

    return field;
  }


  /**
   * Add a new fieldset to the editor or returns the tab in case it was already added
   * A fieldset combines properties in a row (side-by-side)
   * @param {function} [configure] - Configures the fieldset
   */
  fieldset(configure)
  {
    let set = this._createFieldset();
    configure(set);
  }


  /**
   * Adds an info tab to the editor which outputs data for ZeroEntity.
   * This won't work as expected for other entities as they probably do not have required properties defined.
   * @returns {EditorTab}
   */
  infoTab()
  {
    return this.tab('infos', '@ui.tab_infos', x => 0, false, 'is-blank', () => import('../editor/editor-infos.vue'));
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
    return this.fields.filter(x => !alias || x.options.allTabs ? true : x.options.tab === alias);;
  }


  /**
   * Get fields grouped in fieldsets for the specified tab
   * @param {string|EditorTab} [tab] - Pass the tab or its alias
   * @returns {EditorField[]}
   */
  getFieldsets(tab)
  {
    let fields = this.getFields(tab);
    let currentFieldset = "__undefined";
    let fieldsets = [];

    fields.forEach(field =>
    {
      if (field.options.fieldset != currentFieldset || !field.options.fieldset)
      {
        currentFieldset = field.options.fieldset;
        fieldsets.push({
          fields: [],
          cols: []
        });
      }

      fieldsets[fieldsets.length - 1].fields.push(field);
      fieldsets[fieldsets.length - 1].cols.push(field.options.fieldsetColumns);
    });

    fieldsets.forEach(fieldset =>
    {
      fieldset.count = fieldset.fields.length;
      let reserved = fieldset.cols.reduce((acc, x) => acc + (x || 0), 0);
      let rest = reserved < 1 ? 12 : (12 - reserved) % 12;
      let columnsToFill = fieldset.cols.filter(x => !x).length;
      let perColumn = Math.floor(rest / columnsToFill);

      fieldset.fields.forEach(field =>
      {
        if (!field.options.fieldsetColumns)
        {
          field.options.fieldsetColumns = perColumn;
        }
      });
    });

    return fieldsets;
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
    this._preview = { ...editor.previewOptions };
    return this;
  }


  /**
   * Create a preview for this editor
   * This is primarly used for <ui-modules /> module previews
   * @param {string|object} template - A vue template string (`model` is passed as property for the module content) or a custom vue component
   * @param {object} [options] - Custom options
   * @param {number} [options.icon] - Custom icon
   * @param {number} [options.hideLabel=false] - Whether to hide the displayed module-type label in the preview
   * @returns {Editor}
   */
  preview(template, options)
  {
    this._preview = { ...this._preview, template, ...options };
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
      fieldset: configure =>
      {
        let set = this._createFieldset(alias);
        configure(set);
      },
      removeField: path => this.removeField(path)
    };
  } 


  /**
   * Create a new fieldset
   */
  _createFieldset(tab)
  {
    let id = Strings.guid();

    return {
      id,
      field: (path, options) =>
      {
        options = options || {};
        options.fieldset = id;
        options.tab = tab;
        return this.field(path, options);
      },
      removeField: path => this.removeField(path)
    };
  }

};

export default Editor;