<template>
  <div class="page-container">
    <div class="page-container-tree">
      <ui-tree :get="getItems" />
    </div>
    <div></div>
  </div>
</template>


<script>
  import PageTreeApi from 'zeroresources/page-tree.js'

  export default {
    name: 'app-page',

    methods: {

      getItems(parent)
      {
        return PageTreeApi.getChildren(parent);
      }

    }
  }
</script>


<style lang="scss">
  .page-container
  {
    display: grid;
    grid-template-columns: auto 1fr;
    justify-content: stretch;
    height: 100%;
  }

  .page-container-tree
  {
    width: 340px;
    background: var(--color-bg-light);
    padding: var(--padding);
  }
</style>