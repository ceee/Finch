<template>
  <ui-overlay-editor class="pages-recyclebin-actions">
    <template v-slot:header>
      <ui-header-bar :title="model.name" :back-button="false" :close-button="true" />
    </template>
    <template v-slot:footer>
      <ui-button type="white" label="@ui.close" @click="config.hide"></ui-button>
    </template>

    <button class="pages-recyclebin-action">
      <i class="pages-recyclebin-action-icon fth-rotate-ccw"></i>
      <p class="pages-recyclebin-action-text">
        <strong>Restore page</strong>
        <span>Moves this page back into the page tree.</span>
      </p>
    </button>

    <button class="pages-recyclebin-action" v-if="model.operationId">
      <i class="pages-recyclebin-action-icon fth-zap"></i>
      <p class="pages-recyclebin-action-text">
        <strong>Undo operation</strong>
        <span>Restores all {{operationCount}} pages from the affected operation.</span>
      </p>
    </button>

    <button class="pages-recyclebin-action">
      <i class="pages-recyclebin-action-icon fth-trash is-negative"></i>
      <p class="pages-recyclebin-action-text">
        <strong>Delete page...</strong>
        <span>Remove this recycled page forever.</span>
      </p>
    </button>
  </ui-overlay-editor>
</template>


<script>
  import RecycleBinApi from 'zero/resources/recycle-bin.js'
  import PageTreeApi from 'zero/resources/page-tree.js'
  import PagesApi from 'zero/resources/pages';
  import Arrays from 'zero/services/arrays.js'

  export default {

    props: {
      isCopy: {
        type: Boolean,
        default: false
      },
      model: Object,
      config: Object
    },

    data: () => ({
      state: 'default',
      operationCount: 0
    }),


    mounted()
    {
      RecycleBinApi.getCountByOperation(this.model.operationId).then(count =>
      {
        this.operationCount = count;
      });
    },


    methods: {

      onSelect(item)
      {
        
      },

      onSave()
      {
        this.state = 'loading';

        PagesApi.copy(this.model.id, this.selected.id, this.includeDescendants).then(res =>
        {
          if (res.success)
          {
            this.state = 'success';
            this.config.confirm(res.model);
          }
          else
          {
            this.state = 'error';
          }
        });
      }
    }
  }
</script>

<style lang="scss">
  .pages-recyclebin-action
  {
    display: grid;
    grid-template-columns: auto 1fr;
    grid-gap: var(--padding);
    align-items: center;
    width: 100%;
    background: var(--color-bg-bright);
    border-radius: var(--radius);
    padding: 16px;

    & + .pages-recyclebin-action
    {
      margin-top: 16px;
    }

    /*&:hover
    {
      background: var(--color-bg-bright-two);
    }*/
  }

  .pages-recyclebin-action-text
  {
    margin: 16px 0;
    line-height: 1.5;

    strong
    {
      display: block;
      margin-bottom: 5px;
    }

    span
    {
      color: var(--color-fg-dim);
    }
  }

  .pages-recyclebin-action-icon
  {
    font-size: 18px;
    margin-right: -6px;
    align-self: stretch;
    background: var(--color-bg-dim);
    padding: 10px;
    width: 50px;
    display: flex;
    justify-content: center;
    align-items: center;
    border-radius: var(--radius);
    color: var(--color-fg);

    &.is-negative
    {
      color: var(--color-accent-error);
    }
  }

  .pages-recyclebin-actions .ui-property
  {
    display: flex;
    justify-content: space-between;
  }

  .pages-recyclebin-actions .ui-property-content
  {
    display: inline;
    flex: 0 0 auto;
  }

  .pages-recyclebin-actions .ui-property-label
  {
    padding-top: 1px;
  }

  .pages-recyclebin-actions .ui-box
  {
    margin: 0;    
    padding: 20px var(--padding) 18px;

    & + .ui-box
    {
      padding: 16px 0;
      margin-top: 26px;
    }

    .ui-tree-item.is-disabled
    {
      opacity: .5;
    }

    .ui-tree-item.is-selected, .ui-tree-item:hover:not(.is-disabled)
    {
      background: var(--color-bg-bright-two);
    }

    .ui-tree-item.is-selected
    {
      &:after
      {
        font-family: "Feather";
        content: "\e83e";
        font-size: 16px;
        color: var(--color-primary);
      }
      
      .ui-tree-item-text
      {
        font-weight: bold;
      }
    }
  }

  .pages-recyclebin-actions content
  {
    padding-top: 0;
  }

  .pages-copy-text
  {
    margin: 0 0 20px;
  }
</style>