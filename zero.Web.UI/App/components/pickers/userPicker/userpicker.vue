<template>
  <div class="ui-userpicker" :class="{'is-disabled': disabled }">
    <ui-pick :config="pickerConfig" :value="value" @input="onChange" :disabled="disabled" />
  </div>
</template>


<script>
  import UsersApi from 'zero/resources/users.js'
  import { extend as _extend } from 'underscore'

  export default {
    name: 'uiUserpicker',

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
        default: () => { }
      }
    },

    data: () => ({
      previews: [],
      pickerConfig: {}
    }),

    created()
    {
      this.pickerConfig = _extend({
        scope: 'user',
        items: UsersApi.getForPicker,
        previews: UsersApi.getPreviews,
        limit: this.limit,
        multiple: this.limit > 1,
        preview: {
          enabled: true,
          iconAsImage: true
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

<style lang="scss">
  .ui-userpicker
  {
    .ui-select-button-icon.is-image, .ui-select-button-icon
    {
      padding: 0;
      border-radius: 50px;

      img
      {
        border-radius: 50px;
      }
    }
  }
</style>