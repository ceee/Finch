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
        <ui-dropdown>
          <template v-slot:button>
            <ui-button type="light" label="Actions" caret="down" />
          </template>
          <ui-dropdown-list :items="actions" :action="actionSelected" />
        </ui-dropdown>
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
      },
      actions: []
    }),


    created()
    {
      this.actions.push({
        name: 'Create',
        icon: 'fth-plus'
      });
      this.actions.push({
        name: 'Move',
        icon: 'fth-corner-down-right'
      });
      this.actions.push({
        name: 'Copy',
        icon: 'fth-copy',
        disabled: true
      });
      this.actions.push({
        name: 'Sort',
        icon: 'fth-shuffle'
      });
      this.actions.push({
        type: 'separator'
      });
      this.actions.push({
        name: 'Delete',
        icon: 'fth-x',
        action(item, dropdown)
        {
          dropdown.hide();
        }
      });
    },


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

      actionSelected(item, dropdown)
      {
        dropdown.hide();
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