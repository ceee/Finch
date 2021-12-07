<template>
  <div class="ui-rte" :id="id" :disabled="disabled">

    <!--<bubble-menu :editor="editor" :tippy-options="{ duration: 100 }" v-if="editor && hasBubble">
      <div class="ui-rte-overlay-controls theme-dark is-active">
        <div class="ui-rte-overlay-control-outer" v-for="cmd in editor.cmds" v-if="cmd && cmd.bubble && !cmd.isParent" :key="cmd.id">
          <button type="button" :data-alias="cmd.alias" v-localize:title="cmd.title" class="ui-rte-overlay-control" :class="{ 'is-active': cmd.isActive(editor) }" @click="cmd.onClick(editor)">
            <ui-icon :symbol="cmd.symbol" :size="cmd.symbolSize" />
          </button>
        </div>
      </div>
    </bubble-menu>-->

    <div class="ui-rte-controls" v-if="editor">
      <div class="ui-rte-control-outer" v-for="cmd in cmds" :key="cmd.id">
        <button type="button" v-if="!cmd.isParent" :data-alias="cmd.alias" v-localize:title="cmd.title" class="ui-rte-control" :class="{ 'is-active': cmd.isActive(editor) }" @click="cmd.onClick(editor)">
          <ui-icon :symbol="cmd.symbol" :size="cmd.symbolSize" />
        </button>
        <!--<ui-dropdown v-if="cmd.isParent" align="right">
          <template v-slot:button>
            <button :data-alias="cmd.alias" type="button" v-localize:title="cmd.title" class="ui-rte-control" :class="{ 'is-active': cmd.isActive(editor) }">
              <ui-icon :symbol="cmd.symbol" :size="cmd.symbolSize" />
            </button>
          </template>
          <ui-dropdown-button v-for="child in cmd.children" :key="child.id" :label="child.title" @click="child.onClick(editor)" />
        </ui-dropdown>-->
      </div>
    </div>

    <editor-content class="ui-rte-input" :editor="editor" />

  </div>
</template>


<script lang="ts">
  import { generateId } from '../../../utils/numbers';
  import { debounce } from '../../../utils/timing';
  import { Editor, EditorContent, BubbleMenu } from '@tiptap/vue-3';
  // import { Placeholder } from '@tiptap/vue-3';
  import EditorMenuBar from './rte.menubar';
  //import { MaxSize } from './rte.extensions';
  import createConfig from './rte.config';

  export default {
    name: 'uiRte',

    components: { EditorContent, BubbleMenu, EditorMenuBar },

    props: {
      modelValue: {
        type: String,
        default: ''
      },
      disabled: {
        type: Boolean,
        default: false
      },
      maxLength: {
        type: Number,
        default: null
      },
      placeholder: {
        type: String,
        default: null
      },
      setup: {
        type: Function,
        default: () => { }
      }
    },

    data: () => ({
      id: 'rte-' + generateId(),
      blocked: false,
      onDebouncedChange: null,
      editor: null,
      extensions: [],
      cmds: []
    }),

    watch: {
      modelValue()
      {
        if (!this.blocked)
        {
          this.init();
        }
      },
      disabled(value)
      {
        this.editor.setOptions({
          editable: !value
        })
      },
    },

    computed: {
      hasBubble()
      {
        return this.cmds.find(x => x.bubble) != null;
      }
    },

    created()
    {
      this.onDebouncedChange = debounce(this.onChange, 350);
    },

    mounted()
    {
      var config = createConfig();

      if (typeof this.setup === 'function')
      {
        this.setup(config);
      }

      this.extensions = config.extensions;
      this.cmds = config.commands.map(cmd =>
      {
        let params = this.mapCommand(cmd);

        if (cmd.children && cmd.children.length)
        {
          params.isParent = true;
          params.children = cmd.children.map(child => this.mapCommand(child));
        }

        return params;
      });

      //if (this.placeholder)
      //{
      //  this.extensions.push(new Placeholder({
      //    emptyEditorClass: 'is-editor-empty',
      //    emptyNodeClass: 'is-empty',
      //    emptyNodeText: this.placeholder,
      //    showOnlyWhenEditable: true,
      //    showOnlyCurrent: true
      //  }));
      //}

      //if (this.maxLength > 0)
      //{
      //  this.extensions.push(new MaxSize({ maxSize: this.maxLength }));
      //}

      this.editor = new Editor({
        editable: !this.disabled,
        extensions: this.extensions,
        onUpdate: ({ editor }) =>
        {
          this.onDebouncedChange(editor.getHTML());
        }
      });

      this.editor.cmds = this.cmds;

      this.init();
    },

    methods: {
      init()
      {
        this.editor.commands.setContent(this.modelValue, false);
      },

      onChange(content)
      {
        this.blocked = true;
        this.$emit('update:modelValue', content);
        this.$nextTick(() => this.blocked = false);
      },

      mapCommand(cmd)
      {
        return {
          id: generateId(),
          alias: cmd.alias,
          title: cmd.title,
          symbol: cmd.symbol,
          symbolSize: cmd.symbolSize || 17,
          isActive: typeof cmd.isActive === 'function' ? cmd.isActive : () => false,
          disabled: typeof cmd.disabled === 'function' ? cmd.disabled : () => false,
          onClick: typeof cmd.onClick === 'function' ? cmd.onClick : () => { },
          bubble: cmd.bubble || false
        };
      }
    },

    beforeDestroy()
    {
      this.editor.destroy();
    }
  }
</script>

<style lang="scss">

  .ui-rte
  {
    background: var(--color-input);
    border-radius: var(--radius);
    border: var(--color-input-border);
  }

  .ui-rte:focus-within
  {
    background-color: var(--color-input-focus-bg);
    border: var(--color-input-focus-border);
    box-shadow: var(--color-input-focus-shadow);
    outline: none;
  }

  .ui-rte-input
  {
    height: auto;
    min-height: 48px;
    padding-top: 9px;
    max-height: 420px;
    overflow-y: auto;
    border: none !important;

    .ProseMirror:focus
    {
      outline: none;
    }

    p
    {
      margin: 0;
    }

    p + p
    {
      margin-top: 1em;
    }

    p.is-editor-empty:first-child:before
    {
      content: attr(data-empty-text);
      float: left;
      color: var(--color-input-placeholder);
      opacity: .7;
      pointer-events: none;
      height: 0;
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
    background: none;
    padding: 5px 5px 0;
    display: flex;
    align-items: center;
    justify-content: flex-start;
    border-bottom: var(--color-input-border);
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

  .ui-rte-control-outer + .ui-rte-control-outer
  {
    margin-left: 5px;
  }

  .ui-rte-control:hover, .ui-rte-control.is-active
  {
    background: var(--color-box-nested);
    color: var(--color-text);
  }

  .ui-rte-input
  {
    background: none !important;
    border-radius: 0;
  }

  .ui-rte-input .ProseMirror
  {
    > *
    {
      margin: 1rem 0;
    }

    > :first-child
    {
      margin-top: 0;
    }

    > :last-child
    {
      margin-bottom: 0;
    }

    h1, h2, h3, h4, h5, h6
    {
      margin-bottom: .5rem;
      font-weight: bold;
    }

    h1
    {
      font-size: 1.4em;
    }

    h2
    {
      font-size: 1.2em;
    }

    h3
    {
      font-size: 1.1em;
    }

    h4, h5, h6
    {
      font-size: 1em;
    }

    hr
    {
      display: block;
      border-bottom-style: dashed;
      border-bottom-color: var(--color-line-dashed);
    }

    li p
    {
      display: block;
      line-height: 1.5;
    }
  }
</style>