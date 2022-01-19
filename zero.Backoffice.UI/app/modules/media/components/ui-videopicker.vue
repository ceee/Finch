<template>
  <div class="shop-videopicker" :class="{'is-disabled': disabled }">
    <input ref="input" type="hidden" :value="value" />
    <div class="shop-videopicker-previews" v-if="previews.length > 0" v-sortable="{ onUpdate: onSortingUpdated }">
      <div v-for="(preview, index) in previews" :key="index" class="shop-videopicker-preview">
        <ui-select-button :icon="preview.icon" :label="preview.name" :description="preview.text" :disabled="disabled" @click="pick(preview.model)" :tokens="{ id: preview.id }" />
        <ui-icon-button v-if="!disabled" @click="remove(preview.model)" icon="fth-x" title="@ui.close" />
      </div>
    </div>

    <ui-select-button v-if="canAdd" icon="fth-plus" :label="limit > 1 ? '@ui.add' : '@ui.select'" @click="pick()" :disabled="disabled" />
  </div>
</template>


<script>
  import * as overlays from '../../../services/overlay';

  export default {
    name: 'uiVideopicker',

    props: {
      value: {
        type: [Object, Array],
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
      items: [],
      previews: []
    }),

    watch: {
      value(val)
      {
        this.setup(val);
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
      this.setup(this.value);
      this.updatePreviews();
    },

    methods: {

      setup(value)
      {
        this.items = JSON.parse(JSON.stringify(value)) || [];
        if (!this.multiple)
        {
          this.items = this.items && !Array.isArray(this.items) ? [this.items] : [];
        }
        console.info(this.items);
      },

      onChange()
      {
        let value = this.multiple ? [...this.items] : (this.items.length > 0 ? this.items[0] : null);
        this.$emit('input', value);
        this.$emit('update:value', value);
      },


      onSortingUpdated(ev)
      {
        this.previews = arrayMove(this.previews, ev.oldIndex, ev.newIndex);
        this.items = arrayMove(this.items, ev.oldIndex, ev.newIndex);
        this.onChange();
      },


      async updatePreviews()
      {
        this.previews = this.items.map((item, idx) => ({
          id: idx,
          name: '@videopicker.providers.' + item.provider,
          text: item.title,
          icon: 'fth-video',
          model: item
        }));
      },


      remove(item)
      {
        this.items.splice(this.items.indexOf(item), 1);
        this.onChange();
      },


      async pick(item)
      {
        if (this.disabled)
        {
          return;
        }

        const result = await overlays.open({
          component: () => import('../overlays/videopicker.vue'),
          display: 'editor',
          model: item
        })

        if (result.eventType == 'confirm')
        {
          if (item && item.id)
          {
            this.items.splice(this.items.indexOf(item), 1);
          }

          this.items.push(result.value);
          this.onChange();
        }
      }
    }
  }
</script>

<style lang="scss">
  .shop-videopicker-preview
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

  .shop-videopicker-previews + .ui-select-button,
  .shop-videopicker-preview + .shop-videopicker-preview
  {
    margin-top: 10px;
  }
</style>