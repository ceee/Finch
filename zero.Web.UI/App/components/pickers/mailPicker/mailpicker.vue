<template>
  <div class="ui-mailtemplatespicker" :class="{'is-disabled': disabled }">
    <ui-pick :config="pickerConfig" :value="value" @input="onChange" :disabled="disabled" />
  </div>
</template>


<script>
  import MailTemplatesApi from 'zero/resources/mailTemplates.js'
  import { extend as _extend } from 'underscore'

  export default {
    name: 'uiMailtemplatespicker',

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
        scope: 'mailtemplate',
        items: MailTemplatesApi.getForPicker,
        previews: MailTemplatesApi.getPreviews,
        limit: this.limit,
        multiple: this.limit > 1
        //onChange: function (id, item)
        //{
        //  vm.manufacturer = item ? { Id: item.id, Name: item.name } : null;
        //}
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