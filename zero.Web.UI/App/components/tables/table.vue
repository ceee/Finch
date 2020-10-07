<template>
  <div class="ui-table-outer">
    <div class="ui-table" :class="{'is-inline': inline }">
      <header class="ui-table-row ui-table-head">      
        <div v-for="column in columns" :key="column.key" class="ui-table-cell" :table-field="column.field" :style="column.flex">
          {{ column.label | localize }}
          <button :disabled="!column.canSort" @click="sort(column)" type="button" class="ui-table-sort" :class="filter.orderBy == column.field ? 'sort-' + (filter.orderIsDescending ? 'desc' : 'asc') : null">
            <i class="arrow arrow-down"></i>
          </button>
        </div>
        <button type="button" v-if="configuration.selectable" table-field="table_selectable" class="ui-table-cell is-head is-selectable" @click="select()">
          <i class="fth-check-square"></i>
        </button>
      </header>

      <div class="ui-table-row" v-for="item in items" :class="{ 'is-selected': configuration.selectable && selected.indexOf(item) > -1 }">
        <component :is="column.tag" type="button" :to="getLink(column, item)" @click="onClick($event, column, item)" v-for="column in columns" :key="column.key" 
                   class="ui-table-cell" :style="column.flex" :table-field="column.field" :field-type="column.as" v-table-value="{ item, column }"></component>
        <button type="button" v-if="configuration.selectable" table-field="table_selectable" class="ui-table-cell is-selectable" @click="select(item)">
          <i class="fth-check-square"></i>
        </button>
      </div>

      <div class="ui-table-empty" v-if="!isLoading && items.length < 1">
        <i class="ui-table-empty-icon fth-list"></i>
        There are no items to show in this list
      </div>

      <div class="ui-table-loading" v-if="isLoading">
        <ui-loading />
      </div>
    </div>

    <footer class="ui-table-pagination" v-if="pages > 1">
      <ui-pagination :pages="pages" :page="filter.page" @change="setPage" :inline="inline" />
    </footer>

  </div>
</template>


