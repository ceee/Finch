<template>
  <div v-if="output" class="ui-iconpicker" :class="{'is-disabled': disabled }">
    <input ref="input" type="hidden" :value="value" />
    <ui-select-button :icon="previewIcon" label="@ui.icon" :description="buttonDescription" @click="pick" :disabled="disabled" />
  </div>
</template>


<script>
  import PickIconOverlay from './overlay.vue';
  import Overlay from 'zero/helpers/overlay.js';
  import { extend as _extend } from 'underscore';

  export default {
    name: 'uiIconpicker',

    props: {
      value: {
        type: String,
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      },
      colors: {
        type: Boolean,
        default: false
      },
      output: {
        type: Boolean,
        default: true
      },
      set: {
        type: String,
        default: 'feather'
      },
      options: {
        type: Object,
        default: () =>
        {
          return {

          };
        }
      }
    },

    data: () => ({
      iconSet: null
    }),

    watch: {
      set()
      {
        this.loadSet();
      }
    },

    created()
    {
      this.loadSet();
    },

    computed: {
      buttonDescription()
      {
        return this.value ? this.value.split(' ')[0] : '@ui.icon_select';
      },
      previewIcon()
      {
        return this.value || 'fth-plus';
      }
    },

    methods: {

      onChange(value)
      {
        this.$emit('change', value);
        this.$emit('input', value);
        // TODO this does not trigger the forms dirty flag
      },

      loadSet()
      {
        this.iconSet = __zero.icons.find(x => x.alias === this.set);
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
          component: PickIconOverlay,
          display: 'editor',
          set: this.iconSet,
          model: this.value,
          colors: this.colors,
        }, typeof this.options === 'object' ? this.options : {});

        return Overlay.open(options).then(value =>
        {
          this.onChange(value);
          //this.$refs.input.value = value;
        });
      }
      
    }
  }
</script>