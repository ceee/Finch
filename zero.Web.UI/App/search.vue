<template>
  <div class="app-search" v-if="open">
    <div class="app-search-dialog">
      <form class="app-search-form" @submit.prevent="onSubmit">
        <input type="search" v-model="query" placeholder="Search..." />
        <ui-button :submit="true" type="primary" label="Go" />
      </form>
      <div class="app-search-items">
        <div v-for="item in model.items" :key="item.id" class="page-create-item">
          <svg width="22" height="22" stroke-width="2" data-symbol="fth-folder" class="ui-icon page-create-item-icon"><use xlink:href="#fth-folder"></use></svg>
          <span class="page-create-item-text">
            <span>{{item.name}}</span>
            <span class="page-create-item-description">{{item.id}}</span>
          </span>
        </div>
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
    align-items: center;
  }

  .app-search-dialog
  {
    background: var(--color-box);
    border-radius: var(--radius);
    box-shadow: var(--shadow-overlay);
    width: 620px;
    padding: var(--padding);
  }

  .app-search-form
  {  
    display: grid;
    grid-template-columns: 1fr auto;
  }

  .app-search-items
  {
    margin-top: var(--padding-s);
    font-size: var(--font-size);
  }
</style>