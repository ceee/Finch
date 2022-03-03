<template>
  <div v-if="loaded" class="ui-table-filter">
    <ui-search v-if="!hideSearch" v-model="attach.query.search" class="onbg" />

    <ui-dropdown v-if="hasFilter && storedFilters.length" align="right">
      <template v-slot:button>
        <ui-button type="light onbg" :label="filterLabel" :tokens="{ name: currentFilter ? currentFilter.name : null }" caret="down" />
      </template>
      <ui-dropdown-button v-if="storedFilters.length" v-for="(filter, index) in storedFilters" :key="index" :value="filter" :label="filter.name" @click="setFilter" />
      <ui-dropdown-separator v-if="storedFilters.length" />
      <ui-dropdown-button label="@ui.add" icon="fth-plus" @click="addOrEditFilter()" />
      <ui-dropdown-button v-if="currentFilter" label="@listfilter.button_edit" icon="fth-edit-2" @click="addOrEditFilter(currentFilter.id)" />
      <ui-dropdown-button v-if="currentFilter" label="@listfilter.button_clear" icon="fth-x" @click="setFilter(null)" />
    </ui-dropdown>
    <ui-button v-if="hasFilter && !storedFilters.length" type="light onbg" label="@listfilter.button" @click="addOrEditFilter()" />

    <ui-dropdown v-if="!hideSelection && selection.length > 0" align="right">
      <template v-slot:button>
        <ui-button type="light" :label="selectedText" caret="down" />
      </template>
      <slot name="actions"></slot>
    </ui-dropdown>

    <ui-dropdown v-if="actions && actions.length > 0" align="right">
      <template v-slot:button>
        <ui-button type="light onbg" icon="fth-more-horizontal" />
      </template>
      <ui-dropdown-button v-for="(action, index) in actions" :key="index" :value="action" :prevent="action.autoclose === false" :label="action.label" :icon="action.icon" @click="onActionClicked" />
    </ui-dropdown>
  </div>
</template>


<script>
  //import Overlay from 'zero/helpers/overlay.js';
  import { generateId } from '../utils/numbers';
  import * as overlays from '../services/overlay';
  //import FilterOverlay from './table-filter-overlay.vue';

  const KEY_PREFIX = 'zero.ui-table-filter.';

  export default {
    name: 'uiTableFilter',

    props: {
      attach: [Object, undefined],
      hideSearch: {
        type: Boolean,
        default: false
      }
    },

    watch: {
      attach: function (value)
      {
        this.setup();
      }
    },

    data: () => ({
      loaded: false,
      hasFilter: false,
      filterOptions: null,
      //hideFilter: true,
      hideSelection: false,
      selection: [],
      storedFilters: [],
      currentFilter: null,
      actions: []
    }),

    created()
    {
      //this.reload();
    },

    mounted()
    {
      this.setup();
    },

    computed: {
      filterLabel()
      {
        return this.currentFilter ? '@listfilter.button_selected' : '@listfilter.button';
      }
    },

    methods: {

      // attach table data
      setup()
      {
        if (!this.attach)
        {
          return;
        }

        this.filterOptions = { ...this.attach.filter };
        this.hasFilter = this.filterOptions && this.filterOptions.editor;
        this.storedFilters = this.getStoredFilters();
        this.actions = this.attach.listConfig.actions;

        this.loaded = true;
      },


      // called when an action has been clicked
      onActionClicked(action, opts)
      {
        action.call(opts, this);
      },


      // load stored filters from local storage
      getStoredFilters()
      {
        if (!this.hasFilter)
        {
          return [];
        }
        return JSON.parse(localStorage.getItem(KEY_PREFIX + this.filterOptions.editor.alias) || "[]");
      },


      // set the current filter
      setFilter(value, two)
      {
        if (!value)
        {
          this.currentFilter = null;
          this.$emit('filter', null);
          this.attach.setFilter(null);
        }
        else
        {
          this.currentFilter = JSON.parse(JSON.stringify(value));
          this.currentFilter.filter.id = this.currentFilter.id;
          this.$emit('filter', this.currentFilter.filter);
          this.attach.setFilter(this.currentFilter.filter);
        }
      },


      // removes a filter from the storage
      removeFilter(id)
      {
        let savedFilter = this.storedFilters.find(x => x.id === id);
        this.storedFilters.splice(this.storedFilters.indexOf(savedFilter), 1);
        localStorage.setItem(KEY_PREFIX + this.filterOptions.editor.alias, JSON.stringify(this.storedFilters));

        if (this.currentFilter && this.currentFilter.id === id)
        {
          this.setFilter(null);
        }
      },


      // persist a filter in local storage
      saveFilter(name, filter, id)
      {
        id = id || generateId(4);

        let savedFilter = this.storedFilters.find(x => x.id === id);
        let index = this.storedFilters.indexOf(savedFilter);
        let model = { id, name, filter };

        if (index > -1)
        {
          this.storedFilters.splice(index, 1, model);
        }
        else
        {
          this.storedFilters.push(model);
        }

        localStorage.setItem(KEY_PREFIX + this.filterOptions.editor.alias, JSON.stringify(this.storedFilters));

        return id;
      },


      async addOrEditFilter(id)
      {
        let model = { name: null, filter: this.filterOptions.template };

        if (id)
        {
          model = this.storedFilters.find(x => x.id === id);
        }

        const result = await overlays.open({
          component: () => import('./ui-table-filter-overlay.vue'),
          display: 'editor',
          model: {
            editor: this.filterOptions.editor,
            template: this.filterOptions.template,
            value: model,
            isCreate: !id
          }
        });

        if (result.eventType == 'confirm')
        {
          const value = result.value;

          if (value.remove && id)
          {
            this.removeFilter(id);
            return;
          }

          if (value.filterName)
          {
            id = this.saveFilter(value.filterName, value.model, id);
          }

          this.setFilter({
            id,
            name: value.filterName,
            filter: value.model
          });
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