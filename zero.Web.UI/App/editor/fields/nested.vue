<template>
  <div class="editor-nested" :depth="depth">
    <div class="ui-pick-previews" v-if="items.length">
      <div v-for="(item, index) in items" class="ui-pick-preview">
        <ui-select-button :icon="getIcon(item)" :label="getName(item)" :description="getDescription(item)" :disabled="disabled" @click="editItem(item)" />
        <ui-icon-button v-if="!disabled" icon="fth-x" title="@ui.close" @click="removeItem(index)" :disabled="disabled" />
      </div>
    </div>
    <ui-select-button v-if="limit > items.length" icon="fth-plus" :label="addLabel || '@ui.add'" @click="addItem" :disabled="disabled" />
  </div>
</template>

<script>
  import UiEditor from 'zero/editor/editor.vue';
  import UiEditorOverlay from 'zero/editor/editor-overlay.vue';
  import Overlay from 'zero/helpers/overlay.js';
  import Editor from 'zero/core/editor.ts';
  import Strings from 'zero/helpers/strings.js';

  export default {

    components: { UiEditor },

    props: {
      value: [Array, Object],
      meta: Object,
      depth: Number,
      disabled: Boolean,
      editor: {
        type: Object,
        required: true
      },
      limit: {
        type: Number,
        default: 100
      },
      title: String,
      addLabel: String,
      itemLabel: Function,
      itemDescription: Function,
      itemIcon: [String, Function],
      template: {
        type: Object,
        required: true
      }
    },

    watch: {
      value: {
        deep: true,
        handler(val)
        {
          this.setup();
        }
      }
    },


    data: () => ({
      items: [],
      multiple: false
    }),


    mounted()
    {
      this.setup();
    },


    methods: {

      setup()
      {
        this.items = JSON.parse(JSON.stringify(this.value)) || [];
        this.multiple = this.limit > 1;
        if (!this.multiple)
        {
          this.items = this.items ? [this.items] : [];
        }
      },

      getNewItem()
      {
        return JSON.parse(JSON.stringify(this.template || {}));
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
          editor: this.editor,
          title: this.title || '@ui.edit.title',
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
        return Strings.htmlToText(typeof this.itemLabel === 'function' ? this.itemLabel(item) : 'Item');
      },


      getDescription(item)
      {
        return Strings.htmlToText(typeof this.itemDescription === 'function' ? this.itemDescription(item) : '');
      },


      getIcon(item)
      {
        return typeof this.itemIcon === 'function' ? this.itemIcon(item) : this.itemIcon;
      }
    }
  }
</script>