<template>
  <div class="ui-table-outer">
    <div class="ui-table">
      <header class="ui-table-row ui-table-head">      
        <div v-for="column in columns" class="ui-table-cell" :table-field="column.field" :style="column.flex">
          {{ column.label | localize }}
          <button :disabled="!column.canSort" @click="sort(column)" type="button" class="ui-table-sort" :class="filter.orderBy == column.field ? 'sort-' + (filter.orderIsDescending ? 'desc' : 'asc') : null">
            <i class="arrow arrow-down"></i>
          </button>
        </div>
      </header>

      <div class="ui-table-row" v-for="item in items">
        <component :is="column.tag" :to="getLink(column, item)" v-for="column in columns" class="ui-table-cell" :style="column.flex" :table-field="column.field" v-table-value="{ item, column }"></component>
      </div>

      <div class="ui-table-empty" v-if="!isLoading && items.length < 1" @click="isLoading=true">
        <i class="ui-table-empty-icon fth-list"></i>
        There are no items to show in this list
      </div>

      <div class="ui-table-loading" v-if="isLoading" @click="isLoading=false">
        <ui-loading />
      </div>
    </div>

    <footer class="ui-table-pagination" v-if="pages > 1">
      <ui-pagination :pages="pages" :page="filter.page" @change="setPage" />
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
    // define columns and how they are displayed
    columns: {},
    // prefix for column header translations
    labelPrefix: '',
    // scroll to top on page change
    scrollToTop: true,
    // promise which returns items based on the current filter and sorting
    items: null
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
      filter: {
        deep: true,
        handler: function ()
        {
          this.debouncedUpdate();
        }
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
        pageSize: 30,
        search: null
      },
      debouncedUpdate: null
    }),

    created()
    {
      this.debouncedUpdate = _debounce(this.update, 300);

      this.configuration = _extend(defaultConfig, this.value);

      if (this.configuration.order.enabled)
      {
        this.filter.orderBy = this.configuration.order.by;
        this.filter.orderIsDescending = this.configuration.order.isDescending;
      }

      this.generateColumns(this.configuration.columns);
    },

    mounted()
    {
      this.load(true);
    },

    directives: {
      'table-value': TableValueDirective
    },

    methods: {

      getLink(column, item)
      {
        if (column.tag !== 'router-link')
        {
          return {};
        }

        return column.link(item);
      },

      // load items based on the current filter
      load(initial)
      {
        this.configuration.items(this.filter).then(result =>
        {
          this.pages = result.totalPages;
          this.count = result.totalItems;

          this.isLoading = false;
          this.items = result.items;

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

          this.columns.push(_extend(data, {
            tag: typeof data.link !== 'undefined' ? 'router-link' : 'div',
            label: typeof data.label !== 'undefined' ? data.label : (this.configuration.labelPrefix + key),
            field: key,
            canSort: typeof data.sort === 'boolean' ? data.sort : this.configuration.sort,
            flex: data.width ? { 'flex': '0 1 ' + data.width + 'px' } : {}
          }));
        });
      },

      // set the active page
      setPage(index)
      {
        this.filter.page = index;
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
    background: var(--color-box);
    flex-wrap: nowrap;
    -webkit-box-pack: justify;
    justify-content: space-between;
    min-width: auto;
    border-radius: var(--radius);
    table-layout: fixed;
    word-wrap: break-word;
    font-size: var(--font-size);
    width: 100%;
    box-shadow: var(--color-shadow-short);
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
    border-bottom: 1px solid var(--color-line-light);
    position: relative;
    //min-height: 60px;
    outline: 1px solid transparent;
    transition: outline 0.1s ease, box-shadow 0.1s ease;

    &:last-child
    {
      border-bottom: none;
    }
  }

  /*.ui-table-row:not(.ui-table-head):hover
  {
    box-shadow: 0 0 5px 4px var(--color-shadow);
    z-index: 1;
    outline: 1px solid var(--color-line);
    border-bottom-color: transparent;
  }*/

  .ui-table-head
  {
    font-weight: 700;
    border-radius: 5px 5px 0 0;
    color: var(--color-fg);
    position: sticky;
    top: 0;
    border-bottom: 1px solid var(--color-line-light);
    z-index: 2;
    background: var(--color-box);
    //box-shadow: inset 0 -3px 3px rgba(0,0,0,0.02);

    .ui-table-cell
    {
      display: inline-flex;
      justify-content: space-between;
    }
  }

  .ui-table-cell
  {
    display: inline-block;
    align-items: center;
    flex: 1 1 5%;
    position: relative;
    text-align: left;
    padding: 18px 20px 17px 20px;
    border-left: 1px solid var(--color-line-light);
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
  }

  a.ui-table-cell
  {
    color: var(--color-fg);
    transition: none;
   
    &:hover
    {
      //text-decoration: underline;
      color: var(--color-primary);
      background: var(--color-bg-xxlight);
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
      border-top-color: var(--color-fg);
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
    color: var(--color-fg-xlight);

    &:before
    {
      content: "\e8f6";
    }

    &.is-checked
    {
      color: var(--color-fg);

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

  .ui-table .flag
  {
    position: relative;
    top: 2px;
  }
</style>