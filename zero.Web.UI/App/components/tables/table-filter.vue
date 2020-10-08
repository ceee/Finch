<template>
  <div class="ui-table-filter">
    <ui-search v-if="!hideSearch" v-model="value.search" class="onbg" />
    <ui-dropdown v-if="!hideFilter && filters.length" align="right">
      <template v-slot:button>
        <ui-button v-if="!hideFilter" type="light onbg" label="Filter" caret="down" />
      </template>
      <ui-dropdown-button label="@ui.add" icon="fth-plus" @click="onFilterAdd" />
      <ui-dropdown-button label="Clear filter" icon="fth-x" @click="onFilterClear" />
      <ui-dropdown-separator v-if="filters.length" />
      <ui-dropdown-button v-if="filters.length" v-for="(filter, index) in filters" :key="index" :value="filter" :label="filter.label" :icon="filter.icon" @click="onFiltered" />
    </ui-dropdown>
    <ui-button v-if="!hideFilter && !filters.length" type="light onbg" label="Filter" @click="onFilterAdd" />
    <ui-dropdown v-if="!hideSelection && selection.length > 0" align="right">
      <template v-slot:button>
        <ui-button type="light" :label="selectedText" caret="down" />
      </template>
      <slot name="actions"></slot>
    </ui-dropdown>
    <ui-dropdown v-if="value.actions && value.actions.length > 0" align="right">
      <template v-slot:button>
        <ui-button type="light onbg" icon="fth-more-horizontal" />
      </template>
      <ui-dropdown-button v-for="(action, index) in value.actions" :key="index" :value="action" :prevent="action.autoclose === false" :label="action.label" :icon="action.icon" @click="onActionClicked" />
    </ui-dropdown>
  </div>
</template>


<script>
  import Overlay from 'zero/services/overlay';
  import FilterOverlay from './table-filter-overlay';
  import { isArray as _isArray } from 'underscore';

  export default {
    name: 'uiTableFilter',

    props: {
      value: {
        type: Object,
        required: true,
        default: () =>
        {
          return {
            filter: {},
            actions: [],
            selectable: false,
            search: null
          }
        }
      },
      selection: {
        type: Array,
        default: () => []
      }
    },

    watch: {
      value: function (value)
      {
        this.reload();
      }
    },

    data: () => ({
      hideFilter: true,
      hideSearch: true,
      hideSelection: true,
      filters: []
    }),

    created()
    {
      this.reload();
    },

    computed: {
      selectedText()
      {
        return this.selection.length + ' selected'; 
      }
    },

    methods: {

      reload()
      {
        this.hideFilter = typeof this.value.filter !== 'object';
        this.hideSearch = typeof this.value.search === false;
        this.hideSelection = this.value.selectable !== true;
      },

      onActionClicked(action, opts)
      {
        action.action(opts);
      },


      onFilterAdd()
      {
        this.openFilter();
      },

      onFilterClear()
      {

      },

      onFiltered()
      {

      },

      openFilter()
      {
        return Overlay.open({
          component: FilterOverlay,
          display: 'editor',
          title: 'Filter',
          defaults: this.value.filter.model,
          model: this.value.filter.model,
          fields: this.value.filter.fields
        }).then(value =>
        {
          console.info(value);
        });
      }
    }
  }
</script>

<style lang="scss">
  .ui-table-filter
  {
    display: flex;
    align-items: center;
    height: 100%;

    > * + *
    {
      margin-left: 15px;
    }
  }
</style>