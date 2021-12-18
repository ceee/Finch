<template>
  <div class="ui-media-upload">
    <h2 class="ui-headline" v-localize="'@media.upload.headline'"></h2>

    <div class="ui-media-upload-progress">
      <ui-progress :value="progress" :animated="!finished" />
      <p class="ui-media-upload-progress-text" v-if="!finished">Uploading {{completed + 1}} of {{fileCount}} ...</p>
      <p class="ui-media-upload-progress-text" v-else>Completed</p>
      <!--<upload-status-item :file="model[4]" />-->
    </div>

    <!--<div class="ui-media-upload-items">
      <upload-status-item v-for="file in model.files" :file="file" />
    </div>-->

    <div class="app-confirm-buttons" v-if="finished && hasErrors">
      <ui-button type="light" label="@ui.close" :disabled="loading" @click="config.confirm(null)"></ui-button>
    </div>
  </div>
</template>


<script lang="ts">
  import api from '../api';
  import { generateId } from '../../../utils/numbers';
  import UploadStatusItem from './upload-status-item.vue';

  export default {

    props: {
      model: {
        type: Object,
        default: () => ({
          files: [],
          folderId: null
        })
      },
      config: Object
    },

    data: () => ({
      progress: 0,
      completed: 0,
      loading: false,
      items: [],
      hasErrors: false
    }),


    components: { UploadStatusItem },


    computed: {
      fileCount()
      {
        return this.model.files.length;
      },
      finished()
      {
        return this.completed >= this.fileCount;
      }
    },


    async mounted()
    {
      await this.upload();
    },

    methods: {

      async upload()
      {
        const files = [...this.model.files];
        const combinedSize = files.reduce((prev, current, idx) => prev + current.size, 0);

        for (const file of files)
        {
          const factor = file.size / combinedSize;
          const progressBefore = this.progress;

          await api.upload(file, this.model.folderId, progress =>
          {
            this.progress = progressBefore + progress * factor;
          });

          this.completed += 1;
        }

        if (!this.hasErrors)
        {
          this.config.confirm(null);
        }
      }
    }
  }
</script>

<style lang="scss">
  .ui-media-upload
  {
    text-align: left;
  }

  .ui-media-upload
  {
    /*height: 200px;
    background: var(--color-bg-mid);
    border-radius: var(--radius) var(--radius) 0 0;
    margin-top: var(--padding);
    padding: 10px;
    overflow: hidden;*/
  }

  .ui-media-upload-items
  {
    margin-top: 25px;
    max-height: 495px;
    overflow-y: auto;
  }

  .ui-media-upload-progress-text
  {
    margin-top: var(--padding-xs);
    font-size: var(--font-size-s);
    color: var(--color-text-dim);
  }


</style>