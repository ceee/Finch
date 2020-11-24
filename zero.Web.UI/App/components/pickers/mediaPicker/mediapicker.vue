<template>
  <div class="ui-mediapicker">
    <div v-if="previews.length > 0" class="ui-mediapicker-previews">
      <div v-for="item in previews" class="ui-mediapicker-preview">
        <div v-if="!item.error" class="ui-mediapicker-preview-image">
          <img v-if="item.thumbnailSource" :src="item.thumbnailSource" :alt="item.name" />
          <button v-if="!disabled" type="button" class="ui-mediapicker-preview-image-delete" @click="remove(item)" v-localize:title="'@ui.remove'"><i class="fth-x"></i></button>
          <button v-if="!disabled" type="button" class="ui-mediapicker-preview-image-edit" @click="edit(item)" v-localize:title="'@ui.edit'"><i class="fth-edit-2"></i></button>
        </div>
        <div v-if="!item.error" class="ui-mediapicker-preview-text">
          <b :title="item.name">{{getFilename(item.name)}}</b>
          <span class="is-filesize">{{getFileinfo(item)}}</span>
        </div>
        <div v-if="item.error">
          <ui-select-button icon="fth-alert-circle color-red" label="@errors.preview.notfound" description="@errors.preview.notfound_text" @click="remove({ id: item.id })" :tokens="{ id: item.id }" />
        </div>
      </div>
    </div>
    <div v-if="canAdd" class="ui-mediapicker-select" :class="{'is-disabled': disabled }">
      <input ref="input" type="hidden" :value="value" />
      <ui-select-button icon="fth-plus" label="@mediapicker.select_text" description="@mediapicker.select_description" @click="pick" :disabled="disabled" />
    </div>
  </div>
</template>


<script>
  import MediaApi from 'zero/api/media.js'
  import PickMediaOverlay from './overlay.vue';
  import Overlay from 'zero/helpers/overlay.js';
  import { each as _each, extend as _extend, debounce as _debounce, isArray as _isArray } from 'underscore';
  import Strings from 'zero/helpers/strings.js';

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

  const MAX_FILENAME_LENGTH = 32;

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
    name: 'uiMediapicker',

    props: {
      value: {
        type: [Array, String],
        default: null
      },
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
    },


    data: () => ({
      configuration: {},
      previews: []
    }),


    watch: {
      value()
      {
        this.updatePreviews();
      }
    },


    created()
    {
      this.configuration = _extend(defaultConfig, this.config);
    },


    mounted()
    {
      this.updatePreviews();
    },


    computed: {
      multiple()
      {
        return this.configuration.limit > 1;
      },
      canAdd()
      {
        return !this.disabled && this.configuration.limit - this.previews.length > 0;
      },
    },


    methods: {

      updatePreviews()
      {
        //console.info('preview', JSON.parse(JSON.stringify(this.value)));
        let ids = [];

        if (typeof this.value === 'string')
        {
          ids = [this.value];
        }
        else if (_isArray(this.value))
        {
          ids = this.value;
        }

        this.previews = [];

        if (!ids || ids.length < 1)
        {
          return;
        }

        MediaApi.getByIds(ids).then(res =>
        {
          _each(ids, id =>
          {
            let value = res[id];

            if (!value)
            {
              this.previews.push({
                id: id,
                error: true
              });
            }
            else
            {
              this.previews.push(value);
            }
          });
        });
      },


      remove(item)
      {
        let newValue = this.value;

        if (typeof this.value === 'string')
        {
          newValue = null;
        }
        else if (_isArray(this.value))
        {
          const index = this.value.indexOf(item.id);
          newValue.splice(index, 1);
        }

        this.onChange(newValue);
      },


      getFilename(name)
      {
        if (name.length < MAX_FILENAME_LENGTH)
        {
          return name;
        }

        const parts = name.split('.');
        const extension = parts.pop();

        return parts.join('.').substring(0, MAX_FILENAME_LENGTH - 6) + '...' + extension;
      },


      getFileinfo(item)
      {
        if (item.dimension)
        {
          return `${Strings.filesize(item.size)} – ${item.dimension.width} × ${item.dimension.height}`;
        }

        return Strings.filesize(item.size);
      },


      onChange(value)
      {    
        this.$emit('change', value);
        this.$emit('input', value);
        //this.updatePreviews();
        // TODO this does not trigger the forms dirty flag
      },


      pick()
      {
        if (this.disabled)
        {
          return;
        }

        let options = _extend({
          title: '@iconpicker.title',
          closeLabel: '@ui.close',
          component: PickMediaOverlay,
          model: this.configuration.limit > 1 ? this.value[0] : this.value,
          folderId: null, //'mediaFolders.97-A',
          width: 520
        }, typeof this.config === 'object' ? this.config : {});

        options.display = 'dialog';

        return Overlay.open(options).then(value =>
        {
          let newValue = this.value;

          if (this.multiple)
          {
            if (!newValue)
            {
              newValue = [];
            }
            newValue.push(value.id);
          }
          else
          {
            newValue = value.id;
          }

          this.onChange(newValue);
        });
      }
      
    }
  }
