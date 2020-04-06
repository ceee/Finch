<template>
  <div class="page-container">
    <div class="page-container-tree">tree</div>
    <div></div>
  </div>
</template>


<script>
  export default {
    name: 'app-page',

    mounted ()
    {
      
    },

    methods: {

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