<template>
  <form enctype="multipart/form-data" class="media-overview-drop-outer" dropzone=""
        @dragenter.prevent="onDragEnter" @dragleave.prevent="onDragLeave" @dragover.prevent="" @drop.prevent.stop="onDrop($event.dataTransfer.files)">
    <div class="media-overview-drop" :class="{ 'is-dragging': dragging }">
      <ui-icon symbol="fth-upload" :size="26" :stroke-width="2" />
      <input type="file" multiple class="-input" @change="onDrop($event.target.files)" />
    </div>
  </form>
</template>

<script lang="ts">
  import * as overlays from '../../../../services/overlay';

  export default {
    name: 'mediaOverviewDrop',

    props: {
      folderId: {
        type: String,
        required: false
      }
    },

    data: () => ({
      dragging: false
    }),


    methods: {

      async onDrop(files)
      {
        this.dragging = false;

        const result = await overlays.open({
          component: () => import('../../overlays/upload-status.vue'),
          width: 420,
          softdismiss: false,
          model: {
            files,
            folderId: this.folderId
          }
        });

        if (result.eventType === 'confirm')
        {
          this.$emit('completed', files);
        }
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
    position: relative;
    cursor: pointer;

    &.is-dragging
    {
      border-color: var(--color-accent);
    }

    .-input
    {
      position: absolute;
      left: 0;
      right: 0;
      top: 0;
      bottom: 0;
      height: 130px;
      opacity: 0;
    }

    input[type=file],
    input[type=file]::-webkit-file-upload-button
    {
      cursor: pointer;
    }
  }

  .ui-datagrid-outer.is-selecting .media-overview-drop
  {
    pointer-events: none;
    opacity: .55;
  }
</style>
