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
  }
</style>