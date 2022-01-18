<template>
  <div class="ui-languagepicker" :class="{'is-disabled': disabled }">
    <ui-pick :config="pickerConfig" :value="value" @input="onChange" :disabled="disabled" />
  </div>
</template>


<script>
  import api from './api';
  import { extendObject } from '../../utils';

  export default {
    name: 'uiLanguagepicker',

    props: {
      value: {
        type: [String, Array],
        default: null
      },
      limit: {
        type: Number,
        default: 1
      },
      disabled: {
        type: Boolean,
        default: false
      },
      options: {
        type: Object,
        default: () => {}
      }
    },

    data: () => ({
      previews: [],
      pickerConfig: {}
    }),

    created()
    {
      this.pickerConfig = extendObject({
        scope: 'language',
        items: api.getForPicker,
        previews: api.getPreviews,
        limit: this.limit,
        multiple: this.limit > 1
      }, this.options);
    },


    methods: {
      onChange(value)
      {
        this.$emit('input', value);
        this.$emit('update:value', value);
      }
    }
  }
</script>