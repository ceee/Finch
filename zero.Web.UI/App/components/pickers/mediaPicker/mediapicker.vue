<template>
  <div class="ui-iconpicker" :class="{'is-disabled': disabled }">
    <input ref="input" type="hidden" :value="value" />
    <ui-select-button icon="fth-plus" label="@mediapicker.select_text" description="@mediapicker.select_description" @click="pick" :disabled="disabled" />
  </div>
</template>


<script>
  import PickMediaOverlay from './overlay';
  import Overlay from 'zero/services/overlay';
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

    computed: {
      
    },

    methods: {

      onChange(value)
      {
        this.$emit('change', value);
        this.$emit('input', value);
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
          model: this.value,
          theme: 'dark',
        }, typeof this.config === 'object' ? this.config : {});

        return Overlay.open(options).then(value =>
        {
          this.onChange(value);
          //this.$refs.input.value = value;
        });
      }
      
    }
  }
</script>