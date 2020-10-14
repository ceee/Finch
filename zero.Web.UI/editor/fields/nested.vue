<template>
  <div class="editor-nested" :depth="depth">
    <div class="ui-pick-previews" v-if="items.length">
      <div v-for="(item, index) in items" class="ui-pick-preview">
        <ui-select-button :icon="config.icon" :label="getName(item)" :description="getDescription(item)" :disabled="disabled" @click="editItem(item)" />
        <ui-icon-button v-if="!disabled" icon="fth-x" title="@ui.close" @click="removeItem(index)" :disabled="disabled" />
      </div>
    </div>
    <ui-select-button v-if="limit > items.length" icon="fth-plus" :label="config.addLabel || '@ui.add'" @click="addItem" :disabled="disabled" />
  </div>
</template>

<script>
  import UiEditor from '@zero/editor/editor.vue';
  import UiEditorOverlay from '@zero/editor/editor-overlay.vue';
  import Overlay from '@zero/services/overlay.js';

  export default {

    components: { UiEditor },

    emits: ['input'],

    props: {
      value: {
        type: [Array, Object]
      },
      meta: {
        type: Object,
        default: () => { }
      },
      depth: {
        type: Number,
        default: 0
      },
      disabled: {
        type: Boolean,
        default: false
      },
      config: Object
    },


    data: () => ({
      items: [],
      limit: 100,
      multiple: false
    }),


    mounted()
    {
      this.items = JSON.parse(JSON.stringify(this.value));
      this.limit = this.config.limit || this.limit;
      this.multiple = this.limit > 1;
      if (!this.multiple)
      {
        this.items = this.items ? [this.items] : [];
      }
    },


    methods: {

      getNewItem()
      {
        return JSON.parse(JSON.stringify(this.config.template || {}));
      },

      addItem()
      {
        if (this.limit <= this.items.length)
        {
          return;
        }
        this.editItem(this.getNewItem(), true);
        this.onChange();
      },


      editItem(item, isAdd)
      {
        // open editing overlay
        return Overlay.open({
          component: UiEditorOverlay,
          display: 'editor',
          renderer: this.config.renderer,
          title: this.config.title || '@ui.edit.title',
          model: item,
          width: 1100,
          create: isAdd
        }).then(value =>
        {
          if (isAdd)
          {
            this.items.push(value);
          }
          else
          {
            const index = this.items.indexOf(item);
            this.removeItem(index);
            this.items.splice(index, 0, value);
          }
          this.onChange();
        });
      },


      removeItem(index)
      {
        this.items.splice(index, 1);
        this.onChange();
      },


      onChange()
      {
        this.$emit('input', this.multiple ? this.items : (this.items.length > 0 ? this.items[0] : null));
      },


      getName(item)
      {
        return this.config.item && typeof this.config.item.label === 'function' ? this.config.item.label(item) : 'Item';
      },


      getDescription(item)
      {
        return this.config.item && typeof this.config.item.description === 'function' ? this.config.item.description(item) : '';
      }
    }
  }
</script>