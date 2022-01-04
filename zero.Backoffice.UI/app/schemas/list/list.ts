
import ListColumn from './list-column';
import ListAction from './list-action';
import { ZeroSchema } from 'zero/schemas';

class List implements ZeroSchema
{
  alias: string;

  _fetch;
  _filterOptions;

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
   * Build a link for a row (returns options passed to <ui-link /> or a route name where id-param is automatically inserted)
   */
  link = null;

  /**
   * Convert a row into a button where the callback is this function
   */
  onClick = null;

  columns: ListColumn[] = [];

  componentConfig = {
    active: false,
    component: null,
    width: 320,
    block: false
  };

  scrollContainerSelector = null;

  actions = [];


  /**
   * Set another list as the base for this list (copies all fields)
   * @param {List} list - Base list
   * @returns {List}
   */
  setBase(list)
  {
    this.columns = list.columns.map(x => new ListColumn(x.path).setBase(x));
    this.actions = list.actions.map(x => new ListAction(x.key, x.label, x.icon, x.action, x.autoclose));
    this.query = { ...list.query };
    this.templateLabel = list.templateLabel;
    this.link = list.link;
    this.onClick = list.onClick;
    this.actions = [...list.actions];
    return this;
  }


  /**
   * Converts the list parameters (like page, search, filter, ...) to a vue router query
   */
  paramsToQuery = params =>
  {
    let values: any = {};

    if (params.page !== this.query.page)
    {
      values.page = params.page;
    }
    if (params.isDescending !== this.query.isDescending || params.orderBy !== this.query.orderBy)
    {
      values.by = params.orderBy || this.query.orderBy;
      values.desc = params.isDescending || this.query.isDescending;
    }
    if (!!params.search)
    {
      values.search = params.search;
    }
    if (params.filter && params.filter.id)
    {
      values.filter = params.filter.id;
    }
    return values;
  };

  /**
   * Converts an URL query string to list parameters
   */
  queryToParams = query =>
  {
    if (!query)
    {
      return {};
    }
    let values = JSON.parse(JSON.stringify(this.query));
    if (query.page)
    {
      values.page = +query.page || this.query.page;
    }
    if (query.by)
    {
      values.orderBy = query.by;
    }
    if (query.desc)
    {
      values.isDescending = query.desc === "true" || query.desc === true;
    }
    if (query.search)
    {
      values.search = query.search;
    }
    if (query.filter)
    {
      values.filter = values.filter || {};
      values.filter.id = query.filter;
    }
    return values;
  };


  constructor(alias)
  {
    this.alias = alias;
  }

  get filterOptions()
  {
    return this._filterOptions;
  }


  /**
   * Specify a list column
   * @param {string} path - Model path
   * @param {object} [options] - Custom options
   * @param {string} [options.label] - A custom label for this column (otherwise it's generated via `templateLabel`)
   * @param {boolean} [options.hideLabel=false] - Hide the column label
   * @param {number} [options.index] - Custom position for this column
   * @param {number} [options.width] - Custom width of the column in px
   * @param {boolean} [options.canSort=false] - Disable/enable sorting within this column
   * @param {string} [options.class] - Append HTML class to the generated cells
   * @returns {ListColumn}
   */
  column(path, options)
  {
    let column = this.columns.find(x => x.path === path);

    if (!column)
    {
      column = new ListColumn(path, options);
      if (options && options.index > -1)
      {
        this.columns.splice(options.index, 0, column);
      }
      else
      {
        this.columns.push(column);
      }
    }
    else
    {
      column.options = { ...column.options, ...(options || {}) };
    }

    return column;
  }


  /**
   * Removes a list column
   * @param {string} path - Model path
   */
  removeColumn(path)
  {
    let column = this.columns.find(x => x.path === path);
    if (column != null)
    {
      this.columns.splice(this.columns.indexOf(column), 1);
    }
  }


  /**
   * Use a component instead of columns
   * @param {object} component - The custom vue component
   * @param {object} [options] - Custom options
   * @param {string} [options.width=320] - Desired width of an item (in a grid)
   * @param {boolean} [options.block=false] - Full width per item, therefore block/list display
   */
  component(component, options?)
  {
    options = options || {};

    this.componentConfig = {
      active: component != null,
      component: component,
      width: options.width || 320,
      block: options.block || false
    };
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
      callback().then(_ =>
      {
        opts.loading(false);
        opts.hide();
      });
    }, false);
  }


  /**
   * Get list items with this function
   * @param {function} callback - This function is called when the list is requested (parameter is the filter object). You should either return a Promise or an Array here.
   */
  onFetch(callback)
  {
    this._fetch = callback;
  }


  /**
   * Fetch items with the specified filter
   */
  fetch(filter)
  {
    return this._fetch(filter);
  }


  /**
   * Filter a list by providing an editor renderer and a default template.
   * This activates the Filter button and overlay in the <ui-table-filter /> component.
   * @param {Editor} editor - An editor renderer which opens in an overlay
   * @param {object} template - The default template to use when creating a new filter
   */
  useFilter(editor, template)
  {
    this._filterOptions = { editor, template };
  }
};

export default List;