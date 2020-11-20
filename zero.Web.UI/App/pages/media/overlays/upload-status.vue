<template>
  <div class="ui-media-upload">
    <h2 class="ui-headline" v-localize="'Upload status'"></h2>

    <div class="ui-media-upload-items">
      <button type="button" v-for="item in items" class="ui-media-upload-item">
        <img class="-preview" v-if="item.isImage" :src="item.source" />
        <span class="-preview" v-if="!item.isImage"><i class="fth-file"></i></span>
        <p class="ui-media-upload-item-text">
          {{item.name}}
          <span class="-minor">
            <br>
            <span v-if="item.progress < 100 && !item.error && !item.success" class="ui-media-upload-item-progress">
              <span class="-inner" :style="{ width: item.progress + '%' }"></span>
            </span>
            <span v-if="item.success">Completed</span>
          </span>
        </p>
      </button>
    </div>

    <div class="app-confirm-buttons">
      <!--<ui-button v-if="!disabled" type="action" :submit="true" label="@ui.save"></ui-button>-->
      <ui-button type="light" :label="config.closeLabel" :disabled="loading" @click="config.close"></ui-button>
    </div>
  </div>
</template>


<script>
  import MediaApi from 'zero/resources/media.js'
  import Strings from 'zero/helpers/strings.js';

  export default {

    props: {
      model: FileList,
      config: Object
    },

    data: () => ({
      loading: false,
      items: []
    }),


    mounted()
    {
      const files = this.model;
      let items = [];

      if (!files || files.length < 1)
      {
        return;
      }

      for (var i = 0; i < files.length; i++)
      {
        let file = files[i];

        this.items.push({
          id: 'upload:' + Strings.guid(),
          name: file.name,
          size: file.size,
          mimeType: file.type,
          source: null,
          progress: 0,
          file: file,
          isImage: false,
          success: false,
          error: null
        });
      }


      for (var i = 0; i < this.items.length; i++)
      {
        let item = this.items[i];

        MediaApi.upload(item.file, this.config.folderId, progress =>
        {
          item.progress = progress;
        }).then(res =>
        {
          if (res.success)
          {
            item.source = res.model.thumbnailSource || res.model.source;
            item.isImage = res.model.type === 'image';
            item.success = true;
          }
          else
          {
            item.success = false;
            // TODO output error
          }
        });
      }
    },

    methods: {

      onSubmit()
      {
         
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

  .ui-media-upload-item
  {
    width: 100%;
    height: 70px;
    display: grid;
    grid-template-columns: 70px 1fr;
    gap: 15px;
    align-items: center;
    line-height: 1.4;

    & + .ui-media-upload-item
    {
      margin-top: 15px;
    }

    .-preview
    {
      display: flex;
      align-items: center;
      justify-content: center;
      flex-direction: column;
      height: 70px;
      width: 70px;
      object-fit: cover;
      background: var(--color-bg-mid);
      border-radius: var(--radius);
      position: relative;
      text-align: center;
      font-size: 22px;
    }

    /*&.is-upload .-preview
    {
      background: var(--color-primary);
      color: var(--color-primary-text);
    }*/

    .-extension
    {
      font-size: 12px;
      font-style: normal;
      margin-top: 8px;
    }

    .-minor
    {
      color: var(--color-text-light);
      font-size: var(--font-size-s);
    }
  }

  .ui-media-upload-item-progress
  {
    display: block;
    width: 50%;
    height: 8px;
    margin-top: 6px;
    border-radius: 2px;
    background: var(--color-bg-mid);
    position: relative;

    .-inner
    {
      position: absolute;
      left: 0;
      top: 0;
      height: 100%;
      border-radius: 2px;
      background: var(--color-primary);
    }
  }
</style>