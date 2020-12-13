<template>
  <div class="ui-quill" :disabled="disabled">
    <div ref="editor" :id="id" v-show="!raw"></div>
  </div>
</template>


<script>
  import Strings from 'zero/helpers/strings.js';
  import Pell from './rte.pell.dependency.js';
  import Quill from 'quill';
  import 'quill/dist/quill.bubble.css';

  export default {
    name: 'uiQuill',

    props: {
      value: {
        type: String,
        default: ''
      },
      disabled: {
        type: Boolean,
        default: false
      },
    },

    data: () => ({
      raw: false,
      editor: null,
      id: 'quill-' + Strings.guid(),
      blocked: false
    }),

    watch: {
      value()
      {
        this.setValue();
      }
    },

    created()
    {
      
    },

    mounted()
    {
      this.initialize();
    },

    methods: {

      initialize()
      {
        this.quill = new Quill(this.$refs.editor, {
          theme: 'bubble',
          placeholder: 'Write something...',
          modules: {
            toolbar: [
              ['bold', 'italic', 'underline', 'strike', 'code', 'link'],
              ['clean']
            ]
          },
          formats: ['bold','italic','underline','strike','code','link']
        });

        this.$refs.editor.querySelector('.ql-tooltip').classList.add('theme-dark');
      },


      setValue()
      {
        
      },


      onLinkCreate()
      {

      },


      acceptHtml()
      {
        
      }

    }
  }
</script>

<style lang="scss">
  .ui-quill
  {
    font-size: var(--font-size);
  }

  .ui-quill .ql-editor
  {
    line-height: 1.5;
    padding: 10px 12px 8px;
  }

  .ui-quill .ql-editor.ql-blank::before
  {
    left: 12px;
    color: var(--color-input-placeholder);
    font-style: normal;
  }

  .ui-quill .ql-container
  {
    background: var(--color-input);
    max-width: 800px;
    min-height: 42px;
    display: flex;
    width: 100%;
    flex-direction: column;
    font-size: var(--font-size);
    display: inline-block;
    line-height: 1.5;
    color: var(--color-text);
    border-radius: var(--radius);
    border: 1px solid transparent;
    font-family: var(--font);
  }

  .ui-quill .ql-container:focus, .ui-quill .ql-container:focus-within
  {
    background-color: var(--color-input-focus-bg);
    border: var(--color-input-focus-border);
    box-shadow: var(--color-input-focus-shadow);
    outline: none;
  }

  .ui-quill .ql-tooltip
  {
    color: var(--color-text);
    background-color: var(--color-dropdown);
    border-radius: var(--radius);
  }

  .ui-quill .ql-toolbar
  {
    padding: 3px 6px;
  }

  .ui-quill .ql-tooltip-arrow
  {
    border-bottom-color: var(--color-dropdown) !important;
  }

  .ui-quill .ql-toolbar button
  {
    height: 36px;
    width: 36px;
    padding: 8px 10px;  
  }

  .ui-quill .ql-toolbar button svg
  {
    height: 85%;
  }

  .ui-quill .ql-toolbar .ql-formats
  {
    margin: 0 !important;
  }

  .ui-quill .ql-bubble .ql-fill
  {
    fill: var(--color-text);
  }
  .ui-quill .ql-bubble .ql-stroke
  {
    stroke: var(--color-text);
  }
</style>