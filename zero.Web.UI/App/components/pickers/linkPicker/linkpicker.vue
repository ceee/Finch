<template>
  <div class="ui-linkpicker" :class="{'is-disabled': disabled }">
    <input ref="input" type="hidden" :value="value" />
    <!--<div class="ui-pagepicker-previews" v-if="previews.length > 0">
      <div v-for="preview in previews" class="ui-pagepicker-preview">
        <ui-select-button :icon="preview.icon" :label="preview.name" :description="preview.text" :disabled="disabled" @click="pick(preview.id)" :tokens="{ id: preview.id }" />
        <ui-icon-button v-if="!disabled" @click="remove(preview.id)" icon="fth-x" title="@ui.close" />
      </div>
    </div>-->
    <ui-select-button v-if="canAdd" icon="fth-plus" :label="limit > 1 ? '@ui.add' : '@ui.select'" @click="pick()" :disabled="disabled" />
  </div>
</template>


<script>
  import LinkpickerOverlay from './overlay.vue';
  import Overlay from 'zero/helpers/overlay.js';
  import { extend as _extend, isArray as _isArray, isEmpty as _isEmpty, clone as _clone } from 'underscore';

  export default {
    name: 'uiLinkpicker',

    props: {
      value: {
        type: [Object, Array],
        default: null
      },
      limit: {
        type: Number,
        default: 1
      },
      title: {
        type: Boolean,
        default: true
      },
      label: {
        type: Boolean,
        default: false
      },
      target: {
        type: Boolean,
        default: true
      },
      suffix: {
        type: Boolean,
        default: false
      },
      areas: {
        type: Array,
        default: () => []
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
      previews: []
    }),

    watch: {
      value()
      {
        this.updatePreviews();
      }
    },

    computed: {
      multiple()
      {
        return this.limit > 1;
      },
      canAdd()
      {
        return true; // TODO
        let count = Array.isArray(this.value) ? this.value.length : (!this.value ? 0 : 1);
        return !this.disabled && count < this.limit;
      }
    },

    mounted()
    {
      this.updatePreviews();
    },

    methods: {

      onChange(value)
      {
        this.$emit('change', value);
        this.$emit('input', value);
        // TODO this does not trigger the forms dirty flag
      },


      updatePreviews()
      {
        this.previews = [];
        //if (!this.value || _isEmpty(this.value))
        //{
        //  this.previews = [];
        //  return;
        //}

        //let ids = _isArray(this.value) ? this.value : [this.value];
        //PagesApi.getPreviews(ids).then(res =>
        //{
        //  this.previews = res;
        //});
      },


      remove(id)
      {
        if (Array.isArray(this.value))
        {
          let index = this.value.indexOf(id);
          this.value.splice(index, 1);
          this.onChange(this.value);
        }
        else
        {
          this.onChange(this.limit > 1 ? [] : null);
        }
      },


      pick(id)
      {
        if (this.disabled)
        {
          return;
        }

        let options = _extend({
          title: 'Select a link',
          closeLabel: '@ui.close',
          component: LinkpickerOverlay,
          display: 'editor',
          model: this.multiple ? id : this.value,
          options: {
            limit: this.limit,
            label: this.label,
            title: this.title,
            target: this.target,
            suffix: this.suffix,
            areas: this.areas
          }
        }, typeof this.options === 'object' ? this.options : {});

        return Overlay.open(options).then(value =>
        {
          return new Promise(resolve => resolve(value));
        });
      }    
    }
  }
</script>

<style lang="scss">
  .ui-linkpicker-preview
  {
    display: flex;
    justify-content: space-between;
    align-items: center;

    .ui-icon-button
    {
      height: 24px;
      width: 24px;

      i
      {
        font-size: 13px;
      }
    }
  }

  .ui-linkpicker-previews + .ui-select-button,
  .ui-linkpicker-preview + .ui-linkpicker-preview
  {
    margin-top: 10px;
  }
</style>