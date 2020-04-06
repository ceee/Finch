<template>
  <div class="page-container">
    <div class="page-container-tree" v-resizable="resizable">
      <ui-header-bar title="Pages">
        <ui-dot-button />
      </ui-header-bar>
      <ui-tree :get="getItems" />
      <div class="page-container-tree-resizable ui-resizable"></div>
    </div>
    <div>
      <ui-header-bar title="Checkout" :on-back="onBack">
        <ui-button type="light" label="Actions" caret="down" />
        <ui-button type="light" label="Preview" icon="fth-eye" />
        <ui-button label="Save" />
      </ui-header-bar>
    </div>
  </div>
</template>


<script>
  import PageTreeApi from 'zeroresources/page-tree.js'

  export default {
    name: 'app-page',

    data: () => ({
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
          this.cache[key] = response;
          return response;
        });
      },

      onBack()
      {
        console.info('back');
      }

    }
  }
</script>


<style lang="scss">
  .page-container
  {
    display: grid;
    grid-template-columns: auto 1fr;
    grid-gap: 1px;
    justify-content: stretch;
    height: 100%;
  }

  .page-container-tree
  {
    width: 340px;
    background: var(--color-bg-light);
    padding: 0;
    position: relative;

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