<template>
  <div class="page-container">
    <page-tree />
  </div>
</template>

<script lang="ts">
  import api from './api';
  import { defineComponent } from 'vue';
  import PageTree from './partials/page-tree.vue';

  export default defineComponent({

    components: { PageTree },

  });
</script>

<style lang="scss">
  .page-container
  {
    display: grid;
    grid-template-columns: auto 1fr;
    gap: 2px;
    justify-content: stretch;
    height: calc(100vh - 70px);
  }


  .page-overview
  {
    display: flex;
    flex-direction: column;
    margin-left: 80px;
    padding-top: 115px;
  }

  .page-overview-action
  {
    color: var(--color-text);
    font-size: var(--font-size);
    display: grid;
    grid-template-columns: auto 1fr;
    gap: 35px;
    align-items: center;
  }

  .page-overview-action + .page-overview-action
  {
    margin-top: var(--padding);
  }

  .page-overview-action-icon
  {
    display: inline-flex;
    justify-content: center;
    align-items: center;
    width: 90px;
    height: 90px;
    background: var(--color-box);
    border-radius: var(--radius);
    box-shadow: var(--shadow-short);
  }

  .page-overview-action-text
  {
    line-height: 1.3;
    color: var(--color-text-dim);
  }

  .page-overview-action-text strong
  {
    display: inline-block;
    margin-bottom: 8px;
    color: var(--color-text);
    font-size: var(--font-size-l);
  }
</style>