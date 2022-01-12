<template>
  <div class="ui-modules-select">
    <h2 class="ui-headline">Add module</h2>
    <div v-if="!loading">
      <div class="ui-modules-select-items">
        <button type="button" v-for="item in types" class="ui-modules-select-item" @click="onSelect(item)">
          <div class="ui-modules-select-item-top">
            <ui-icon class="ui-modules-select-item-icon" :symbol="item.icon" :size="22" />
          </div>
          <span class="ui-modules-select-item-text">
            <ui-localize :value="item.name" class="-headline" />
            <span v-if="item.description" class="-desc" v-localize="item.description"></span>
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
  .app-overlay[data-alias="modules-select"]
  {
    width: calc(100vw - 40px);
    max-width: 1080px;
    height: calc(100vh - 40px);
    max-height: 750px;
  }

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
    display: grid;
    grid-template-columns: repeat(auto-fit, minmax(180px, 1fr));
    grid-gap: var(--padding-m);
    margin: 0 -16px;
    padding: 0 16px;
    margin-top: var(--padding);
    max-height: 550px;
    overflow-y: auto;
  }

  .ui-modules-select-item
  {
    display: inline-flex;
    flex-direction: column;
    width: 100%;
    grid-template-columns: 40px 1fr auto;
    gap: 12px;
    align-items: stretch;
    position: relative;
    color: var(--color-text);
    /*&:hover, &:focus
    {
      background: var(--color-tree-selected);
    }*/
    &:hover .ui-modules-select-item-top
    {
      border: 1px solid var(--color-bg-shade-5);
      background: var(--color-bg-shade-1);
      outline: none;
    }
  }

  .ui-modules-select-item-top
  {
    background: var(--color-bg-shade-2);
    height: 100px;
    border-radius: var(--radius);
    display: inline-flex;
    justify-content: center;
    align-items: center;
    border: 1px solid var(--color-line);
  }

  .ui-modules-select-item-text
  {
    display: flex;
    flex-direction: column;

    .-desc
    {
      color: var(--color-text-dim);
      margin-top: 3px;
      font-size: var(--font-size-s);
      line-height: 1.4;
    }

    .-headline
    {
      color: var(--color-text);
      font-weight: 700;
    }
  }

  .ui-modules-select-item-icon
  {
    position: relative;
    font-size: var(--size-xl);
    color: var(--color-text);
  }
</style>