</script>

<style lang="scss">
  .ui-mediapicker-previews
  {
    .display-grid &
    {
      display: flex;
      gap: 10px;

      .ui-mediapicker-preview + .ui-mediapicker-preview
      {
        margin-top: 0;
      }
    }
  }

  .ui-mediapicker-previews + .ui-mediapicker-select,
  .ui-mediapicker-preview + .ui-mediapicker-preview
  {
    margin-top: 10px;
  }

  .ui-mediapicker:not(.display-grid) .ui-mediapicker-previews + .ui-mediapicker-add
  {
    display: flex;
    flex-direction: column;

    .ui-select-button + .ui-mediapicker-add-upload
    {
      margin-left: 0;
      margin-top: 10px;
    }
  }

  .ui-mediapicker-preview
  {
    display: flex;
    align-items: center;
  }

  .ui-mediapicker-preview-image
  {
    display: flex;
    justify-content: center;
    align-items: center;
    width: 50px;
    height: 50px;
    /*background: var(--color-bg);*/
    border: 1px solid var(--color-line);
    padding: 3px;
    border-radius: var(--radius);
    color: var(--color-text);
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

    &.is-icon
    {
      width: 42px;
      height: 42px;
      display: inline-flex;
      justify-content: center;
      align-items: center;
      border-radius: var(--radius);
      background: var(--color-box);
      color: var(--color-text);
      text-align: center;
      font-size: 16px;
    }

    &:hover .ui-mediapicker-preview-image-delete,
    &:hover .ui-mediapicker-preview-image-edit
    {
      opacity: 1;
      transition-delay: 0.1s;
    }

    .display-big &, .display-grid &
    {
      width: 100px;
      height: 100px;
    }
  }

  .ui-mediapicker-preview-text
  {
    display: flex;
    flex-direction: column;
    margin-left: 16px;
    font-size: var(--font-size);

    .is-filesize
    {
      color: var(--color-text-dim);
      margin-top: 3px;
      font-size: var(--font-size-xs);
    }

    .display-grid &
    {
      display: none;
    }
  }

  .ui-mediapicker-preview-image-delete,
  .ui-mediapicker-preview-image-edit
  {
    opacity: 0;
    transition: opacity 0.15s ease;
    position: absolute;
    display: inline-block;
    right: 3px;
    bottom: 3px;
    width: 24px;
    height: 24px;
    line-height: 26px;
    border-radius: 20px;
    background: var(--color-negative);
    color: var(--color-primary-text);
    z-index: 2;
    text-align: center;
    font-size: 13px;

    .ui-media.display-default &
    {
      width: 20px;
      height: 20px;
      line-height: 22px;
    }
  }

  .ui-mediapicker-preview-image-edit
  {
    right: 30px;
    background: var(--color-box);
    color: var(--color-text);

    .ui-media.display-default &
    {
      right: 24px;
    }
  }
</style>