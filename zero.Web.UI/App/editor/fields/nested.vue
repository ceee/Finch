<template>
  <div class="editor-nested" :depth="depth">
    <div class="ui-pick-previews" v-if="items.length">
      <div v-for="(item, index) in items" class="ui-pick-preview">
        <ui-select-button :icon="config.icon" :label="getName(item)" :description="getDescription(item)" :disabled="disabled" @click="editItem(item)" />
        <ui-icon-button v-if="!disabled" icon="fth-x" title="@ui.close" @click="removeItem(index)" />
      </div>
    </div>
    <ui-select-button icon="fth-plus" :label="config.addLabel || '@ui.add'" @click="addItem" :disabled="disabled" />
  </div>
</template>

<script>
  import UiEditor from 'zero/editor/editor';
  import UiEditorOverlay from 'zero/editor/editor-overlay';
  import Overlay from 'zero/services/overlay';

  export default {

    components: { UiEditor },

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
      items: []
    }),


    mounted()
    {
      this.items = JSON.parse(JSON.stringify(this.value));
    },


    methods: {

      getNewItem()
      {
        return JSON.parse(JSON.stringify(this.config.template || {}));
      },

      addItem()
      {
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
          console.info(JSON.parse(JSON.stringify(value)));
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
        this.$emit('input', this.items);
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