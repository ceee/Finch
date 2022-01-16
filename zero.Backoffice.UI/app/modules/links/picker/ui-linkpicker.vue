<template>
  <div class="ui-linkpicker" :class="{'is-disabled': disabled }">
    <input ref="input" type="hidden" :value="value" />
    <div class="ui-linkpicker-previews" v-if="previews.length > 0">
      <div v-for="preview in previews" class="ui-linkpicker-preview">
        <ui-select-button :icon="preview.icon" :label="preview.name" :description="preview.text" :disabled="disabled" @click="pick(preview.origin)" :tokens="{ id: preview.id }" />
        <ui-icon-button v-if="!disabled" @click="remove(preview.id)" icon="fth-x" title="@ui.close" />
      </div>
    </div>
    <ui-select-button v-if="canAdd" icon="fth-plus" :label="limit > 1 ? '@ui.add' : '@ui.select'" @click="pick()" :disabled="disabled" />
  </div>
</template>


<script>
  import api from '../api';
  import * as overlays from '../../../services/overlay';

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
      areas: {
        type: Array,
        default: []
      },
      allowTitle: {
        type: Boolean,
        default: true
      },
      allowTarget: {
        type: Boolean,
        default: true
      },
      allowSuffix: {
        type: Boolean,
        default: true
      },
      disabled: {
        type: Boolean,
        default: false
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


      async updatePreviews()
      {
        this.previews = [];

        if (!this.value || (Array.isArray(this.value) && this.value.length < 1))
        {
          this.$emit('previews', this.multiple ? [] : null);
          this.previews = [];
          return;
        }

        let links = Array.isArray(this.value) ? this.value : [this.value];

        const result = await api.convert(links);

        let index = 0;
        this.previews = result.data.map(x =>
        {
          x.origin = links[index++];
          return x;
        });
        this.$emit('previews', this.multiple ? this.previews : this.previews[0]);
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
          this.onChange(this.multiple ? [] : null);
        }
      },


      async pick(item)
      {
        if (this.disabled)
        {
          return;
        }

        const result = await overlays.open({
          component: () => import('./ui-linkpicker-overlay.vue'),
          display: 'editor',
          model: {
            value: this.multiple ? item : this.value,
            areas: this.areas,
            allowTitle: this.allowTitle,
            allowTarget: this.allowTarget,
            allowSuffix: this.allowSuffix
          }
        });

        if (result.eventType === 'confirm')
        {
          if (this.multiple)
          {
            this.value.push(result.value);
            this.onChange(this.value);
          }
          else
          {
            this.onChange(result.value);
          }
        }
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