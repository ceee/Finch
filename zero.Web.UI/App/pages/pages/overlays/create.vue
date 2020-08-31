<template>
  <div v-if="!loading" class="page-create">
    <h2 class="ui-headline" v-localize="'@page.create.title'"></h2>
    <div v-if="pageTypes.length && config.parent" class="page-create-parent">
      <span v-localize="'@page.create.parent'"></span>: <strong>{{config.parent.name}}</strong>
    </div>
    <div class="page-create-items">
      <button type="button" v-for="item in pageTypes" class="page-create-item" @click="onSelect(item)">
        <i class="page-create-item-icon" :class="item.icon"></i>
        <span class="page-create-item-text">
          {{item.name | localize}}
          <span v-if="item.description" v-localize="item.description"></span>
        </span>
      </button>     
    </div>
    <ui-message type="error" v-if="!pageTypes.length" text="@page.create.nonavailable" />
    <div class="app-confirm-buttons">
      <ui-button type="light" :label="config.closeLabel" :disabled="loading" @click="config.close"></ui-button>
    </div>
  </div>
</template>


<script>
  import PagesApi from 'zero/resources/pages.js';
  import Overlay from 'zero/services/overlay.js';

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
      pageTypes: []
    }),


    created()
    {
      this.model.parentId = this.config.parent ? this.config.parent.id : null;
    },


    mounted()
    {
      PagesApi.getAllowedPageTypes(this.model.parentId).then(response =>
      {
        this.pageTypes = response;
        this.loading = false;
      });
    },


    methods: {
      onSelect(item)
      {
        this.config.close();

        this.$router.push({
          name: 'page-create',
          params: { type: item.alias, parent: this.model.parentId }
        });
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
    background: var(--color-bg-dim);
    line-height: 1.4;
    color: var(--color-fg-dim);
    padding: 14px 16px;
    font-size: var(--font-size);

    strong
    {
      color: var(--color-fg);
    }
  }

  .page-create-items
  {
    margin: 0 -16px;
    margin-top: var(--padding);
    max-height: 600px;
    overflow-y: auto;
  }

  .page-create-item
  {
    display: grid;
    width: 100%;
    grid-template-columns: 30px 1fr auto;
    gap: 6px;
    align-items: center;
    position: relative;
    color: var(--color-fg);
    padding: 16px;
    border-radius: var(--radius); 

    &:hover, &:focus
    {
      background: var(--color-bg-bright-two);
    }
  }

  .page-create-item-text
  {
    display: flex;
    flex-direction: column;

    span
    {
      color: var(--color-fg-dim);
      margin-top: 3px;
    }
  }

  .page-create-item-icon
  {
    font-size: 18px;
    line-height: 1;
    font-weight: 400;
    position: relative;
    top: -2px;
    color: var(--color-fg);
    transition: color 0.2s ease;
  }
</style>