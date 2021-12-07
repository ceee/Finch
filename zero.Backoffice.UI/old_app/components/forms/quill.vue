<template>
  <div class="ui-quill" :disabled="disabled">
    <div ref="editor" :id="id">
      <editor-menu-bubble :editor="editor" :keep-in-bounds="keepInBounds" v-slot="{ commands, isActive, menu }">
        <div class="menububble"
             :class="{ 'is-active': menu.isActive }"
             :style="`left: ${menu.left}px; bottom: ${menu.bottom}px;`">

          <button type="button" class="menububble__button"
                  :class="{ 'is-active': isActive.bold() }"
                  @click="commands.bold">
            B
          </button>

          <button type="button" class="menububble__button"
                  :class="{ 'is-active': isActive.italic() }"
                  @click="commands.italic">
            I
          </button>

          <button type="button" class="menububble__button"
                  :class="{ 'is-active': isActive.code() }"
                  @click="commands.code">
            C
          </button>

        </div>
      </editor-menu-bubble>

      <editor-content class="editor__content" :editor="editor" />
    </div>
  </div>
</template>


<script>
  import Strings from 'zero/helpers/strings.js';
  import { Editor, EditorContent, EditorMenuBubble } from 'tiptap'
  import
    {
      Blockquote,
      BulletList,
      CodeBlock,
      HardBreak,
      Heading,
      ListItem,
      OrderedList,
      TodoItem,
      TodoList,
      Bold,
      Code,
      Italic,
      Link,
      Strike,
      Underline,
      History,
    } from 'tiptap-extensions'

  export default {
    name: 'uiQuill',

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
      keepInBounds: true,
      editor: new Editor({
        extensions: [
          new Blockquote(),
          new BulletList(),
          new CodeBlock(),
          new HardBreak(),
          new Heading({ levels: [1, 2, 3] }),
          new ListItem(),
          new OrderedList(),
          new TodoItem(),
          new TodoList(),
          new Link(),
          new Bold(),
          new Code(),
          new Italic(),
          new Strike(),
          new Underline(),
          new History(),
        ],
      })
    }),

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

    },

    beforeDestroy()
    {
      this.editor.destroy();
    },
  }
</script>

<style lang="scss">
  .menububble {
  position: absolute;
  display: flex;
  z-index: 20;
  background: black;
  border-radius: 5px;
  padding: 0.3rem;
  margin-bottom: 0.5rem;
  transform: translateX(-50%);
  visibility: hidden;
  opacity: 0;
  transition: opacity 0.2s, visibility 0.2s;

  &.is-active {
    opacity: 1;
    visibility: visible;
  }

  &__button {
    display: inline-flex;
    background: transparent;
    border: 0;
    color: white;
    padding: 0.2rem 0.5rem;
    margin-right: 0.2rem;
    border-radius: 3px;
    cursor: pointer;

    &:last-child {
      margin-right: 0;
    }

    &:hover {
      background-color: rgba(white, 0.1);
    }

    &.is-active {
      background-color: rgba(white, 0.2);
    }
  }

  &__form {
    display: flex;
    align-items: center;
  }

  &__input {
    font: inherit;
    border: none;
    background: transparent;
    color: white;
  }
}
</style>