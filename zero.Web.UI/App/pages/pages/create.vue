<template>
  <ui-form v-if="!loading" ref="form" class="page-create" v-slot="form" @submit="onSubmit" @load="onLoad">
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
  </ui-form>
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
      console.info(this.config.parent);
      this.model.parentId = this.config.parent ? this.config.parent.id : null;
    },


    methods: {

      onLoad(form)
      {
        form.load(PagesApi.getAllowedPageTypes(this.model.parentId)).then(response =>
        {
          this.pageTypes = response;
          this.loading = false;
        });
      },

      onSelect(item)
      {
        this.config.close();

        this.$router.push({
          name: 'page-create',
          params: { type: item.alias }
        });
      },


      onSubmit(form)
      {
        //form.handle(TranslationsApi.save(this.item)).then(response =>
        //{
        //  console.info(response);
        //});
      },

      onDelete()
      {
        //Overlay.confirmDelete().then((opts) =>
        //{
        //  opts.state('loading');

        //  TranslationsApi.delete(this.model.id).then(response =>
        //  {
        //    if (response.success)
        //    {
        //      opts.state('success');
        //      opts.hide();
        //      this.config.close();
        //    }
        //    else
        //    {
        //      opts.errors(response.errors);
        //    }
        //  });
        //}); 
      }

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
      color: var(--color-primary);

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