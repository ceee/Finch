<template>
  <div class="ui-table-filter">
    <ui-button v-if="filter" type="light" label="Filter" caret="down" />
    <ui-search v-if="search" />
  </div>
</template>


<script>
  export default {
    name: 'uiTableFilter',

    props: {
      filter: {
        type: Boolean,
        default: true
      },
      search: {
        type: Boolean,
        default: true
      }
    },

    data: () => ({
      
    }),

    mounted ()
    {
      
    },

    methods: {

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