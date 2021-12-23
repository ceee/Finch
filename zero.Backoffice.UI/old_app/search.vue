<template>
  <div class="app-search">
    <form class="app-search-form" @submit.prevent="onSubmit">
        <input ref="input" class="app-search-form-input" type="search" v-model="query" placeholder="Search..." />
        <ui-button class="app-search-submit" :submit="true" type="blank" icon="fth-search" />
      </form>
      <div v-if="list.items.length" class="app-search-items">
        <ui-link :to="item.url" v-for="item in list.items" :key="item.id" class="app-search-item" @click.native="config.close">
          <ui-icon :symbol="item.icon" :size="18" class="app-search-item-icon" />
          <span class="app-search-item-text">
            <span class="app-search-item-name">{{item.name}} <span class="app-search-item-group" v-localize="item.group"></span></span>
            <span class="app-search-item-description" v-if="item.description">{{item.description}}</span>
          </span>
        </ui-link>
      </div>
  </div>
</template>


<script>
  import { map as _map, find as _find } from 'underscore';
  import SearchApi from 'zero/api/search.js';

  export default {

    props: {
      model: Object,
      config: Object
    },

    name: 'app-search',

    data: () => ({
      open: false,
      query: null,
      list: {
        page: 1,
        totalPages: 1,
        items: []
      }
    }),

    mounted()
    {
      this.$nextTick(() =>
      {
        this.$refs.input.focus();
        //this.$refs.input.select();
      });
    },

    methods: {
      async onSubmit()
      {
        this.list = await SearchApi.query(this.query);
        console.info(this.list);
      }
    }
  }
</script>

<style lang="scss">
  .app-search-overlay .app-overlay
  {
    padding: var(--padding);
  }

  .app-search-dialog
  {
    
  }

  .app-search-form
  {  
    position: relative;
  }

  input.app-search-form-input
  {
    padding-right: 48px;
  }

  .app-search-submit
  {
    position: absolute;
    right: 0;
    height: 100%;
    width: 48px;
    justify-content: center;
  }

  .app-search-items
  {
    margin-top: var(--padding-s);
    font-size: var(--font-size);
    max-height: 490px;
    overflow-y: auto;
  }

  .app-search-item
  {
    display: grid;
    width: 100%;
    grid-template-columns: 26px 1fr auto;
    gap: 12px;
    align-items: center;
    position: relative;
    color: var(--color-text);
    padding: var(--padding-xs);
    border-radius: var(--radius); 

    &:hover, &:focus
    {
      background: var(--color-tree-selected);
    }

    & + .app-search-item
    {
      margin-top: 2px;
    }
  }

  .app-search-item-text
  {
    display: flex;
    flex-direction: column;
    position: relative;
    top: 1px;
  }

  .app-search-item-name
  {
    font-size: var(--font-size);
    /*display: flex;
    flex-direction: row;
    align-items: center;
    flex-wrap: wrap;*/
  }

  .app-search-item-description
  {
    color: var(--color-text-dim);
    margin-top: 3px;
  }

  .app-search-item-group
  {
    display: block;
    font-size: var(--font-size-xs);
    color: var(--color-text-dim);
    margin-top: 3px;
    //margin-left: 8px;
  }

  .app-search-item-icon
  {
    position: relative;
    top: -1px;
    left: 4px;
    color: var(--color-text); 
  }
</style>