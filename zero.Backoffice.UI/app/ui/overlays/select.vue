<template>
  <div class="ui-select-overlay">
    <!--<h2 class="ui-headline" v-localize="configuration.title"></h2>-->
    <ui-loading v-if="loading" />
    <div v-if="!loading">
      <!--<div v-if="list.length && configuration.parent" class="ui-select-overlay-parent">
        <span v-localize="title"></span>: <strong>{{configuration.parent.name}}</strong>
      </div>-->
      <div class="ui-select-overlay-items">
        <button type="button" v-for="item in list" class="ui-select-overlay-item" @click="onSelect(item)">
          <ui-icon class="ui-select-overlay-item-icon" :symbol="item.icon || 'fth-box'" :size="22" />
          <span class="ui-select-overlay-item-text">
            <strong class="-title"><ui-localize :value="item.name" /></strong>
            <span class="-desc" v-if="item.description" v-localize="item.description"></span>
          </span>
        </button>
      </div>
      <ui-message type="error" v-if="!list.length" text="@page.create.nonavailable" />
      <!--<div class="app-confirm-buttons">
        <ui-button type="light" label="@ui.close" @click="config.close"></ui-button>
      </div>-->
    </div>
  </div>
</template>


<script>
  export default {

    props: {
      model: Object,
      config: Object
    },

    data: () => ({
      list: [],
      loading: false,
      disabled: false
    }),


    mounted()
    {
      this.loading = true;
      this.load();
    },


    methods: {
      load()
      {
        if (typeof this.model.items === 'function')
        {
          this.model.items().then(res =>
          {
            this.list = res;
            this.loading = false;
          });
        }
        else
        {
          this.list = JSON.parse(JSON.stringify(this.model.items));
          this.loading = false;
        }
      },

      onSelect(item)
      {
        this.config.confirm(item);
      },
    }
  }
</script>

<style lang="scss">
  .ui-select-overlay
  {
    text-align: left;
    margin-top: -8px;
    
    .ui-message
    {
      margin: 0;
    }
  }

  .ui-select-overlay-parent
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

  .ui-select-overlay-items
  {
    margin: 0 0;
    //margin-top: var(--padding);
    max-height: 490px;
    overflow-y: auto;
  }

  .ui-select-overlay-item
  {
    display: grid;
    width: 100%;
    grid-template-columns: 70px 1fr auto;
    gap: 12px;
    align-items: center;
    position: relative;
    color: var(--color-text);
    padding: 0;
    border-radius: var(--radius);

    /*&:hover, &:focus
    {
      background: var(--color-tree-selected);
    }*/

    & + .ui-select-overlay-item
    {
      margin-top: var(--padding-s);
    }
  }

  .ui-select-overlay-item-text
  {
    display: flex;
    flex-direction: column;

    .-desc
    {
      color: var(--color-text-dim);
      margin-top: 3px;
    }
  }

  .ui-select-overlay-item-icon
  {
    color: var(--color-text);
    width: 60px;
    height: 60px;
    padding: 20px;
    border-radius: var(--radius);
    background: var(--color-bg-shade-3);
  }
</style>