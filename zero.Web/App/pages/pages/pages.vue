<template>
  <div class="page-container">
    <div class="page-container-tree" v-resizable="resizable">
      <ui-header-bar title="Pages">
        <ui-dot-button />
      </ui-header-bar>
      <ui-tree :get="getItems" />
      <div class="page-container-tree-resizable ui-resizable"></div>
    </div>
    <router-view></router-view>
  </div>
</template>


<script>
  import PageTreeApi from 'zero/resources/page-tree.js'

  export default {

    data: () => ({
      page: true,
      cache: {},
      resizable: {
        axis: 'x',
        min: 260,
        max: 520,
        save: 'page-tree',
        handle: '.ui-resizable'
      }
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
          response.forEach(item =>
          {
            item.url = {
              name: 'page',
              params: { id: item.id }
            }
          });
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
    grid-gap: 2px;
    justify-content: stretch;
    height: 100vh;
  }

  .page-container-tree
  {
    width: 340px;
    background: var(--color-bg-light);
    padding: 0;
    position: relative;
    overflow-y: auto;
    height: 100vh;

    .ui-header-bar + .ui-tree
    {
      margin-top: 2px;
    }

    .ui-dot-button
    {
      margin-right: -8px;
    }
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