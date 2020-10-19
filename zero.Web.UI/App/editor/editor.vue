<template>
  <div>
    <div class="editor-outer" v-if="loaded" :class="{ 'has-tabs': hasTabs }">
      <ui-tabs class="editor">
        <ui-tab v-if="!tab.disabled(value)" v-for="(tab, index) in tabs" class="ui-box" :class="tab.class" :label="tab.name" :count="tab.count(value)" :key="index">
          <editor-component v-for="(field, fieldIndex) in tab.fields" :key="fieldIndex" :config="field" @input="onChange" :editor="editorConfig" :value="value" />
        </ui-tab>
      </ui-tabs>
    </div>
  </div>
</template>


<script>
  import EditorComponent from 'zero/editor/editor-component.vue';
  import Editor from 'zero/core/editor.js';

  export default {
    name: 'uiEditor',

    provide: function ()
    {
      return {
        meta: this.meta,
        disabled: false
      };
    },

    props: {
      config: {
        type: [String, Editor],
        required: true
      },
      meta: {
        type: Object,
        default: () => { }
      },
      value: {
        type: Object
      }
    },

    components: { EditorComponent },

    data: () => ({
      editorConfig: null,
      loaded: false,
      hasTabs: false,
      tabs: []
    }),

    created()
    {
      this.editorConfig = typeof this.config === 'string' ? this.zero.getEditor(this.config) : this.config;

      if (this.editorConfig.tabs.length > 0)
      {
        this.hasTabs = true;
        this.tabs = this.editorConfig.tabs.map(tab =>
        {
          return {
            ...tab,
            count: value => typeof tab.count === 'number' ? tab.count : (typeof tab.count === 'function' ? tab.count(value) : 0),
            disabled: value => typeof tab.disabled === 'boolean' ? tab.disabled : (typeof tab.disabled === 'function' ? tab.disabled(value) : false),
            fields: this.editorConfig.getFields(tab)
          };
        });
      }
      else
      {
        this.tabs = [{
          name: '_',
          count: value => 0,
          disabled: value => false,
          fields: this.editorConfig.getFields()
        }];
      }

      this.loaded = true;
    },

    methods: {

      onChange()
      {
        this.$emit('input', this.value);
      }
    }
  }
</script>

<style lang="scss">
  .editor > .ui-view-box
  {
    padding-top: 0;
  }

  .editor
  {
    .ui-tabs-list
    {
      padding-top: 0;
    }
  }

  .editor-outer
  {
    &.-infos-aside:not(.is-page)
    {
      display: grid;
      grid-template-columns: 1fr 340px var(--padding);
      align-items: flex-start;
    }

    &:not(.has-tabs) 
    {
      .ui-tabs-list
      {
        display: none;
      }

      .ui-tab, .editor-infos
      {
        margin-top: 0;
      }
    }
  }

  .editor-infos
  {
    margin: 50px 0 0;
    position: sticky;
    top: var(--padding);

    .ui-box
    {
      margin-left: 0;
      margin-right: 0;
      margin-bottom: 0;
    }

    .ui-box:first-child
    {
      margin-top: 0;
    }

    .ui-box + .ui-box
    {
      margin-top: 16px;
    }

    .ui-box + .ui-box.is-light
    {
      margin-top: 1px;
      border-top-left-radius: 0;
      border-top-right-radius: 0;
    }

    .ui-property + .ui-property
    {
      margin-top: 10px;
      padding-top: 10px;
      border-top: none;
    }

    .is-toggle
    {
      align-items: center;
    }

    .ui-property-label
    {
      width: auto;
      padding-right: 20px;
    }

    .ui-property-content
    {
      flex: 0 1 auto;
    }

    .ui-property
    {
      display: flex;
      justify-content: space-between;
    }
  }

  .editor-active-toggle
  {
    border-bottom-left-radius: 0;
    border-bottom-right-radius: 0;
  }

  /*.editor-active-toggle.is-active
  {
    background: var(--color-accent-info-bg);   

    .ui-property-label
    {
      color: var(--color-accent-info);
    }
  }*/


  .editor-global-flag
  {
    display: grid;
    grid-template-columns: auto minmax(0, 1fr);
    grid-gap: 16px;
    padding-bottom: 10px;
    align-items: center;
    font-size: var(--font-size);
    line-height: 1.5;
    position: relative;

    p
    {
      margin: 0;
      color: var(--color-text);
    }

    i
    {
      font-size: 24px;
      color: var(--color-primary);
      margin-top: -2px;
    }

    & + .ui-button
    {
      margin-top: 1.2rem;
    }

    & + .ui-property
    {
      margin-top: 32px;
      padding-top: 32px;
      border-top: 1px solid var(--color-line);
    }

    &.is-block
    {
      padding: 20px;
      padding-right: 70px;
      border-radius: var(--radius);
      background: var(--color-box-nested);
      margin-bottom: var(--padding);

      i
      {
        margin-top: -22px;
        right: 20px;
      }
    }
  }

  .editor-error
  {
    background: transparent;
    min-height: calc(100vh - 100px);
  }
</style>