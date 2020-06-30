<template>
  <div class="editor-nested" :depth="depth">
    <div v-for="(item, index) in items" class="editor-nested-item" :depth="depth">
      <div class="editor-nested-item-header">
        <button type="button" class="editor-nested-item-header-link" @click="toggle(item)">
          <i class="editor-nested-item-header-icon fth-truck" :class="item.icon"></i>
          <span class="editor-nested-item-header-text">
            {{item.name | localize}}
            <span v-if="item.description" v-localize="item.description"></span>
          </span>
        </button>
        <aside class="editor-nested-item-header-actions">
          <ui-icon-button icon="fth-x" title="@ui.remove" type="action small" @click="removeItem(index)" />
        </aside>
      </div>
      <div v-if="item.open" class="editor-nested-item-content">
        <ui-editor :config="config.renderer" :value="item" @input="onChange" :meta="meta" :nested="true" :depth="depth + 1" />
      </div>
    </div>
    <ui-button type="light" :label="config.addLabel || '@ui.add'" @click="addItem" />
  </div>
</template>

<script>
  import UiEditor from 'zero/editor/editor';

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
        let item = JSON.parse(JSON.stringify(this.config.template || {}));
        item.open = false;
        return item;
      },

      addItem()
      {
        this.items.push(this.getNewItem());
      },


      toggle(item)
      {
        this.$set(item, 'open', !item.open);
      },


      removeItem(index)
      {
        this.items.splice(index, 1);
      },


      onChange(val)
      {
        this.$emit('input', this.items);
      }

    }
  }
</script>

<style lang="scss">
  .editor-nested-item:first-child .editor-nested-item-header
  {
    padding-top: 0;
  }

  .editor-nested-item + .editor-nested-item,
  .editor-nested-item + .ui-button
  {
    margin-top: 15px;
  }

  .editor-nested-item-header
  {
    display: grid;
    grid-template-columns: 1fr auto;
    align-items: center;
    font-size: var(--font-size);
    padding: 5px 0;
    color: var(--color-fg);
    position: relative;
    transition: color 0.2s ease;
    line-height: 1.5;

    &.has-line
    {
      border-bottom: 1px solid var(--color-bg);
      padding-bottom: 25px;
    }
  }

  .editor-nested-item-header-link
  {
    display: grid;
    grid-template-columns: 30px 1fr auto;
    grid-gap: 6px;
    height: 100%;
    align-items: center;
    position: relative;
    color: var(--color-fg);

    &.is-active
    {
      font-weight: bold;
      color: var(--color-secondary);

      .editor-nested-item-header-text span
      {
        font-weight: 400;
      }
    }
  }

  .editor-nested-item-header-text
  {
    display: flex;
    flex-direction: column;

    span
    {
      color: var(--color-fg-light);
      margin-top: 3px;
    }
  }

  .editor-nested-item-header-toggle
  {
    position: absolute;
    color: var(--color-fg-mid);
    height: 100%;
    top: 0;
    left: 0;
    width: 30px;
    text-align: right;
    padding-right: 5px;
    outline: none !important;
    transition: color 0.2s ease;

    &:hover
  {
    color: var(--color-fg);
  }

  }

  .editor-nested-item-header-icon
  {
    font-size: 18px;
    line-height: 1;
    font-weight: 400;
    position: relative;
    top: -2px;
    color: var(--color-fg);
    transition: color 0.2s ease;
  }

  .editor-nested-item-content
  {
    margin-top: 10px;
    //background: var(--color-bg-xlight);
    border: 1px solid var(--color-line-light);
    border-right: none;
    padding: 32px;
    margin-top: 10px;
    border-radius: var(--radius) 0 0 var(--radius);
    margin-right: -32px;
    padding-right: var(--padding);
    background: var(--color-box);
    box-shadow: var(--color-shadow-short);
  }

  .editor-nested .ui-property-label
  {
    width: 180px;
    padding-right: 40px;
  }

  .editor-nested .ui-property + .ui-property
  {
    //border-top: none;
  }
</style>