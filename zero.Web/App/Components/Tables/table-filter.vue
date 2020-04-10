<template>
  <div class="ui-table-filter">
    <ui-button type="light" label="Filter" caret="down" />
    <input type="search" class="ui-input" maxlength="120" v-localize:placeholder="'@login.fields.email_placeholder'" />
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