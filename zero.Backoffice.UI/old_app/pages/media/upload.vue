<template>
  <div class="media-upload">

    <div v-if="entity.source" class="media-upload-preview" :data-type="entity.type">
      <span ref="image" v-if="entity.type === 'image'" class="media-upload-preview-image media-pattern" @click="setFocalPoint" @dblclick="entity.focalPoint = null">
        <span class="media-upload-preview-focal-point" :style="getFocalPointStyle(entity.focalPoint)"></span>
        <img :src="entity.previewSource" :alt="entity.name" />
      </span>
      
      <a :href="entity.source" target="_blank" v-if="entity.type === 'file'" class="media-upload-preview-file">
        <i :class="icons[entity.type]" :data-extension="entity.source.split('.').pop()"></i>
        <div>
          <span>{{entity.source.split('/').pop()}}</span><br />
          <span class="is-minor">{{entity.source.split('.').pop()}}</span>
        </div>
      </a>
    </div>

    <div v-if="entity.source">
      <a :href="entity.source" target="_blank" v-if="entity.type === 'image'" class="ui-linktext media-upload-preview-remove">Open</a>
      <button type="button" class="ui-linktext media-upload-preview-remove" @click="removeFile" v-if="!disabled">Remove file</button>
    </div>

    <div v-if="!entity.source && !disabled" class="ui-select-button type-light">
      <span class="ui-select-button-icon"><i class="fth-plus"></i></span>
      <content class="ui-select-button-content">
        <strong class="ui-select-button-label">Upload file</strong>
      </content>
      <input class="media-upload-input" type="file" @change="onUpload" />
    </div>
  </div>
</template>


<script>
  import MediaApi from 'zero/api/media.js';

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

      getFocalPointStyle(point)
      {
        return {
          display: 'inline-block',
          left: (point ? point.left : 0.5) * 100 + '%',
          top: (point ? point.top : 0.5) * 100 + '%'
        }
      },

      setFocalPoint(ev)
      {
        if (this.disabled)
        {
          return;
        }

        let image = this.$refs.image.getBoundingClientRect();
        let point = { x: ev.pageX - image.x, y: ev.pageY - image.y };
        const left = +(point.x / image.width).toFixed(2);
        const top = +(point.y / image.height).toFixed(2);

        this.entity.focalPoint =  {
          left: left < 0 ? 0 : (left > 1 ? 1 : left),
          top: top < 0 ? 0 : (top > 1 ? 1: top)
        };
      }
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
    padding: 0;
    border-radius: var(--radius);
    display: inline-block;
    position: relative;
    cursor: pointer;
    overflow: visible;
    user-select: none;

    img
    {
      display: block;
      max-width: 100%;
      max-height: 400px;
      border-radius: var(--radius);
      position: relative;
      z-index: 1;
    }

    /*&:hover img
    {
      opacity: 0.7;
    }*/
  }

  .media-upload-preview-file
  {
    display: flex;
    background: var(--color-box-nested);
    border-radius: var(--radius);
    align-items: center;
    justify-content: flex-start;
    color: var(--color-text);
    padding: var(--padding-s);
    padding-right: var(--padding);

    i
    {
      font-size: 22px;
      position: relative;
      margin-right: var(--padding-s);
    }

    .is-minor
    {
      color: var(--color-text-dim);
      font-size: var(--font-size-xs);
      text-transform: uppercase;
    }
  }

  .media-upload-preview-focal-point
  {
    display: none;
    position: absolute;
    left: 50%;
    top: 50%;
    margin: -8px 0 0 -8px;
    width: 16px;
    height: 16px;
    border-radius: 20px;
    background: white;
    border: 3px solid #222;
    box-shadow: 1px 1px 2px rgba(0,0,0,0.4);
    z-index: 2;
  }

  .media-upload-preview-remove
  {
    margin-right: 8px;
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