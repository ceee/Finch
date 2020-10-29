<template>
  <div class="ui-pagepicker" :class="{'is-disabled': disabled }">
    <input ref="input" type="hidden" :value="value" />
    <div class="ui-pagepicker-previews" v-if="previews.length > 0">
      <div v-for="preview in previews" class="ui-pagepicker-preview">
        <ui-select-button :icon="preview.icon" :label="preview.name" :description="preview.text" :disabled="disabled" @click="pick(preview.id)" :tokens="{ id: preview.id }" />
        <ui-icon-button v-if="!disabled" @click="remove(preview.id)" icon="fth-x" title="@ui.close" />
      </div>
    </div>
    <ui-select-button v-if="canAdd" icon="fth-plus" :label="limit > 1 ? '@ui.add' : '@ui.select'" @click="pick()" :disabled="disabled" />
  </div>
</template>


<script>
  import PageTreeApi from 'zero/resources/page-tree.js'
  import PagesApi from 'zero/resources/pages.js'
  import PageOverlay from './overlay';
  import Overlay from 'zero/services/overlay';
  import { extend as _extend, isArray as _isArray, isEmpty as _isEmpty, clone as _clone } from 'deps/underscore';

  export default {
    name: 'uiPagepicker',

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
      root: {
        type: String,
        default: null
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
        let count = _isArray(this.value) ? this.value.length : (!this.value ? 0 : 1);
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
        if (!this.value || _isEmpty(this.value))
        {
          this.previews = [];
          return;
        }

        let ids = _isArray(this.value) ? this.value : [this.value];
        PagesApi.getPreviews(ids).then(res =>
        {
          this.previews = res;
        });
      },


      remove(id)
      {
        if (_isArray(this.value))
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

        let disabledIds = [];

        if (!!this.value && !_isArray(this.value))
        {
          disabledIds = [this.value];
        }
        else if (_isArray(this.value))
        {
          disabledIds = this.value;
        }

        let options = _extend({
          title: '@page.picker.headline',
          closeLabel: '@ui.close',
          component: PageOverlay,
          display: 'editor',
          model: this.multiple ? id : this.value,
          rootId: this.root,
          disabledIds: disabledIds
        }, typeof this.options === 'object' ? this.options : {});

        return Overlay.open(options).then(value =>
        {
          if (this.multiple)
          {
            if (!this.value || !_isArray(this.value))
            {
              this.onChange([value.id]);
            }
            else if (this.value.indexOf(value.id) < 0)
            {
              if (id)
              {
                this.remove(id);
              }
              this.value.push(value.id);
              this.onChange(this.value);
            }
          }
          else
          {
            this.onChange(value ? value.id : null);
          }
        });
      }    
    }
  }
</script>

<style lang="scss">
  .ui-pagepicker-preview
  {
    display: flex;
    justify-content: space-between;
    align-items: center;

    .ui-icon-button
    {
      height: 24px;
      width: 24px;
      //background: none;

      i
      {
        font-size: 13px;
      }
    }
  }

  .ui-pagepicker-previews + .ui-select-button,
  .ui-pagepicker-preview + .ui-pagepicker-preview
  {
    margin-top: 10px;
  }
</style>