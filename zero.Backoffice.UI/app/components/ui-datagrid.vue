<template>
  <div class="ui-datagrid-outer" :class="{'is-selecting': configuration.selectable && selected.length > 0}">
    <div class="ui-datagrid">
      <div class="ui-datagrid-items" v-if="!isLoading" :style="'grid-template-columns: repeat(auto-fill, minmax(' + configuration.width + 'px, 1fr))'" :class="{'is-block': configuration.block }">
        <slot name="before"></slot>
        <div class="ui-datagrid-item" v-for="(item, index) in items" :key="index" v-on:contextmenu="onRightClicked(item, $event)">
          <button v-if="configuration.selectable && selected.length > 0" type="button" class="ui-datagrid-cell-select" @click.exact="select(item)" @click.shift="shiftSelect(item)"></button>
          <slot :item="item" :selected="configuration.selectable && selected.indexOf(item) > -1"></slot>
        </div>
        <slot name="below"></slot>
      </div>

      <div class="ui-datagrid-empty" v-if="!isLoading && items.length < 1">
        <ui-icon symbol="fth-list" :size="34" class="ui-datagrid-empty-icon" />
        <span v-localize="'@ui.emptylist'"></span>
      </div>

      <div class="ui-datagrid-loading" v-if="isLoading">
        <ui-loading />
      </div>
    </div>

    <footer class="ui-datagrid-pagination" v-if="pages > 1">
      <ui-pagination :pages="pages" :page="filter.page" @change="setPage" />
    </footer>

    <ui-dropdown ref="dropdown" align="top" theme="dark" class="ui-datagrid-dropdown">
      <slot name="actions" v-bind="actionProps"></slot>
    </ui-dropdown>

  </div>
</template>


