<template>
  <div class="app-search" v-if="open">
    <div class="app-search-dialog">
      <form class="app-search-form" @submit.prevent="onSubmit">
        <input class="app-search-form-input" type="search" v-model="query" placeholder="Search..." />
        <ui-button class="app-search-submit" :submit="true" type="blank" icon="fth-search" />
      </form>
      <div class="app-search-items">
        <router-link :to="item.url" v-for="item in model.items" :key="item.id" class="app-search-item">
          <ui-icon :symbol="item.icon" :size="18" class="app-search-item-icon" />
          <span class="app-search-item-text">
            <span class="app-search-item-name">{{item.name}} <span class="app-search-item-group" v-localize="item.group"></span></span>
            <span class="app-search-item-description" v-if="item.description">{{item.description}}</span>
          </span>
        </router-link>
      </div>
    </div>
  </div>
</template>


<script>
  import { map as _map, find as _find } from 'underscore';
  import SearchApi from 'zero/api/search.js';


  export default {
    name: 'app-search',

    data: () => ({
      open: false,
      query: null,
      model: {
        page: 1,
        totalPages: 1,
        items: []
      }
    }),


    methods: {

      async onSubmit()
      {
        this.model = await SearchApi.query(this.query);
        console.info(this.model);
      }
    }
  }

</script>

<style lang="scss">
  .app-search
  {
    position: fixed;
    top: 0;
    bottom: 0;
    left: 0;
    right: 0;
    background: var(--color-overlay-shade);
    z-index: 5;
    display: flex;
    justify-content: center;
    align-items: flex-start;
  }

  .app-search-dialog
  {
    background: var(--color-box);
    border-bottom-left-radius: var(--radius);
    border-bottom-right-radius: var(--radius);
    box-shadow: var(--shadow-overlay);
    width: min(100vw, 780px);
    padding: var(--padding);
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
    font-size: 9px;
    text-transform: uppercase;
    color: var(--color-text-dim-one);
    margin-top: 2px;
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