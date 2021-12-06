<template>
  <div class="ui-spacepicker" :class="{'is-disabled': disabled }">
    <ui-pick v-if="pickerConfig" :config="pickerConfig" :value="value" @input="onChange" :disabled="disabled" />
  </div>
</template>


<script>
  import SpacesApi from 'zero/api/spaces.js'
  import { extend as _extend } from 'underscore'

  export default {
    name: 'uiSpacepicker',

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
      pickerConfig: null
    }),

    created()
    {
      SpacesApi.getAll().then(items =>
      {
        this.pickerConfig = _extend({
          scope: 'space',
          keys: {
            id: 'alias',
            name: 'name',
            description: 'description',
            icon: 'icon'
          },
          items: items,
          limit: this.limit,
          multiple: this.limit > 1
        }, this.options);
      });
    },


    methods: {
      onChange(value)
      {
        this.$emit('input', value);
      }
    }
  }
</script>