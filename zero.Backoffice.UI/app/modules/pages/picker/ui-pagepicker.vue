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


<script lang="ts">
  import api from '../api';
  import * as overlays from '../../../services/overlay';

  export default {
    name: 'uiPagepicker',

    props: {
      value: {
        type: [String, Array],
        default: null
      },
      model: Object,
      limit: {
        type: Number,
        default: 1
      },
      disabledIds: {
        type: [Array, Function],
        default: []
      },
      disabled: {
        type: Boolean,
        default: false
      },
      rootId: {
        type: [String, Function],
        default: null
      },
      options: {
        type: Object,
        default: () => { }
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
        this.$emit('update:value', value);
        // TODO this does not trigger the forms dirty flag
      },


      async updatePreviews()
      {
        if (!this.value || (Array.isArray(this.value) && !this.value.length))
        {
          this.previews = [];
          return;
        }

        let ids = Array.isArray(this.value) ? this.value : [this.value];

        const result = await api.getPreviews(ids);
        this.previews = result.data;
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


      async pick(id)
      {
        if (this.disabled)
        {
          return;
        }

        let disabledIds = [];

        if (!!this.value && !Array.isArray(this.value))
        {
          disabledIds = [this.value];
        }
        else if (Array.isArray(this.value))
        {
          disabledIds = this.value;
        }

        disabledIds.push(this.model.id);

        if (typeof this.disabledIds === 'function')
        {
          let moreDisabledIds = this.disabledIds(this.model) || [];
          disabledIds.push(...moreDisabledIds);
        }
        else if (this.disabledIds.length > 0)
        {
          disabledIds.push(...this.disabledIds);
        }

        const result = await overlays.open({
          title: '@page.picker.headline',
          closeLabel: '@ui.close',
          component: () => import('./overlay.vue'),
          display: 'editor',
          model: this.multiple ? id : this.value,
          rootId: typeof this.rootId === 'function' ? this.rootId(this.model) : this.rootId,
          disabledIds: disabledIds
        });

        if (result.eventType == 'confirm')
        {
          if (this.multiple)
          {
            if (!this.value || !Array.isArray(this.value))
            {
              this.onChange([result.value.id]);
            }
            else if (this.value.indexOf(result.value.id) < 0)
            {
              if (id)
              {
                this.remove(id);
              }
              this.value.push(result.value.id);
              this.onChange(this.value);
            }
          }
          else
          {
            this.onChange(result.value ? result.value.id : null);
          }
        }
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