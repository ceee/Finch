<template>
  <div class="ui-countrypicker" :class="{'is-disabled': disabled }">
    <ui-pick :config="pickerConfig" :value="value" @input="onChange" :disabled="disabled" />
  </div>
</template>


<script>
  import CountriesApi from 'zero/resources/countries.js'
  import { extend as _extend } from 'deps/underscore'

  export default {
    name: 'uiCountrypicker',

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
      this.pickerConfig = _extend({
        scope: 'country',
        items: CountriesApi.getForPicker,
        previews: CountriesApi.getPreviews,
        limit: this.limit,
        multiple: this.limit > 1,
        preview: {
          
        }
      }, this.options);
    },


    methods: {
      onChange(value)
      {
        this.$emit('input', value);
      }
    }
  }
</script>