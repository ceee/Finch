<template>
  <div class="ui-table-outer">
    <div class="ui-table" :class="{'is-inline': inline }">
      <slot name="top"></slot>

      <header class="ui-table-row ui-table-head">      
        <div v-for="column in columns" :key="column.path" class="ui-table-cell" :table-field="column.path" :style="column.flex" :class="column.options.class">
          <span v-localize:html="column.label"></span>
          <button :disabled="!column.options.canSort" @click="sort(column)" type="button" class="ui-table-sort" :class="query.orderBy == column.path ? 'sort-' + (query.orderIsDescending ? 'desc' : 'asc') : null">
            <i class="arrow arrow-down"></i>
          </button>
        </div>
        <!--<button type="button" v-if="configuration.selectable" table-field="table_selectable" class="ui-table-cell is-head is-selectable" @click="select()">
          <i class="fth-check-square"></i>
        </button>-->
      </header>

      <slot name="topRow"></slot>

      <component :is="component" v-for="(item, index) in items" :key="index" :to="getLink(item)" type="button" class="ui-table-row" :class="{ 'is-selected': selected.indexOf(item) > -1 }" @click="onRowClick(item)">
        <div v-for="column in columns" :key="column.path" class="ui-table-cell" :class="column.options.class" :style="column.flex" :table-field="column.path" :field-type="column.type" v-table-value="{ column, item }"></div>
        <!--<button type="button" v-if="configuration.selectable" table-field="table_selectable" class="ui-table-cell is-selectable" @click="select(item)">
          <i class="fth-check-square"></i>
        </button>-->
      </component>

      <div class="ui-table-empty" v-if="!isLoading && items.length < 1">
        <i class="ui-table-empty-icon fth-list"></i>
        There are no items to show in this list
      </div>

      <div class="ui-table-loading" v-if="isLoading">
        <ui-loading />
      </div>
    </div>

    <footer class="ui-table-pagination" v-if="pages > 1">
      <ui-pagination :pages="pages" :page="query.page" @change="setPage" :inline="inline" />
    </footer>

  </div>
</template>


<script>
  import './table.scss'
  import UiPagination from 'zero/components/pagination.vue';
  import TableValue from './table-value.js';
  import { debounce as _debounce, isEqual as _isEqual } from 'underscore';
  import Qs from 'qs';

  export default {
    name: 'uiTable',

    props: {
      config: {
        type: [String, Object],
        required: true
      },
      inline: {
        type: Boolean,
        default: false
      }
    },

    directives: {
      'table-value': TableValue
    },

    components: { UiPagination },

    data: () => ({
      loaded: false,
      listConfig: {},
      columns: [],
      query: {},
      filter: null,

      items: [],
      component: 'div',
      isLoading: true,

      pages: 1,
      count: 0,

      debouncedUpdate: null,
      selected: []
    }),

    mounted()
    {
      this.setup();
    },

    watch: {
      'query.search': function (val)
      {
        if (this.loaded) this.onChange();
      },
      $route(to, from)
      {
        this.setup();
      }
    },

    methods: {

      setup()
      {
        this.debouncedUpdate = _debounce(this.update, 300);
        this.listConfig = typeof this.config === 'string' ? this.zero.getList(this.config) : this.config;
        this.columns = this.listConfig.columns.map(column =>
        {
          return {
            ...column,
            column: column,
            label: column.options.hideLabel ? null : (column.options.label || this.listConfig.templateLabel(column.path)),
            flex: column.options.width ? { 'flex': '0 1 ' + column.options.width + 'px' } : {}
          };
        });
        this.query = { ...this.listConfig.query, ...this.listConfig.queryToParams(this.$route.query) };
        this.component = !!this.listConfig.link ? 'router-link' : (!!this.listConfig.onClick ? 'button' : 'div');
        this.filter = { ...this.listConfig.filterOptions };
        this.$nextTick(() =>
        {
          this.loaded = true;
          this.load(true);
        });
      },


      // load items based on the current query
      load(initial)
      {
        this.isLoading = true;

        this.listConfig.fetch(this.query).then(result =>
        {
          this.$emit('loaded', result);
          this.pages = result.totalPages;
          this.count = result.totalItems;

          this.$emit('count', this.count);

          this.isLoading = false;
          this.items = result.items;
          this.selected = [];

          if (!initial) //&& this.configuration.scrollToTop)
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


      // a property has changed and therefore we update the URL and table content
      onChange()
      {
        const query = Qs.stringify(this.listConfig.paramsToQuery(this.query));
        const path = __zero.path + this.$route.path.substring(1) + (query ? '?' + query : '');

        history.replaceState(null, null, path);
        this.debouncedUpdate();
      },


      getLink(item)
      {
        if (!this.listConfig.link)
        {
          return null;
        }
        else if (typeof this.listConfig.link === 'function')
        {
          return this.listConfig.link(item);
        }
        return {
          name: this.listConfig.link,
          params: {
            id: item.id
          }
        };
      },


      onRowClick(item)
      {
        if (typeof this.listConfig.onClick === 'function')
        {
          this.listConfig.onClick(item);
        }
      },


      // set the active page
      setPage(index)
      {
        this.query.page = index;
        this.onChange();
      },

      // set a new filter
      setFilter(filter)
      {
        this.query.filter = filter;
        this.onChange();
      },

      // sort by a column
      sort(column)
      {
        if (this.query.orderBy === column.path && this.query.orderIsDescending)
        {
          this.query.orderIsDescending = false;
        }
        else if (this.query.orderBy === column.path)
        {
          this.query.orderBy = null;
        }
        else
        {
          this.query.orderBy = column.path;
          this.query.orderIsDescending = true;
        }

        // reset the page on sorting change
        this.query.page = 1;

        this.onChange();
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