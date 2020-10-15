<template>
  <div class="ui-table-filter">
    <ui-search v-if="!hideSearch" v-model="value.search" class="onbg" />
    <ui-dropdown v-if="!hideFilter && filters.length" align="right">
      <template v-slot:button>
        <ui-button v-if="!hideFilter" type="light onbg" :label="filterLabel" caret="down" />
      </template>
      <ui-dropdown-button v-if="filters.length" v-for="(filter, index) in filters" :key="index" :value="filter" :label="filter.name" @click="onFiltered" />
      <ui-dropdown-separator v-if="filters.length" />
      <ui-dropdown-button label="@ui.add" icon="fth-plus" @click="openFilter()" />
      <ui-dropdown-button v-if="filter" label="Edit filter" icon="fth-edit-2" @click="openFilter(filter.id)" />
      <ui-dropdown-button label="Clear filter" icon="fth-x" @click="onFilterClear" />
    </ui-dropdown>
    <ui-button v-if="!hideFilter && !filters.length" type="light onbg" label="Filter" @click="openFilter()" />
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
  import Strings from 'zero/services/strings';
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
      },
      alias: {
        type: String,
        default: null
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
      filters: [],
      filter: null
    }),

    created()
    {
      this.reload();
    },

    computed: {
      selectedText()
      {
        return this.selection.length + ' selected'; 
      },
      storageKey()
      {
        return 'zero.ui-table-filter.' + this.alias;
      },
      filterLabel()
      {
        return this.filter ? 'Filter: <span>' + this.filter.name + "</span>" : 'Filter';
      }
    },

    methods: {

      reload()
      {
        this.hideFilter = typeof this.value.filter !== 'object';
        this.hideSearch = typeof this.value.search === false;
        this.hideSelection = this.value.selectable !== true;
        this.filters = JSON.parse(localStorage.getItem(this.storageKey) || "[]");
      },

      onActionClicked(action, opts)
      {
        action.action(opts);
      },

      onFilterClear()
      {
        this.updateFilter(null);
      },

      onFiltered(value)
      {
        this.updateFilter(value);
      },

      openFilter(id)
      {
        let model = {
          name: null,
          filter: this.value.filter.model
        };

        if (id)
        {
          model = this.filters.find(x => x.id === id);
        }

        return Overlay.open({
          component: FilterOverlay,
          display: 'editor',
          title: 'Filter',
          defaults: this.value.filter.model,
          model: model,
          fields: this.value.filter.fields,
          canSave: !!this.alias,
          isCreate: !id
        }).then(value =>
        {
          if (value.remove && id)
          {
            this.removeFilter(id);
            return;
          }

          if (!!value.filterName)
          {
            id = this.saveFilter(value.filterName, value.model, id);
          }

          this.updateFilter({
            id: id,
            name: value.filterName,
            filter: value.model
          });
        });
      },


      saveFilter(name, filter, id)
      {
        id = id || Strings.guid();

        let savedFilter = this.filters.find(x => x.id === id);
        let index = this.filters.indexOf(savedFilter);

        let model = {
          id: id,
          name: name,
          filter: filter
        };

        if (index > -1)
        {
          this.filters.splice(index, 1, model);
        }
        else
        {
          this.filters.push(model);
        }

        localStorage.setItem(this.storageKey, JSON.stringify(this.filters));

        return id;
      },


      removeFilter(id)
      {
        let savedFilter = this.filters.find(x => x.id === id);
        let index = this.filters.indexOf(savedFilter);
        this.filters.splice(index, 1);
        localStorage.setItem(this.storageKey, JSON.stringify(this.filters));

        if (this.filter && this.filter.id === id)
        {
          this.updateFilter(null);
        }
      },


      updateFilter(value)
      {
        if (!value)
        {
          this.filter = null;
          this.$emit('filter', null);
        }
        else
        {
          this.filter = JSON.parse(JSON.stringify(value));
          this.$emit('filter', this.filter.filter);
        }
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