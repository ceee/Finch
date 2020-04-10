<template>
  <div class="ui-table-outer">
    <div class="ui-table">
      <header class="ui-table-row ui-table-head">      
        <div v-for="column in columns" class="ui-table-cell" :table-field="column.field">
          {{ column.label | localize }}
          <button v-if="column.canSort" @click="sort(column)" type="button" class="ui-table-sort" :class="filter.orderBy == column.field ? 'sort-' + (filter.isDescending ? 'desc' : 'asc') : null">
            <i class="arrow arrow-down"></i>
          </button>
        </div>
      </header>
      <content class="ui-table-body">
        <div class="ui-table-row" v-for="item in items">
          <div v-for="column in columns" class="ui-table-cell" :table-field="column.field" v-table-value="{ item, column }"></div>
        </div>
      </content>
    </div>

    <footer class="ui-table-pagination" v-if="pages > 1">
      <ui-icon-button class="ui-table-pagination-next" type="white" title="Previous" icon="fth-chevron-left" :disabled="filter.page < 2" @click="setPage(filter.page - 1)" />
      <ui-dropdown align="bottom">
        <template v-slot:button>
          <button type="button" class="ui-button type-blank caret-down ui-table-pagination-select">
            <span class="ui-button-text" v-localize="{ key: '@ui.page_xofy', tokens: { x: filter.page, y: pages }}"></span>
            <i class="ui-button-caret fth-chevron-down"></i>
          </button>
        </template>
        <ui-dropdown-list :items="actions" />
      </ui-dropdown>
      <ui-icon-button class="ui-table-pagination-next" type="white" title="Next" icon="fth-chevron-right" :disabled="filter.page >= pages" @click="setPage(filter.page + 1)" />
    </footer>

  </div>
</template>


<script>
  import TableValueDirective from 'zerocomponents/Tables/table-value.js';
  import Strings from 'zeroservices/strings';
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
      actions: [],
      filter: {
        orderBy: null,
        isDescending: true,
        page: 1
      }
    }),

    computed: {

    },

    created()
    {
      this.configuration = _extend(defaultConfig, this.config);
      this.generateColumns(this.configuration.columns);

      this.actions.push({
        name: 'Create',
        icon: 'fth-plus'
      });
      this.actions.push({
        name: 'Move',
        icon: 'fth-corner-down-right'
      });
      this.actions.push({
        name: 'Copy',
        icon: 'fth-copy',
        disabled: true
      });
      this.actions.push({
        name: 'Sort',
        icon: 'fth-arrow-down'
      });
      this.actions.push({
        type: 'separator'
      });
      this.actions.push({
        name: 'Delete',
        icon: 'fth-x',
        action(item, dropdown)
        {
          dropdown.hide();
        }
      });
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
            label: column.label || (this.configuration.labelPrefix + key),
            field: key,
            canSort: typeof column.sort === 'boolean' ? column.sort : this.configuration.sort
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
    min-height: 60px;
    outline: 1px solid transparent;
    transition: outline 0.1s ease, box-shadow 0.1s ease;

    &:last-child
    {
      border-bottom: none;
    }
  }

  .ui-table-body .ui-table-row:hover
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

    .ui-table-cell
    {
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
    overflow: hidden;
    padding: 10px 20px;
    white-space: nowrap;
    text-overflow: ellipsis;
    border-left: 1px solid var(--color-line-light);

    &:first-child
    {
      border-left: none;
    }
  }

  .ui-table-sort
  {
    height: 29px;
    width: 29px;
    display: inline-flex;
    align-items: center;
    justify-content: center;
    margin-right: -10px;
    border-radius: 15px;
    transition: background 0.2s ease;

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

  .ui-table-pagination
  {
    display: flex;
    justify-content: center;
    margin-top: var(--padding);
    align-items: center;
  }

  .ui-table-pagination-select
  {
    margin: 0 20px;
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