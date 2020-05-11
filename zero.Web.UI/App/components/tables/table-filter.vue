<template>
  <div class="ui-table-filter">
    <ui-button v-if="!hideFilter" type="light" label="Filter" caret="down" />
    <ui-search v-if="!hideSearch" v-model="value.search" />
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
            search: null
          }
        }
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
      hideSearch: true
    }),

    created()
    {
      this.reload();
    },

    methods: {

      reload()
      {
        this.hideFilter = typeof this.value.filter === 'undefined';
        this.hideSearch = typeof this.value.search === 'undefined';
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