<script>
  import TableValueDirective from './table-value.js';
  import UiPagination from 'zero/components/pagination.vue';
  import { each as _each, extend as _extend, debounce as _debounce } from 'underscore';

  const defaultConfig = {
    order: {
      // allow sorting of columns (asc + desc)
      enabled: true,
      // default order by
      by: 'createdDate',
      // order is descending
      isDescending: true
    },
    // default items per page
    pageSize: 25,
    // define columns and how they are displayed
    columns: {},
    // prefix for column header translations
    labelPrefix: '',
    // scroll to top on page change
    scrollToTop: true,
    // promise which returns items based on the current filter and sorting
    items: null,
    // ability to select items
    selectable: false
  };

  export default {
    name: 'uiTable',

    props: {
      value: {
        type: Object,
        required: true,
        default: () =>
        {
          return defaultConfig;
        }
      },
      inline: {
        type: Boolean,
        default: false
      }
    },

    components: { UiPagination },

    watch: {
      'value.columns': function (val)
      {
        this.generateColumns(val);
      },
      'value.search': function (val)
      {
        this.filter.search = val;
      },
      'value.items': function (val)
      {
        this.initialize();
      },
      'filter.search': function (val)
      {
        this.debouncedUpdate();
      },
      $route(to, from)
      {
        this.initialize();
      }
    },

    data: () => ({
      configuration: {},
      columns: [],
      items: [],
      isLoading: true,
      pages: 1,
      count: 0,
      filter: {
        orderBy: null,
        orderIsDescending: true,
        page: 1,
        pageSize: 1,
        search: null
      },
      debouncedUpdate: null,
      selected: []
    }),

    mounted()
    {
      this.initialize();
    },

    directives: {
      'table-value': TableValueDirective
    },

    methods: {

      getLink(column, item)
      {
        if (column.tag !== 'router-link')
        {
          return null;
        }

        return column.link(item);
      },

      onClick(e, column, item)
      {
        e.preventDefault();

        if (typeof column.action === 'function')
        {
          column.action(item);
        }
      },

      initialize()
      {
        this.debouncedUpdate = _debounce(this.update, 300);

        this.configuration = _extend(defaultConfig, this.value);

        this.filter.pageSize = this.configuration.pageSize;

        if (this.configuration.order.enabled)
        {
          this.filter.orderBy = this.configuration.order.by;
          this.filter.orderIsDescending = this.configuration.order.isDescending;
        }

        this.generateColumns(this.configuration.columns);

        this.load(true);
      },

      // load items based on the current filter
      load(initial)
      {
        this.configuration.items(this.filter).then(result =>
        {
          this.$emit('loaded', result);
          this.pages = result.totalPages;
          this.count = result.totalItems;

          this.$emit('count', this.count);

          this.isLoading = false;
          this.items = result.items;
          this.selected = [];

          if (!initial && this.configuration.scrollToTop)
          {
            let container = document.querySelector('.app-main');

            if (container)
            {
              this.$nextTick(() => container.scrollTo({ top: 0, behavior: 'smooth' }));
            }
          }
        });
      },

      // updates the list (debounced)
      update()
      {
        if (!this.isLoading)
        {
          this.load();
        }
      },

      // generate columns from the given config
      generateColumns(columns)
      {
        this.columns = [];

        _each(columns, (column, key) =>
        {
          let data = column;

          if (typeof column === 'string')
          {
            data = {
              as: column
            };
          }

          if (data.as === 'shared')
          {
            data.width = 56;
            data.label = null;
            data.sort = false;
          }

          this.columns.push(_extend(data, {
            key: key,
            tag: typeof data.link !== 'undefined' ? 'router-link' : (typeof data.action !== 'undefined' ? 'button' : 'div'),
            label: typeof data.label !== 'undefined' ? data.label : (this.configuration.labelPrefix + key),
            field: key,
            canSort: typeof data.sort === 'boolean' ? data.sort : this.configuration.order.enabled,
            flex: data.width ? { 'flex': '0 1 ' + data.width + 'px' } : {}
          }));
        });
      },

      // set the active page
      setPage(index)
      {
        this.filter.page = index;
        this.debouncedUpdate();
      },

      // sort by a column
      sort(column)
      {
        if (this.filter.orderBy === column.field && this.filter.orderIsDescending)
        {
          this.filter.orderIsDescending = false;
        }
        else if (this.filter.orderBy === column.field)
        {
          this.filter.orderBy = null;
        }
        else
        {
          this.filter.orderBy = column.field;
          this.filter.orderIsDescending = true;
        }

        this.debouncedUpdate();
      },

      // toggle selection of an item
      select(item)
      {
        if (!item)
        {
          if (this.selected.length >= this.items.length)
          {
            this.selected = [];
          }
          else
          {
            this.selected = [];
            this.items.forEach(item =>
            {
              this.selected.push(item);
            });
          }
        }
        else
        {
          const index = this.selected.indexOf(item);

          if (index > -1)
          {
            this.selected.splice(index, 1);
          }
          else
          {
            this.selected.push(item);
          }
        }

        this.$emit('select', this.selected, this);
      }
    }
  }
</script>

