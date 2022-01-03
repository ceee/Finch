<template>
  <div class="ui-countrypicker" :class="{'is-disabled': disabled }">
    <ui-pick :config="pickerConfig" :value="value" @input="onChange" :disabled="disabled" />
  </div>
</template>


<script>
  import api from './api';
  import { extendObject } from '../../utils';

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
      const mapItem = item => ({
        id: item.id,
        name: item.name,
        icon: 'flag-' + item.code.toLowerCase()
      });

      this.pickerConfig = extendObject({
        scope: 'country',
        items: async search =>
        {
          const res = await api.getByQuery({ search });
          return Promise.resolve(res.data.map(mapItem));
        },
        previews: async ids =>
        {
          const res = await api.getByQuery({ ids });
          return Promise.resolve(res.data.map(mapItem));
        },
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