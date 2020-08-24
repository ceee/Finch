<template>
  <div class="media-upload">

    <div v-if="entity.source" class="media-upload-preview" :data-type="entity.type">
      <a :href="entity.source" target="_blank" v-if="entity.type === 'image'" class="media-upload-preview-image">
        <img :src="entity.previewSource" :alt="entity.name" />
      </a>
      
      <a :href="entity.source" target="_blank" v-if="entity.type === 'file'" class="media-upload-preview-file">
        <i :class="icons[entity.type]" :data-extension="entity.source.split('.').pop()"></i>
        <div>
          <span>{{entity.source.split('/').pop()}}</span><br />
          <span class="is-minor">{{entity.source.split('.').pop()}}</span>
        </div>
      </a>
    </div>

    <div v-if="entity.source">
      <button type="button" class="ui-link media-upload-preview-remove" @click="removeFile">Remove file</button>
    </div>

    <div v-if="!entity.source" class="ui-select-button type-light">
      <span class="ui-select-button-icon"><i class="fth-plus"></i></span>
      <content class="ui-select-button-content">
        <strong class="ui-select-button-label">Upload file</strong>
      </content>
      <input class="media-upload-input" type="file" @change="onUpload" />
    </div>
  </div>
</template>


<script>
  import MediaApi from 'zero/resources/media.js';

  export default {

    props: {
      config: Object,
      entity: Object,
      value: String,
      disabled: Boolean
    },

    data: () => ({
      icons: {
        image: 'fth-image',
        video: 'fth-video',
        file: 'fth-file'
      }
    }),

    methods: {

      removeFile()
      {
        this.$emit('input', x => x.source = null);
      },

      onUpload(event)
      {
        let file = event.target.files[0];

        if (!file)
        {
          return;
        }

        // TODO do not allow switch of file to a new type, e.g. "image" => "file"
        MediaApi.upload(file, this.entity.folderId, null, true).then(res =>
        {
          this.$emit('input', x =>
          {
            x.source = res.source;
            x.previewSource = res.previewSource;
            x.thumbnailSource = res.thumbnailSource;
            x.imageMeta = res.imageMeta;
            x.type = res.type;
            x.size = res.size;
            x.focalPoint = res.focalPoint;
            x.name = res.name;
          });
        });
      },
    }
  }
</script>

<style lang="scss">
  .media-upload-preview
  {
    display: block;
  }

  .media-upload-preview-image
  {
    padding: var(--radius);
    border-radius: var(--radius);
    background: var(--color-bg-dim);
    display: inline-block;

    img
    {
      display: block;
      max-width: 100%;
      max-height: 400px;
      border-radius: var(--radius);
    }
  }

  .media-upload-preview-file
  {
    display: flex;
    background: var(--color-bg-bright-two);
    border-radius: var(--radius);
    align-items: center;
    justify-content: flex-start;
    color: var(--color-fg);
    padding: 15px 30px 15px 15px;

    i
    {
      font-size: 28px;
      position: relative;
      margin-right: 12px;
    }

    .is-minor
    {
      color: var(--color-fg-dim);
      font-size: var(--font-size-xs);
      text-transform: uppercase;
    }
  }

  .media-upload-preview-remove
  {
    margin-right: 2px;
    margin-top: 10px;
  }

  input[type="file"].media-upload-input
  {
    position: absolute;
    height: 100%;
    top: 0;
    left: 0;
    width: 100%;
    z-index: 1;
    bottom: 0;
    opacity: 0.001;
    cursor: pointer;
  }
</style>