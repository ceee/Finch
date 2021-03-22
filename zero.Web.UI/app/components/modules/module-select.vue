<template>
  <div class="ui-modules-select">
    <h2 class="ui-headline">Add module</h2>
    <div v-if="!loading">
      <div class="ui-modules-select-items">
        <button type="button" v-for="item in types" class="ui-modules-select-item" @click="onSelect(item)">
          <ui-icon class="ui-modules-select-item-icon" :symbol="item.icon" :size="22" />
          <span class="ui-modules-select-item-text">
            <ui-localize :value="item.name" />
            <span v-if="item.description" v-localize="item.description"></span>
          </span>
        </button>     
      </div>
      <ui-message type="error" v-if="!types.length" text="@page.create.nonavailable" />
      <div class="app-confirm-buttons">
        <ui-button type="light" :label="config.closeLabel" @click="config.close"></ui-button>
      </div>
    </div>
  </div>
</template>


<script>
  import ModulesApi from 'zero/api/modules.js';
  import Overlay from 'zero/helpers/overlay.js';

  export default {

    props: {
      config: Object
    },

    data: () => ({
      model: {
        name: null,
        parentId: null,
        pageTypeAlias: null
      },
      loading: false,
      item: {},
      disabled: false,
      types: []
    }),


    created()
    {
      this.types = this.config.types;
      this.model.parentId = this.config.parent ? this.config.parent.id : null;
    },


    methods: {
      onSelect(item)
      {
        //this.config.close();
        this.config.confirm(item);
        //this.$router.push({
        //  name: 'ui-modules-select',
        //  params: { type: item.alias, parent: this.model.parentId }
        //});
      },
    }
  }
</script>

<style lang="scss">
  .ui-modules-select
  {
    text-align: left;

    .ui-message
    {
      margin: 0;
    }
  }

  .ui-modules-select-parent
  {
    margin: 30px 0 -10px 0; 
    border-radius: var(--radius);
    /*border: 1px solid var(--color-line-light);*/
    background: var(--color-box-nested);
    line-height: 1.4;
    color: var(--color-text-dim);
    padding: 14px 16px;
    font-size: var(--font-size);

    strong
    {
      color: var(--color-text);
    }
  }

  .ui-modules-select-items
  {
    margin: 0 -16px;
    margin-top: var(--padding);
    max-height: 600px;
    overflow-y: auto;
  }

  .ui-modules-select-item
  {
    display: grid;
    width: 100%;
    grid-template-columns: 40px 1fr auto;
    gap: 12px;
    align-items: center;
    position: relative;
    color: var(--color-text);
    padding: 16px;
    border-radius: var(--radius); 

    &:hover, &:focus
    {
      background: var(--color-tree-selected);
    }

    & + .ui-modules-select-item
    {
      margin-top: 5px;
    }
  }

  .ui-modules-select-item-text
  {
    display: flex;
    flex-direction: column;

    span
    {
      color: var(--color-text-dim);
      margin-top: 3px;
    }
  }

  .ui-modules-select-item-icon
  {
    position: relative;
    top: -2px;
    left: 4px;
    color: var(--color-text);
  }
</style>