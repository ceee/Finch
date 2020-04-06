<template>
  <div class="page-container">
    <div class="page-container-tree" v-resizable="{ axis: 'x', min: 260, max: 440, save: 'page-tree', handle: '.ui-resizable' }">
      <ui-tree :get="getItems" />
      <button type="button" class="page-container-tree-resizable ui-resizable"></button>
    </div>
    <div></div>
  </div>
</template>


<script>
  import PageTreeApi from 'zeroresources/page-tree.js'

  export default {
    name: 'app-page',

    data: () => ({
      cache: {}
    }),

    methods: {

      getItems(parent)
      {
        const key = !parent ? '__root' : parent;

        if (this.cache[key])
        {
          return Promise.resolve(this.cache[key]);
        }

        return PageTreeApi.getChildren(parent).then(response =>
        {
          this.cache[key] = response;
          return response;
        });
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
    padding: var(--padding) 0;
    position: relative;
  }

  .page-container-tree-resizable
  {
    position: absolute;
    top: 0;
    bottom: 0;
    background: var(--color-fg);
    opacity: 0;
    right: 0;
    width: 6px;
    cursor: ew-resize;
    transition: opacity 0.15s ease 0s;

    &:hover
    {
      transition-delay: 0.2s;
      opacity: 0.04;
    }
  }
</style>