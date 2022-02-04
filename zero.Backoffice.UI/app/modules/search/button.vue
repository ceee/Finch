<template>
  <ui-button icon="fth-search" :stroke="2.5" type="blank" class="app-nav-search" @click="openSearch" />
</template>

<script>
  import * as overlays from '../../services/overlay';

  export default {
    data: () => ({
      listener: null
    }),

    mounted()
    {
      this.listener = e =>
      {
        if (e.key === "f" && (e.ctrlKey || e.metaKey))
        {
          e.preventDefault();
          this.openSearch();
        }
      };

      document.addEventListener('keydown', this.listener.bind(this));
    },

    beforeDestroy()
    {
      document.removeEventListener('keydown', this.listener);
    },

    methods: {
      async openSearch()
      {
        const result = await overlays.open({
          component: () => import('./overlay.vue'),
          autoclose: false,
          softdismiss: true,
          width: 780
          //class: 'app-search-overlay'
        });
      }
    }
  }
</script>