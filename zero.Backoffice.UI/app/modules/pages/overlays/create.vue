<template>
  <div class="page-create">
    <h2 class="ui-headline" v-localize="'@page.create.title'"></h2>
    <div v-if="!loading">
      <div v-if="flavors.length && config.model.parent" class="page-create-parent">
        <span v-localize="'@page.create.parent'"></span>: <strong>{{config.model.parent.name}}</strong>
      </div>
      <div class="page-create-items">
        <button type="button" v-for="item in flavors" class="page-create-item" @click="onSelect(item)">
          <ui-icon class="page-create-item-icon" :symbol="item.icon" :size="22" />
          <span class="page-create-item-text">
            <ui-localize :value="item.name" />
            <span class="page-create-item-description" v-if="item.description" v-localize="item.description"></span>
          </span>
        </button>
      </div>
      <ui-message type="error" v-if="!flavors.length" text="@page.create.nonavailable" />
      <div class="app-confirm-buttons">
        <ui-button type="light" label="@ui.close" @click="config.close"></ui-button>
      </div>
    </div>
  </div>
</template>


<script lang="ts">
  import api from '../api';

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
      loading: true,
      item: {},
      disabled: false,
      flavors: []
    }),

    created()
    {
      this.model.parentId = this.config.model.parent ? this.config.model.parent.id : null;
    },

    mounted()
    {
      api.getAllowedFlavors(this.model.parentId || 'root').then(response =>
      {
        this.flavors = response.data;
        this.loading = false;
      });
    },

    methods: {
      onSelect(item)
      {
        this.config.confirm(item);
      },
    }
  }
</script>

<style lang="scss">
  .page-create
  {
    text-align: left;

    .ui-message
    {
      margin: 0;
    }
  }

  .page-create-parent
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

  .page-create-items
  {
    margin: 0 -16px;
    margin-top: var(--padding);
    max-height: 490px;
    overflow-y: auto;
  }

  .page-create-item
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

    & + .page-create-item
    {
      margin-top: 2px;
    }
  }

  .page-create-item-text
  {
    display: flex;
    flex-direction: column;
  }

  .page-create-item-description
  {
    color: var(--color-text-dim);
    margin-top: 3px;
  }

  .page-create-item-icon
  {
    position: relative;
    top: -2px;
    left: 4px;
    color: var(--color-text);
  }
</style>