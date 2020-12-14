<template>
  <div class="ui-rte" :disabled="disabled">
    <div ref="editor" :id="id">
      <editor-menu-bubble :editor="editor" :keep-in-bounds="true" v-slot="{ commands, isActive, menu }">
        <div class="ui-rte-controls theme-dark" :class="{ 'is-active': menu.isActive }" :style="`left: ${menu.left}px; bottom: ${menu.bottom}px;`">
          <button type="button" class="ui-rte-control" :class="{ 'is-active': isActive.bold() }" @click="commands.bold">B</button>
          <button type="button" class="ui-rte-control" :class="{ 'is-active': isActive.italic() }" @click="commands.italic">I</button>
          <button type="button" class="ui-rte-control" :class="{ 'is-active': isActive.code() }" @click="commands.code">C</button>
        </div>
      </editor-menu-bubble>
      <editor-content class="ui-rte-input" :editor="editor" />
    </div>
  </div>
</template>


<script>
  import Strings from 'zero/helpers/strings.js';
  import { debounce as _debounce } from 'underscore';
  import { Editor, EditorContent, EditorMenuBubble } from 'tiptap';
  import
  {
    Blockquote, BulletList, CodeBlock, HardBreak, Heading, ListItem, OrderedList, TodoItem, TodoList,
    Bold, Code, Italic, Link, Strike, Underline, History
  }
  from 'tiptap-extensions';

  export default {
    name: 'uiRte',

    components: { EditorContent, EditorMenuBubble },

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
      id: 'quill-' + Strings.guid(),
      blocked: false,
      onDebouncedChange: null,
      editor: null
    }),

    watch: {
      value()
      {
        this.init();
      }
    },

    created()
    {
      this.onDebouncedChange = _debounce(this.onChange, 350);
    },

    mounted()
    {
      this.editor = new Editor({
        extensions: [
          new CodeBlock(),
          new HardBreak(),
          new Link(),
          new Bold(),
          new Code(),
          new Italic(),
          new Strike(),
          new Underline(),
          new History(),
        ],
        onUpdate: ({ getHTML }) =>
        {
          this.onDebouncedChange(getHTML());
        },
      });

      this.init();
    },

    methods: {
      init()
      {
        this.editor.setContent(this.value);
      },

      onChange(content)
      {
        // TODO this will automatically set the cursor to the end, not desired ;-)
        this.$emit('input', content);
      }
    },

    beforeDestroy()
    {
      this.editor.destroy();
    }
  }
</script>

<style lang="scss">

  .ui-rte-input
  {
    height: auto;
    min-height: 42px;
    padding-top: 9px;

    p
    {
      margin: 0;
    }

    p + p
    {
      margin-top: 1em;
    }
  }

  .ui-rte-controls
  {
    position: absolute;
    display: flex;
    z-index: 20;
    background: var(--color-bg);
    border-radius: var(--radius);
    padding: 5px;
    margin-bottom: 8px;
    transform: translateX(-50%);
    visibility: hidden;
    opacity: 0;
    transition: opacity 0.2s, visibility 0.2s;
    pointer-events: none;
  }

  .ui-rte-controls.is-active
  {
    opacity: 1;
    visibility: visible;
    pointer-events: auto;
  }

  .ui-rte-control
  {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    background: transparent;
    border: 0;
    color: var(--color-text);
    height: 32px;
    width: 32px;
    margin-right: 4px;
    border-radius: var(--radius);
    cursor: pointer;
  }

  .ui-rte-control:hover
  {
    background-color: var(--color-bg-shade-1);
  }

  .ui-rte-control.is-active
  {
    background-color: var(--color-bg-shade-1);
    color: var(--color-text-dim);
  }
</style>