<template>
  <div class="ui-table">

    <header class="ui-table-row ui-table-head">      
      <div v-for="column in columns" class="ui-table-cell" :table-field="column.field">
        {{ column.label | localize }}
        <button v-if="column.sort || config.sort" type="button" class="ui-table-sort"><i class="arrow-down"></i><i class="arrow-up"></i></button>
      </div>
    </header>

    <content clsas="ui-table-body">
      <div class="ui-table-row" v-for="item in items">
        <div v-for="column in columns" class="ui-table-cell" :table-field="column.field" v-table-value="{ item, column }"></div>
      </div>
    </content>

  </div>
</template>


<script>
  import TableValueDirective from 'zerocomponents/Tables/table-value.js';
  import { each as _each, extend as _extend } from 'underscore';

  export default {
    name: 'uiTable',

    props: {
      config: {
        type: Object,
        required: true,
        default: () =>
        {
          return {
            // allow sorting of columns (asc + desc)
            sort: true,
            // define columns and how they are displayed
            columns: {},
            // prefix for column header translations
            labelPrefix: '',
            // promise which returns items based on the current filter and sorting
            items: null
          };
        }
      }
    },

    watch: {
      'config.columns': function (val)
      {
        this.generateColumns(val);
      }
    },

    data: () => ({
      columns: [],
      items: []
    }),

    created()
    {
      this.generateColumns(this.config.columns);
    },

    mounted()
    {
      this.config.items().then(result =>
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
            label: column.label || (this.config.labelPrefix + key),
            field: key
          }));
        });
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
    align-items: center;
    width: 100%;
    border-bottom: 1px solid var(--color-line-light);
    position: relative;
    min-height: 60px;
  }

  .ui-table-head
  {
    font-weight: 700;
    border-radius: 5px 5px 0 0;
    color: var(--color-fg);
    position: sticky;
    top: 0;
    //background: var(--color-bg-mid);
    z-index: 3;
  }

  .ui-table-cell
  {
    flex: 1 1 5%;
    position: relative;
    text-align: left;
    overflow: hidden;
    margin: 0 0 0 30px;
    padding: 15px 0;
    white-space: nowrap;
    text-overflow: ellipsis;
  }

  .ui-table-sort
  {
    display: none;
    /*height: 40px;
    width: 40px;
    display: inline-flex;
    align-items: center;
    justify-content: center;

    .arrow-down
    {
      display: none;
    }

    &.is-active
    {
      .arrow-down, .arrow-up
      {
        border
      }
    }*/
  }
</style>