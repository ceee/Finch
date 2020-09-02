<template>
  <div class="ui-datagrid-outer">
    <div class="ui-datagrid">
      <div class="ui-datagrid-items" :style="'grid-template-columns: repeat(auto-fill, minmax(' + configuration.width + 'px, 1fr))'" :class="{'is-block': configuration.block }">
        <component v-for="(item, index) in items" :is="configuration.component" :key="index" :value="item" class="ui-datagrid-cell"></component>
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
    block: false
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
      '$route': 'load',
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
      debouncedUpdate: null
    }),

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
</style>