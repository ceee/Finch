<template>
  <hr class="ui-dropdown-separator">
</template>


<script>
  export default {
    name: 'uiDropdownSeparator'
  }
</script>


<style lang="scss">
  .ui-dropdown-separator
  {
    border: none;
    border-bottom: 1px solid var(--color-dropdown-line);
    margin: 5px 0;
  }

  .ui-dropdown-separator + .ui-dropdown-separator
  {
    display: none;
  }
</style>