<template>
  <div class="root">
    countries óÒ<br />
    <ui-search v-model="search" @submit="doSearch" />
    <div v-for="item in items" :key="item.id">
      {{item.name}}
    </div> 
  </div>
</template>


<script lang="ts">
  import api from './api';

  export default {
    data: () => ({
      items: [],
      search: null
    }),

    methods: {
      async doSearch(val)
      {
        const result = await api.getByQuery({ search: val });
        this.items = result.items;
      }
    }
  }

</script>