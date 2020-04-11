<template>
  <div class="ui-table-filter">
    <ui-button type="light" label="Filter" caret="down" />
    <ui-search />
  </div>
</template>


<script>
  export default {
    name: 'uiTableFilter',

    props: {
      
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