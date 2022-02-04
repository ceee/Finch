<template>
  <ui-button icon="fth-search" :stroke="2.5" type="blank" class="app-nav-search" @click="openSearch" />
</template>

<script>
  import * as overlays from '../../services/overlay';
  import { useSearchStore } from './store';

  export default {
    data: () => ({
      open: false,
      listener: null,
      store: null
    }),

    mounted()
    {
      this.store = useSearchStore();

      this.listener = e =>
      {
        if (this.store.shortcutEnabled() && e.key === "f" && (e.ctrlKey || e.metaKey) && !this.open)
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
        this.open = true;

        const result = await overlays.open({
          component: () => import('./overlay.vue'),
          autoclose: false,
          softdismiss: true,
          width: 780,
          class: 'app-search-overlay'
        });

        this.open = false;
      }
    }
  }
</script>