
import ListSchema from '../schemas/list/list';
import ListColumn from '../schemas/list/list-column';

export class Datalist
{
  config: ListSchema;
  columns: ListColumn[] = [];
  query: any = {};
  filter: any = {};
  selection: string[] = [];
  loading: boolean = false;
  totalPages: number = 1;
  totalItems: number = 0;
  items: any[] = [];


  constructor(config: string | ListSchema)
  {
    this.config = typeof config === 'string' ? new ListSchema(config) /*// TODO import ListSchema*/ : config;
  }


  /**
   * Get contain per item depending on linkable properties 
   **/
  getItemContainer()
  {
    return !!this.config.link ? 'router-link' : (!!this.config.onClick ? 'button' : 'div');
  }


  async load()
  {
    this.loading = true;

    const result = await this.config.fetch(this.query);

    //this.$emit('loaded', result);
    this.totalPages = result.totalPages;
    this.totalItems = result.totalItems;
    //this.$emit('count', this.count);

    this.loading = false;
    this.items = result.items;
    this.selection = [];
  }


  _setup(routeQuery: any)
  {
    this.columns = this.config.columns.map(column =>
    {
      return {
        ...column,
        column: column,
        label: column.options.hideLabel ? null : (column.options.label || this.config.templateLabel(column.path)),
        flex: column.options.width ? { 'flex': '0 1 ' + column.options.width + 'px' } : {}
      };
    });

    this.query = { ...this.config.query, ...this.config.queryToParams(routeQuery) };
    this.filter = { ...this.config.filterOptions };
  }

};