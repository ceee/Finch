<template>
  <form enctype="multipart/form-data" class="media-overview-drop" :class="{ 'is-dragging': dragging }" 
        @dragenter.prevent="onDragEnter" @dragleave.prevent="onDragLeave" @dragover.prevent="" @drop.prevent.stop="onDrop">
    <ui-icon symbol="fth-upload" :size="26" :stroke-width="2" />
  </form>
</template>

<script lang="ts">
  import * as overlays from '../../../../services/overlay';

  export default {
    name: 'mediaOverviewDrop',

    props: {
      
    },

    data: () => ({
      dragging: false
    }),


    methods: {

      async onDrop(ev)
      {
        console.info(ev.dataTransfer);
        this.dragging = false;

        const result = await overlays.open({
          component: () => import('../../overlays/upload-status.vue'),
          model: ev.dataTransfer.files
        });
      },

      onDragEnter(ev)
      {
        this.dragging = true;
      },

      onDrageLeave(ev)
      {
        this.dragging = false;
      }

    }
  };
</script>

<style lang="scss">
  .media-overview-drop
  {
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100%;
    min-height: 130px;
    border-radius: var(--radius);
    align-self: flex-start;
    box-shadow: var(--shadow-short);
    border: 1px dashed var(--color-line-dashed-onbg);
    color: var(--color-text);

    &.is-dragging
    {
      border-color: var(--color-accent);
    }
  }

  .ui-datagrid-outer.is-selecting .media-overview-drop
  {
    pointer-events: none;
    opacity: .55;
  }
</style>
