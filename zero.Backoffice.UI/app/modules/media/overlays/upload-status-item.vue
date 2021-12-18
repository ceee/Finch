<template>
  <div class="ui-media-upload-item" v-if="loaded">
    <img class="-preview" v-if="preview" :src="preview" />
    <span class="-preview" v-if="!preview"><ui-icon symbol="fth-file" :size="18" /><span class="-ext">{{extension}}</span></span>
    <p class="ui-media-upload-item-text">
      <span class="-text">{{file.name}}</span>
      <span class="-minor">
        <span v-filesize="file.size"></span>
        <!--<span v-if="item.progress < 101 && !item.error && !item.success" class="ui-media-upload-item-progress">
          <span class="-inner" :style="{ width: item.progress + '%' }"></span>
        </span>-->
      </span>
    </p>
  </div>
</template>


<script lang="ts">
  import api from '../api';
  import { generateId } from '../../../utils/numbers';
  import { defineComponent } from 'vue';

  export default defineComponent({

    props: {
      file: File
    },

    data: () => ({
      preview: null,
      extension: null,
      loaded: false,
    }),


    watch: {
      file: {
        deep: true,
        handler()
        {
          this.render();
        }
      }
    },


    mounted()
    {
      this.render();
    },


    beforeUnmount()
    {
      if (this.preview)
      {
        URL.revokeObjectURL(this.preview);
      }
    },


    methods: {

      render()
      {
        this.loaded = false;

        if (!this.file)
        {
          return;
        }

        let name = this.file.name;
        let dotIndex = name.lastIndexOf('.');

        if (dotIndex > -1 && dotIndex + 6 > name.length)
        {
          this.extension = name.substring(dotIndex + 1, name.length);
        }

        if (this.file.type.indexOf('image/') === 0)
        {
          this.preview = URL.createObjectURL(this.file);
        }

        this.loaded = true;
      }

    }


    //const files = this.model;

    //console.info(files);
    //let items = [];

    //if (!files || files.length < 1)
    //{
    //  return;
    //}

    //for (var i = 0; i < files.length; i++)
    //{
    //  let file = files[i];
    //  let preview = null;

    //  if (file.type.indexOf('image/') === 0)
    //  {
    //    let reader = new FileReader();
    //    reader.onload = e => preview = e.target.result;
    //    reader.readAsDataURL(file);
    //  }

    //  this.items.push({
    //    id: 'upload:' + generateId(),
    //    name: file.name,
    //    size: file.size,
    //    mimeType: file.type,
    //    preview: preview,
    //    source: null,
    //    progress: 0,
    //    file: file,
    //    isImage: false,
    //    success: false,
    //    error: null
    //  });
    //}

    //console.info(this.items);
  })
</script>

<style lang="scss">

  .ui-media-upload-item
  {
    width: 100%;
    height: auto;
    display: grid;
    grid-template-columns: auto minmax(0, 1fr);
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
      height: 50px;
      width: 50px;
      object-fit: cover;
      background: var(--color-bg-shade-2);
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


    .-ext
    {
      font-size: 8px;
      color: var(--color-text-dim);
      font-style: normal;
      margin-top: 4px;
      text-transform: uppercase;
      margin-bottom: -4px;
    }

    .-text
    {
      white-space: nowrap;
      text-overflow: ellipsis;
      width: 100%;
      display: block;
      overflow: hidden;
    }

    .-minor
    {
      display: block;
      color: var(--color-text-dim);
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

  .ui-media-upload-item-text
  {
    display: flex;
    flex-direction: column;
  }
</style>