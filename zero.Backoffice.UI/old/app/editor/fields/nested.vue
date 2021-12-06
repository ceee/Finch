<template>
  <div class="editor-nested" :depth="depth">
    <div class="ui-pick-previews" v-if="items.length" v-sortable="{ onUpdate: onSortingUpdated }">
      <div v-for="(item, index) in items" :key="item.id" class="ui-pick-preview">
        <ui-select-button :icon="getIcon(item)" :label="getName(item)" :description="getDescription(item)" :disabled="disabled" @click="editItem(item)" />
        <ui-icon-button v-if="!disabled" icon="fth-x" title="@ui.close" @click="removeItem(index)" :disabled="disabled" :size="14" />
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
  import Arrays from 'zero/helpers/arrays.js';

  export default {

    name: 'UiEditorFieldNested',

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
      entity: Object,
      limit: {
        type: Number,
        default: 100
      },
      width: {
        type: Number,
        default: 820
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
          this.setup(val);
        }
      }
    },


    data: () => ({
      items: [],
      multiple: false
    }),


    mounted()
    {
      this.setup(this.value);
    },


    methods: {

      setup(value)
      {
        this.items = JSON.parse(JSON.stringify(value)) || [];
        this.multiple = this.limit > 1;
        if (!this.multiple)
        {
          this.items = this.items ? [this.items] : [];
        }
      },

      getNewItem()
      {
        return JSON.parse(JSON.stringify(this.template || {}));
        // TODO we need to set a default ID here so we can sort based on this v-for key.
        // the problem is we don't know if the object has an ID nor how long it should be. 
        // It is only generated on the server so we don't have access to it yet.
        // the v-for key is necessary so the v-sortable works and correctly propagates changes, d'oh.
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
        let parentModel = JSON.parse(JSON.stringify(this.entity));

        if (this.meta && this.meta.parentModel)
        {
          parentModel.parentModel = this.meta.parentModel;
        }

        // open editing overlay
        return Overlay.open({
          component: UiEditorOverlay,
          display: 'editor',
          editor: this.editor,
          title: this.title || '@ui.edit.title',
          model: item,
          width: this.width,
          parentModel,
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
        let name = typeof this.itemLabel === 'function' ? this.itemLabel(item) : null;
        return Strings.htmlToText(name ?? '@ui.item');
      },


      getDescription(item)
      {
        return Strings.htmlToText(typeof this.itemDescription === 'function' ? this.itemDescription(item) : '');
      },


      getIcon(item)
      {
        return typeof this.itemIcon === 'function' ? this.itemIcon(item) : this.itemIcon;
      },


      onSortingUpdated(ev)
      {
        this.items = Arrays.move(this.items, ev.oldIndex, ev.newIndex);
        this.onChange();
        //this.$emit('input', this.multiple ? result : (result.length > 0 ? result[0] : null));
      },
    }
  }
</script>