<template>
  <div class="media-content">
    <ui-header-bar :back-button="!!parentId" title="Media">
      <!--<template v-slot:title>
        <h2 class="ui-header-bar-title">
          <span v-for="(item, index) in hierarchy" :key="item.id" class="media-items-hierarchy-item">
            <router-link :to="{ name: 'media', params: { id: item.id } }" v-localize="item.name"></router-link>
            <ui-icon v-if="index < hierarchy.length - 1" symbol="fth-chevron-right" />
          </span>
        </h2>
      </template>-->
    </ui-header-bar>

    <div class="ui-view-box">
      <div class="media-items">
        <div class="media-items-grid">
          <media-item v-for="item in items" :key="item.id" :value="item" />
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import api from './api';
  import MediaItem from './partials/overview-item.vue';

  export default defineComponent({
    props: ['parentId'],

    components: { MediaItem },

    data: () => ({
      items: [],
      paging: {}
    }),


    watch: {
      '$route': async function (val)
      {
        await this.setup();
      }
    },


    async mounted()
    {
      await this.setup();
    },


    methods: {
      async setup()
      {
        const response = await api.getChildren(this.$route.params.parentId || 'root', {});
        this.items = response.data;
        this.paging = response.paging;
      }
    }

  });
</script>



<style lang="scss">
  .media
  {
    width: 100%;
    height: 100vh;
    overflow-y: auto;
  }

  .media-content
  {
    height: 100vh;
    overflow-y: auto;
  }

  .media-items-grid
  {
    display: grid;
    grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
    align-items: stretch;
    gap: var(--padding);
  }
</style>