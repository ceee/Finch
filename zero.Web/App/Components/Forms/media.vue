<template>
  <div class="ui-media">
    <div class="ui-media-previews">
      <div v-for="item in items" class="ui-media-preview">
        <div class="ui-media-preview-image">
          <img v-if="item.source" :src="getPreview(item)" :alt="item.name" />
        </div>
      </div>
    </div>
    <div class="ui-media-add" v-if="canAdd">
      <ui-select-button v-if="!configuration.disallowSelect" icon="fth-folder" label="@mediapicker.select_text" description="@mediapicker.select_description" :disabled="disabled" />
      <div v-if="!configuration.disallowUpload" class="ui-media-add-upload">
        <input type="file" :accept="acceptedFileExtensions" :multiple="multiple" @change="onUpload" />
        <ui-select-button icon="fth-upload" label="@mediapicker.upload_text" :description="uploadDescription" :disabled="true" />
      </div>
    </div>
  </div>
</template>


<script>
  import { each as _each, extend as _extend, debounce as _debounce, isArray as _isArray } from 'underscore';
  import Strings from 'zero/services/strings.js';

  const TYPES = {
    ALL: 'all',
    IMAGE: 'image',
    VIDEO: 'video',
    DOCUMENT: 'document',
    OTHER: 'other'
  };

  const DISPLAY = {
    DEFAULT: 'default',
    BIG: 'big',
    GRID: 'grid'
  };

  const defaultConfig = {
    // maximum media items
    limit: 1,
    // media type
    type: TYPES.ALL,
    // how the previews are displayed
    display: DISPLAY.DEFAULT,
    // whether the select button is disallowed
    disallowSelect: false,
    // whether file upload is disallowed
    disallowUpload: false,
    // allowed file extensions
    fileExtensions: [],
    // maximum file size in MiB
    maxFileSize: 10
  };

  export default {
    name: 'uiMedia',

    props: {
      disabled: {
        type: Boolean,
        default: false
      },
      config: {
        type: Object,
        default: () =>
        {
          return defaultConfig;
        }
      },
      value: {
        type: [Array, Object]
      }
    },

    data: () => ({
      configuration: {},
      items: []
    }),

    watch: {
      config: {
        deep: true,
        handler: function ()
        {
          this.initialize();
        }
      },
      value: {
        deep: true,
        handler: function ()
        {
          this.initialize();
        }
      }
    },

    computed: {
      multiple()
      {
        return this.configuration.limit > 1;
      },
      canAdd()
      {
        return this.configuration.limit - this.items.length > 0;
      },
      uploadDescription()
      {
        return this.configuration.fileExtensions.length > 0 ? this.displayedFileExtensions : '@mediapicker.upload_description';
      },
      acceptedFileExtensions()
      {
        return this.configuration.fileExtensions.join(',');
      },
      displayedFileExtensions()
      {
        return this.configuration.fileExtensions.join(', ').replace(/\./g, '');
      }
    },

    created()
    {
      this.initialize();
    },

    methods: {

      initialize()
      {
        this.configuration = _extend(defaultConfig, this.config);

        if (_isArray(this.value))
        {
          this.items = this.value;
        }
        else if (!!this.value && typeof this.value === 'object' && this.value.source)
        {
          this.items = [this.value];
        }
      },


      onUpload(event)
      {
        let remaining = this.configuration.limit - this.items.length;
        const files = event.target.files;

        if (files && files.length > 0)
        {
          for (var i = 0; i < files.length; i++)
          {
            if (i >= remaining)
            {
              break;
            }

            this.addFile(files[i]);
          }
        }

        this.update();
      },


      addFromLibrary(item)
      {
        this.items.push(item);
        this.update();
      },


      addFile(file)
      {
        var source = URL.createObjectURL(file);
        var media = {
          id: 'upload:' + Strings.guid(),
          name: file.name,
          source: source,
          size: file.size,
          mimeType: file.type
        };

        this.items.push(media);

        var reader = new FileReader();
        reader.onload = function (e)
        {
          media.source = e.target.result;
        };
        reader.readAsDataURL(file);
      },


      update()
      {
        this.$emit('input', this.multiple ? this.items : this.items[0]);
      },


      getPreview(item)
      {
        if (!item.id || item.id.indexOf('upload:') > -1 || item.source.indexOf("http") === 0)
        {
          return item.source;
        }

        if (item.id.indexOf("native:") > -1)
        {
          return item.source + "?width=100&height=100";
        }

        if (!item.hasThumb)
        {
          return item.source;
        }

        var extension = '.' + item.source.split('.').pop();
        return item.source.replace(extension, ".thumb" + extension);
      }
    }
  }
</script>

<style lang="scss">
  .ui-media
  {

  }

  .ui-media-add
  {
    
  }

  .ui-media-add-upload
  {
    display: inline-block;
    position: relative;

    .ui-select-button + &
    {
      margin-left: var(--padding);
    }

    input[type="file"]
    {
      position: absolute;
      left: 0;
      top: 0;
      right: 0;
      bottom: 0;
      width: 100%;
      height: 100%;
      border: none;
      cursor: pointer;
      opacity: 0;
    }
  }

  .ui-media-preview
  {

  }

  .ui-media-preview-image
  {
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100px;
    height: 80px;
    /*background: var(--color-bg);*/
    border: 1px solid var(--color-line-light);
    padding: 5px;
    border-radius: var(--radius);
    color: var(--color-fg);
    position: relative;
    overflow: hidden;

    img
    {
      border-radius: 3px;
      max-width: 100%;
      max-height: 100%;
      margin: auto;
      display: block;
      color: transprent;
      overflow: hidden;
      font-size: 0;
      position: relative;
      z-index: 2;
    }
  }
</style>