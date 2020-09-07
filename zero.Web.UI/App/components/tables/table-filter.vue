<template>
  <div class="ui-table-filter">
    <ui-search v-if="!hideSearch" v-model="value.search" class="onbg" />
    <ui-button v-if="!hideFilter" type="light onbg" label="Filter" caret="down" />
    <ui-dropdown v-if="!hideSelection && selection.length > 0" align="right">
      <template v-slot:button>
        <ui-button type="light" :label="selectedText" caret="down" />
      </template>
      <slot name="actions"></slot>
    </ui-dropdown>
  </div>
</template>


<script>
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
      hideSelection: true
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
        this.hideFilter = typeof this.value.filter === 'undefined';
        this.hideSearch = typeof this.value.search === 'undefined';
        this.hideSelection = this.value.selectable !== true;
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