<style lang="scss">
  .ui-table
  {
    display: flex;
    -webkit-box-orient: vertical;
    -webkit-box-direction: normal;
    flex-direction: column;
    position: relative;
    background: var(--color-table);
    flex-wrap: nowrap;
    -webkit-box-pack: justify;
    justify-content: space-between;
    min-width: auto;
    border-radius: var(--radius);
    table-layout: fixed;
    word-wrap: break-word;
    font-size: var(--font-size);
    width: 100%;
    box-shadow: var(--shadow-short);

    &.is-inline
    {
      box-shadow: none;
      border: 1px solid var(--color-line);
    }
  }

  .ui-table-row
  {
    display: flex;
    -webkit-box-orient: horizontal;
    -webkit-box-direction: normal;
    flex-flow: row nowrap;
    -webkit-box-align: center;
    align-items: stretch;
    width: 100%;
    border-bottom: 1px solid var(--color-table-line-horizontal);
    position: relative;
    transition: outline 0.1s ease, box-shadow 0.1s ease;

    &:last-child
    {
      border-bottom: none;
    }
  }


  .ui-table-head
  {
    font-weight: 700;
    border-radius: 5px 5px 0 0;
    color: var(--color-text);
    position: sticky;
    top: 0;
    //border-bottom: 1px solid var(--color-line);
    z-index: 2;
    background: var(--color-table-head);

    .ui-table-cell
    {
      display: inline-flex;
      justify-content: space-between;
    }
  }

  .ui-table-cell
  {
    display: inline-flex;
    align-items: center;
    flex: 1 1 5%;
    position: relative;
    text-align: left;
    padding: 18px 20px 17px 20px;
    border-left: 1px solid var(--color-table-line-vertical);
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    min-width: 20px;
    line-height: 20px;

    &:first-child
    {
      border-left: none;
    }

    &.is-multiline
    {
      white-space: normal;
      overflow: auto;
      text-overflow: initial;
    }

    &.is-bold
    {
      font-weight: bold;
    }

    &.is-selectable
    {
      font-size: var(--font-size-l);
      flex: 0 1 40px;
      text-align: center;
      padding: 0;
      justify-content: center;
    }
  }

  .ui-table-row:not(.is-selected) .ui-table-cell:not(.is-head).is-selectable i
  {
    color: var(--color-text);
    margin-right: 1px;

    &:before
    {
      content: "\e8cb";
    }
  }

  a.ui-table-cell,
  button.ui-table-cell
  {
    color: var(--color-text);
    transition: none;
   
    &:hover
    {
      color: var(--color-primary);
      background: var(--color-table-hover);
    }
  }

  .ui-table-sort
  {
    height: 20px;
    width: 20px;
    display: inline-flex;
    align-items: center;
    justify-content: center;
    margin-right: -10px;
    border-radius: 15px;
    transition: background 0.2s ease;

    &[disabled]
    {
      opacity: 0;
      visibility: hidden;
      pointer-events: none;
    }

    .arrow
    {
      border-top-color: var(--color-text);
      transition: opacity 0.2s ease, transform 0.3s ease;
      opacity: 0.2;
    }

    /*&:hover
    {
      background: var(--color-bg);
    }*/

    &:hover .arrow
    {
      opacity: 0.5;
    }

    &.sort-desc .arrow, &.sort-asc .arrow, 
    {
      opacity: 1;
    }

    &.sort-asc .arrow
    {
      transform: scaleY(-1) translateY(5px);
    }
  }

  .ui-table-empty, .ui-table-loading
  {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: 250px;
    text-align: center;
    padding: 0 20px;
  }

  .ui-table-empty-icon
  {
    font-size: 34px;
    margin-bottom: 20px;
  }


  /* special styling for display types */

  .ui-table-field-bool
  {
    font-family: var(--font-icon);
    color: var(--color-text-dim);

    &:before
    {
      content: "\e8f6";
    }

    &.is-checked
    {
      color: var(--color-text);

      &:before
      {
        content: "\e83f";
      }
    }

    &.is-colored
    {
      color: var(--color-accent-error);

      &.is-checked
      {
        color: var(--color-accent-success);
      }
    }
  }

  .ui-table-field-shared
  {
    font-size: 16px;
    color: var(--color-text-dim-one);

    &.is-inline
    {
      margin-left: 0.5em;
      font-size: 18px;
      position: relative;
      top: -1px;
    }
  }

  .ui-table-field-shared-2
  {
    align-self: center;
    display: inline-block;
    font-size: 9px;
    font-weight: 700;
    text-transform: uppercase;
    background: var(--color-box-nested);
    color: var(--color-text-dim-one);
    height: 22px;
    line-height: 22px;
    padding: 0 10px;
    border-radius: 16px;
    letter-spacing: .5px;
    font-style: normal;
    margin-left: 12px;

    /*&.is-inline
    {
      margin-left: 0.5em;
      position: relative;
    }*/
  }

  .ui-table-cell[field-type="datetime"]
  {
    display: inline;

    .-minor
    {
      color: var(--color-text-dim);
    }
  }

  .ui-table-cell .-minor
  {
    color: var(--color-text-dim);
  }

  .ui-table-field-image
  {
    border-radius: 3px;
  }
</style>