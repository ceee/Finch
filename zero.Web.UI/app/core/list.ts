
import Editor from './editor.ts';
import ListColumn from './list-column.ts';
import ListAction from './list-action.ts';

class List
{
  #alias;
  #fetch;
  #filterOptions;

  /**
   * Set the default query options which are passed to the fetch function.
   * You can extend it to your needs
   */
  query = {
    // default order by
    orderBy: 'createdDate',
    // order is descending
    isDescending: true,
    // current page index
    page: 1,
    // default items per page
    pageSize: 25,
    // search term
    search: null,
    // custom filter 
    filter: null
  };

  /**
   * Overrides the string generation for the label
   */
  templateLabel = field => field;

  /**
   * Build a link for a row (returns options passed to <router-link /> or a route name where id-param is automatically inserted)
   */
  link = null;

  columns = [];

  actions = [];


  constructor(alias)
  {
    this.#alias = alias;
  }


  get alias()
  {
    return this.#alias;
  }

  get filterOptions()
  {
    return this.#filterOptions;
  }


  /**
   * Specify a list column
   * @param {string} path - Model path
   * @param {object} [options] - Custom options
   * @param {string} [options.label] - A custom label for this column (otherwise it's generated via `templateLabel`)
   * @param {boolean} [options.hideLabel=false] - Hide the column label
   * @param {number} [options.width] - Custom width of the column in px
   * @param {boolean} [options.canSort=true] - Disable/enable sorting within this column
   * @param {string} [options.class] - Append HTML class to the generated cells
   * @returns {ListColumn}
   */
  column(path, options)
  {
    let column = this.columns.find(x => x.path === path);

    if (!column)
    {
      column = new ListColumn(path, options);
      this.columns.push(column);
    }
    else
    {
      column.options = { ...column.options, ...(options || {}) };
    }

    return column;
  }


  /**
   * Add an action to the list header (only used when it is attached to <ui-table-filter />)
   * @param {string} key - Alias to refer to
   * @param {string} label - Dropdown-item label
   * @param {string} icon - Displayed dropdown-item icon
   * @param {function} callback - Called when the action button is clicked (including dropdown options as parameter)
   * @param {boolean} [autoclose=true] - Autoclose the actions overlay when this action is clicked
   * @returns {ListAction}
   */
  action(key, label, icon, callback, autoclose)
  {
    const action = new ListAction(key, label, icon, callback);
    action.autoclose = typeof autoclose === 'undefined' ? true : autoclose;
    this.actions.push(action);
    return action;
  }


  /**
   * Shortcut for an "export" action with predefined key, label and icon
   * @param {Promise} callback - A promise which is called when the action button is clicked (including dropdown options as parameter)
   * @returns {ListColumn}
   */
  export(callback)
  {
    return this.action('export', '@ui.export.action', 'fth-share', opts =>
    {
      opts.loading(true);
      callback().then(_ => opts.hide());
    }, false);
  }


  /**
   * Get list items with this function
   * @param {function} callback - This function is called when the list is requested (parameter is the filter object). You should either return a Promise or an Array here.
   */
  onFetch(callback)
  {
    this.#fetch = callback;
  }


  /**
   * Fetch items with the specified filter
   */
  fetch(filter)
  {
    return this.#fetch(filter);
  }


  /**
   * Filter a list by providing an editor renderer and a default template.
   * This activates the Filter button and overlay in the <ui-table-filter /> component.
   * @param {Editor} editor - An editor renderer which opens in an overlay
   * @param {object} template - The default template to use when creating a new filter
   */
  useFilter(editor, template)
  {
    this.#filterOptions = { editor, template };
  }
};

export default List;