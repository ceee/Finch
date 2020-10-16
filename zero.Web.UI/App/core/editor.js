
class Editor
{
  #alias;
  #templateLabel = field => field;
  #templateDescription = field => field;

  tabs = [];


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
   * Add a new tab to the editor
   * @param {string} alias - Alias for the tab
   * @param {string} name - Name of the tab (can be a translation)
   * @param {number|function} [count] - Output a count indicator
   * @param {boolean|function} [disabled] - Conditionally disable the tab and its content
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

    const tab = { alias, name, disabled, count };

    this.tabs.push(tab);

    return tab;
  }


  /**
   * Add a new tab to the editor
   * @param {string} path - Model path
   * @param {object} renderer - Can either be a renderer object or a vue component
   * @param {object} [options] - Custom options
   * @param {string} [options.label] - A custom label for this field (otherwise it's generated via `onLabelCreate`)
   * @param {string} [options.description] - A custom description for this field (otherwise it's generated via `onDescriptionCreate`)
   * @param {string} [options.helpText] - Display a help text below the field
   * @param {boolean|function} [options.condition] - Conditionally hide the field
   * @param {boolean|function} [options.disabled=false] - Conditionally disable the field
   * @param {string} [options.class] - Append HTML class to the generated property
   */
  field(path, renderer, options)
  {
    
  }
};

export default Editor;