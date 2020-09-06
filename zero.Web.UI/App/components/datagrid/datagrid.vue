<template>
  <div class="ui-datagrid-outer">
    <div class="ui-datagrid">
      <div class="ui-datagrid-items" :style="'grid-template-columns: repeat(auto-fill, minmax(' + configuration.width + 'px, 1fr))'" :class="{'is-block': configuration.block }">
        <div class="ui-datagrid-item" v-for="(item, index) in items" :key="index" v-on:contextmenu="onRightClicked(item, $event)">
          <button v-if="configuration.selectable && selected.length > 0" type="button" class="ui-datagrid-cell-select" @click="select(item)"></button>
          <component :is="configuration.component" :value="item" class="ui-datagrid-cell" :class="{ 'is-selected': configuration.selectable && selected.indexOf(item) > -1 }"></component>
        </div>  
      </div>

      <div class="ui-datagrid-empty" v-if="!isLoading && items.length < 1">
        <i class="ui-datagrid-empty-icon fth-list"></i>
        There are no items to show in this list
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
    // define vue component which renderes an item
    component: null,
    // desired width of an item
    width: 280,
    // scroll to top on page change
    scrollToTop: true,
    // promise which returns items based on the current filter and sorting
    items: null,
    // for block items
    block: false,
    // ability to select items
    selectable: false
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
      'value.items': function (val)
      {
        this.initialize();
      },
      'filter.search': function (val)
      {
        this.debouncedUpdate();
      }
    },

    data: () => ({
      configuration: {},
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
      debouncedUpdate: null,
      actionProps: {
        item: null
      },
      selected: []
    }),


    computed: {
      actionsDefined()
      {
        return this.$scopedSlots.hasOwnProperty('actions');
      }
    },


    mounted()
    {
      this.initialize();
    },


    methods: {

      initialize()
      {
        this.debouncedUpdate = _debounce(this.update, 300);

        this.configuration = _extend(defaultConfig, this.value);

        if (this.configuration.order.enabled)
        {
          this.filter.orderBy = this.configuration.order.by;
          this.filter.orderIsDescending = this.configuration.order.isDescending;
        }

        this.load(true);
      },

      // load items based on the current filter
      load(initial)
      {
        if (initial)
        {
          this.filter.page = 1;
          this.filter.search = null;
        }

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


      // right clicked on an item
      onRightClicked(item, ev)
      {
        if (this.actionsDefined)
        {
          ev.preventDefault();
          this.onActionsClicked(item, ev);
        }
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
      }
    }
  }
</script>

<style lang="scss">
  .ui-datagrid-items
  {
    display: grid;
    gap: var(--padding);
    grid-template-columns: repeat(auto-fill, minmax(280px, 1fr));
    align-items: stretch;

    &.is-block
    {
      display: block;
    }
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
  }

  .ui-datagrid-empty-icon
  {
    font-size: 34px;
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