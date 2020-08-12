<template>
  <div v-if="!loading" class="page-create">
    <h2 class="ui-headline">Create page</h2>
    <div class="page-create-parent" v-if="config.parent">
      Parent: <strong>{{config.parent.name}}</strong>
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
  }

  .page-create-parent
  {
    margin: 30px 0 -10px 0;
    border-radius: var(--radius);
    /*border: 1px solid var(--color-line-light);*/
    background: var(--color-bg-xlight);
    line-height: 1.4;
    color: var(--color-fg-mid);
    padding: 14px 12px;
    font-size: var(--font-size);

    strong
    {
      color: var(--color-fg);
    }
  }

  .page-create-items
  {
    margin-top: var(--padding);
    max-height: 600px;
    overflow-y: auto;
  }

  .page-create-item
  {
    display: grid;
    grid-template-columns: 30px 1fr auto;
    grid-gap: 6px;
    align-items: center;
    position: relative;
    color: var(--color-fg);
    padding: 13px 0;

    &.is-active
    {
      font-weight: bold;
      color: var(--color-secondary);

      .page-create-item-text span
      {
        font-weight: 400;
      }
    }
  }

  .page-create-item-text
  {
    display: flex;
    flex-direction: column;

    span
    {
      color: var(--color-fg-light);
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
    color: var(--color-fg-reverse-mid);
    transition: color 0.2s ease;
  }
</style>