<script>
  import UiPagination from './ui-pagination.vue';
  import { debounce, extendObject } from '../utils';
  import Qs from 'qs';

  const defaultConfig = {
    order: {
      // allow sorting of columns (asc + desc)
      enabled: true,
      // default order by
      by: 'createdDate',
      // order is descending
      isDescending: true
    },
    // desired width of an item
    width: 280,
    // scroll to top on page change
    scrollToTop: true,
    // promise which returns items based on the current filter and sorting
    items: null,
    // for block items
    block: false,
    // ability to select items
    selectable: false,
    // whether the query params should be set for page + search
    setQuery: true,
    // custom scroll container
    scrollContainerSelector: null,
    // filter
    page: 1,
    pageSize: 30
  };

  export default {
    name: 'uiDatagrid',

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
      '$route': function (val)
      {
        this.load(true);
      },
      'value.search': function (val)
      {
        this.filter.search = val;
      },
      'filter.search': function (val)
      {
        //this.filter.page = 1;
        this.debouncedUpdate();
      }
    },

    data: () => ({
      configuration: {},
      items: [],
      isLoading: true,
      pages: 1,
      count: 0,
      loaded: false,
      filter: {
        orderBy: null,
        orderIsDescending: true,
        page: 1,
        pageSize: 30,
        search: null
      },
      debouncedUpdate: null,
      actionProps: {
        item: null,
        selected: false
      },
      selected: []
    }),


    computed: {
      actionsDefined()
      {
        return this.$slots.hasOwnProperty('actions');
      }
    },


    mounted()
    {
      this.setup();
    },


    methods: {

      async setup()
      {
        this.debouncedUpdate = debounce(this.update, 300);

        this.configuration = extendObject(JSON.parse(JSON.stringify(defaultConfig)), this.value);

        if (this.configuration.order.enabled)
        {
          this.filter.orderBy = this.configuration.order.by;
          this.filter.orderIsDescending = this.configuration.order.isDescending;
        }

        await this.load(true);
      },

      // load items based on the current filter
      async load(initial)
      {
        if (!this.loaded && !initial)
        {
          return;
        }

        if (initial)
        {
          this.filter.page = (!this.configuration.setQuery ? null : +this.$route.query.page) || this.configuration.page || 1;
          this.filter.pageSize = this.configuration.pageSize || 30;
          this.filter.search = (!this.configuration.setQuery ? null : this.$route.query.search) || this.configuration.search;
        }

        const result = await this.configuration.items(this.filter);

        this.pages = result.paging.totalPages;
        this.count = result.paging.totalItems;
        this.$emit('count', this.count);

        this.isLoading = false;
        this.items = result.data;

        if (!initial && this.configuration.scrollToTop)
        {
          let container = document.querySelector(this.configuration.scrollContainerSelector || '.app-main');

          if (container)
          {
            this.$nextTick(() => container.scrollTo({ top: 0, behavior: 'smooth' }));
          }
        }

        setTimeout(() =>
        {
          this.loaded = true;
        }, 500);
      },

      // updates the list (debounced)
      async update()
      {
        if (!this.isLoading)
        {
          await this.load();
        }
      },

      // set the active page
      setPage(index)
      {
        this.filter.page = index;
        this.onChange();
      },

      // search
      search(query)
      {
        this.filter.search = query;
        this.onChange();
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

        this.onChange();
      },


      // right clicked on an item
      onRightClicked(item, ev)
      {
        if (this.actionsDefined)
        {
          ev.preventDefault();
          this.onActionsClicked(item, ev);
        }
      },


      // a property has changed and therefore we update the URL and table content
      onChange()
      {
        if (this.configuration.setQuery)
        {
          let params = {};

          if (+this.filter.page > 1)
          {
            params.page = +this.filter.page;
          }
          if (this.filter.search)
          {
            params.search = this.filter.search;
          }

          const query = Qs.stringify(params);
          const path = this.$route.href.split('?')[0] + (query ? '?' + query : '');

          history.replaceState(null, null, path);
        }
        this.debouncedUpdate();
      },


      // actions button clicked on item
      onActionsClicked(item, ev)
      {
        let dropdown = this.$refs.dropdown;

        if (!this.actionsDefined || (typeof this.hasActions === 'function' && !this.hasActions(item)))
        {
          return;
        }

        this.actionProps.item = item;
        this.actionProps.event = ev;
        this.actionProps.selected = this.configuration.selectable && this.selected.indexOf(item) > -1;

        dropdown.toggle();

        if (!dropdown.open)
        {
          return;
        }

        this.$nextTick(() =>
        {
          let target = ev.target;
          do
          {
            if (target.classList.contains('ui-datagrid-cell'))
            {
              break;
            }
          }
          while (target = target.parentElement);

          var width = 240;

          var position = {
            x: ev.pageX,
            y: ev.pageY
          };

          let element = dropdown.$el.querySelector('.ui-dropdown');

          element.style.top = position.y + 'px';
          element.style.left = position.x + 'px';
          element.style.width = width + 'px';
        });
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
      },

      shiftSelect(item)
      {
        return;

        // TODO implement shift selection (this is only a part of it)
        // for selection area see Viselect https://simonwep.github.io/selection/

        const last = this.selected[this.selected.length - 1];

        if (last)
        {
          let startIndex = this.items.indexOf(last);
          let endIndex = this.items.indexOf(item);

          if (startIndex > endIndex)
          {
            const tempIndex = endIndex;
            endIndex = startIndex;
            startIndex = tempIndex;
          }

          for (let index = startIndex; index <= endIndex; index++)
          {
            this.selected.push(this.items[index]);
          }
        }
      },


      clearSelection()
      {
        this.selected = [];
        this.$emit('select', this.selected, this);
      }
    }
  }
</script>

<style lang="scss">
  .ui-datagrid-items
  {
    display: grid;
    gap: var(--padding-s);
    grid-template-columns: repeat(auto-fill, minmax(260px, 1fr));
    align-items: stretch;
  }

  .ui-datagrid-items.is-block
  {
    display: block;
  }

  .ui-datagrid-item
  {
    position: relative;
  }

  .ui-datagrid-empty, .ui-datagrid-loading
  {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    height: 250px;
    text-align: center;
    padding: 0 20px;
    font-size: var(--font-size);

    .ui-loading
    {
      left: 0;
    }
  }

  .ui-datagrid-empty-icon
  {
    margin-bottom: 20px;
  }

  .ui-datagrid-dropdown .ui-dropdown
  {
    position: fixed;
    min-width: 200px;
  }

  .ui-datagrid-cell-select
  {
    width: 100%;
    position: absolute;
    left: 0;
    top: 0;
    right: 0;
    bottom: 0;
    background: transparent;
    z-index: 2;
  }
</style>