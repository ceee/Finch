<template>
  <div class="ui-rte" :disabled="disabled">
    <div ref="editor" :id="id">
      <editor-menu-bubble :editor="editor" :keep-in-bounds="true" v-slot="{ commands, isActive, menu }">
        <div class="ui-rte-overlay-controls theme-dark" :class="{ 'is-active': menu.isActive }" :style="`left: ${menu.left}px; bottom: ${menu.bottom}px;`">
          <button type="button" class="ui-rte-overlay-control" :class="{ 'is-active': isActive.bold() }" @click="commands.bold"><i class="fth-bold"></i></button>
          <button type="button" class="ui-rte-overlay-control" :class="{ 'is-active': isActive.italic() }" @click="commands.italic"><i class="fth-italic"></i></button>
          <button type="button" class="ui-rte-overlay-control" :class="{ 'is-active': isActive.underline() }" @click="commands.underline"><i class="fth-underline"></i></button>
          <button type="button" class="ui-rte-overlay-control" :class="{ 'is-active': isActive.link() }" @click="commands.link"><i class="fth-link"></i></button>
          <button type="button" class="ui-rte-overlay-control" :class="{ 'is-active': isActive.code() }" @click="commands.code"><i class="fth-code"></i></button>
        </div>
      </editor-menu-bubble>
      <editor-menu-bar :editor="editor" v-slot="{ commands, isActive }">
        <div class="ui-rte-controls">
          <span class="ui-rte-controls-label">Editor</span>
          <button type="button" title="Undo" class="ui-rte-control" @click="commands.undo" :disabled="commands.undoDepth() < 1"><i class="fth-chevron-left"></i></button>
          <button type="button" title="Redo" class="ui-rte-control" @click="commands.redo" :disabled="commands.redoDepth() < 1"><i class="fth-chevron-right"></i></button>
          <!--<button type="button" class="ui-rte-control" :class="{ 'is-active': isActive.bold() }" @click="commands.bold"><i class="fth-bold"></i></button>
          <button type="button" class="ui-rte-control" :class="{ 'is-active': isActive.italic() }" @click="commands.italic"><i class="fth-italic"></i></button>
          <button type="button" class="ui-rte-control" :class="{ 'is-active': isActive.underline() }" @click="commands.underline"><i class="fth-underline"></i></button>
          <button type="button" class="ui-rte-control" :class="{ 'is-active': isActive.link() }" @click="commands.link"><i class="fth-link"></i></button>
          <button type="button" class="ui-rte-control" :class="{ 'is-active': isActive.code() }" @click="commands.code"><i class="fth-code"></i></button>-->
        </div>
      </editor-menu-bar>
      <editor-content class="ui-rte-input" :editor="editor" />
    </div>
  </div>
</template>


<script>
  import Strings from 'zero/helpers/strings.js';
  import { debounce as _debounce } from 'underscore';
  import { Editor, EditorContent, EditorMenuBubble, EditorMenuBar } from 'tiptap';
  import
  {
    Blockquote, BulletList, CodeBlock, HardBreak, Heading, ListItem, OrderedList, TodoItem, TodoList,
    Bold, Code, Italic, Link, Strike, Underline, History
  }
  from 'tiptap-extensions';

  export default {
    name: 'uiRte',

    components: { EditorContent, EditorMenuBubble, EditorMenuBar  },

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
        //this.$emit('input', content);
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

    a
    {
      color: var(--color-primary);
      text-decoration: underline;
      cursor: pointer;
    }
  }

  .ui-rte-overlay-controls
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

    &:after
    {
      content: '';
      position: absolute;
      left: 50%;
      top: 100%;
      width: 0;
      height: 0;
      border: 8px solid transparent;
      margin-left: -7px;
      border-top-color: var(--color-bg);
    }
  }

  .ui-rte-overlay-controls.is-active
  {
    opacity: 1;
    visibility: visible;
    pointer-events: auto;
  }

  .ui-rte-overlay-control
  {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    background: transparent;
    border: 0;
    color: var(--color-text);
    height: 32px;
    width: 32px;
    border-radius: var(--radius);
    cursor: pointer;
  }

  .ui-rte-overlay-control + .ui-rte-overlay-control
  {
    margin-left: 4px;
  }

  .ui-rte-overlay-control:hover
  {
    background-color: var(--color-bg-shade-1);
  }

  .ui-rte-overlay-control.is-active
  {
    background-color: var(--color-bg-shade-1);
    color: var(--color-text-dim);
  }


  .ui-rte-controls
  {
    background: var(--color-input);
    border-radius: var(--radius) var(--radius) 0 0;
    padding: 5px 5px 0;
    display: flex;
    align-items: center;
    justify-content: flex-start;
  }

  .ui-rte-controls + .ui-rte-input
  {
    border-top-left-radius: 0;
    border-top-right-radius: 0;
  }

  .ui-rte-controls-label
  {
    color: var(--color-text-dim);
    margin: 0 14px 0 8px;
    font-size: 10px;
    text-transform: uppercase;
    pointer-events: none;
    user-select: none;
  }

  .ui-rte-control
  {
    background-color: transparent;
    cursor: pointer;
    height: 30px;
    width: 30px;
    text-align: center;
    border-radius: 5px;
    font-size: 14px;
    vertical-align: bottom;
  }

  .ui-rte-control[disabled]
  {
    opacity: .5;
  }

  .ui-rte-control + .ui-rte-control
  {
    margin-left: 5px;
  }

  .ui-rte-control:hover, .ui-rte-control.is-active
  {
    background: var(--color-box);
    color: var(--color-text);
  }
</style>