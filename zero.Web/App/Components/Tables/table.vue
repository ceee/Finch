<template>
  <div class="ui-table-outer">
    <div class="ui-table">
      <header class="ui-table-row ui-table-head">      
        <div v-for="column in columns" class="ui-table-cell" :table-field="column.field" :style="column.flex">
          {{ column.label | localize }}
          <button :disabled="!column.canSort" @click="sort(column)" type="button" class="ui-table-sort" :class="filter.orderBy == column.field ? 'sort-' + (filter.isDescending ? 'desc' : 'asc') : null">
            <i class="arrow arrow-down"></i>
          </button>
        </div>
      </header>

      <div class="ui-table-row" v-for="item in items">
        <div v-for="column in columns" class="ui-table-cell" :style="column.flex" :table-field="column.field" v-table-value="{ item, column }"></div>
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
  import { each as _each, extend as _extend } from 'underscore';

  const defaultConfig = {
    // allow sorting of columns (asc + desc)
    sort: true,
    // define columns and how they are displayed
    columns: {},
    // prefix for column header translations
    labelPrefix: '',
    // promise which returns items based on the current filter and sorting
    items: null
  };

  export default {
    name: 'uiTable',

    props: {
      config: {
        type: Object,
        required: true,
        default: defaultConfig
      }
    },

    components: { UiPagination },

    watch: {
      'config.columns': function (val)
      {
        this.generateColumns(val);
      }
    },

    data: () => ({
      configuration: {},
      columns: [],
      items: [],
      pages: 8,
      filter: {
        orderBy: null,
        isDescending: true,
        page: 1
      }
    }),

    created()
    {
      this.configuration = _extend(defaultConfig, this.config);
      this.generateColumns(this.configuration.columns);
    },

    mounted()
    {
      this.configuration.items().then(result =>
      {
        this.items = result;
      });
    },

    directives: {
      'table-value': TableValueDirective
    },

    methods: {

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
            label: data.label || (this.configuration.labelPrefix + key),
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
        if (this.filter.orderBy === column.field && this.filter.isDescending)
        {
          this.filter.isDescending = false;
        }
        else if (this.filter.orderBy === column.field)
        {
          this.filter.orderBy = null;
        }
        else
        {
          this.filter.orderBy = column.field;
          this.filter.isDescending = true;
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

  .ui-table-row:not(.ui-table-head):hover
  {
    box-shadow: 0 0 5px 4px var(--color-shadow);
    z-index: 4;
    outline: 1px solid var(--color-line);
    border-bottom-color: transparent;

    /*&:before
    {
      position: absolute;
      left: 0;
      top: 0;
      bottom: 0;
      content: '';
      width: 3px;
      background: var(--color-line);
    }*/
  }

  .ui-table-head
  {
    font-weight: 700;
    border-radius: 5px 5px 0 0;
    color: var(--color-fg);
    position: sticky;
    top: 0;
    border-bottom: 1px solid var(--color-line-light);
    z-index: 3;
    background: var(--color-box);
    box-shadow: inset 0 -3px 3px rgba(0,0,0,0.02);

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

  /* special styling for display types */

  .ui-table-field-bool
  {
    font-family: var(--font-icon);
    color: var(--color-fg-light);

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
</style>