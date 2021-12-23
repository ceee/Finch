import { Plugin, PluginKey } from 'prosemirror-state'

class Menu
{

  constructor({ options })
  {
    this.options = options
    this.preventHide = false

    // the mousedown event is fired before blur so we can prevent it
    this.mousedownHandler = this.handleClick.bind(this)
    this.options.element.addEventListener('mousedown', this.mousedownHandler, { capture: true })

    this.blurHandler = () =>
    {
      if (this.preventHide)
      {
        this.preventHide = false
        return
      }

      this.options.editor.emit('menubar:focusUpdate', false)
    }
    this.options.editor.on('blur', this.blurHandler)
  }

  handleClick()
  {
    this.preventHide = true
  }

  destroy()
  {
    this.options.element.removeEventListener('mousedown', this.mousedownHandler)
    this.options.editor.off('blur', this.blurHandler)
  }

}

const MenuBar = function (options)
{
  return new Plugin({
    key: new PluginKey('menu_bar'),
    view(editorView)
    {
      return new Menu({ editorView, options })
    },
  })
};



export default {

  props: {
    editor: {
      default: null,
      type: Object,
    },
  },

  data() {
    return {
      focused: false,
    }
  },

  watch: {
    editor: {
      immediate: true,
      handler(editor) {
        if (editor) {
          this.$nextTick(() =>
          {
            var menubar = MenuBar({
              editor,
              element: this.$el,
            });

            editor.registerPlugin(menubar);

            this.focused = editor.focused

            editor.on('focus', () => {
              this.focused = true
            })

            editor.on('menubar:focusUpdate', focused => {
              this.focused = focused
            })
          })
        }
      },
    },
  },

  render() {
    if (!this.editor) {
      return null
    }

    return this.$slots.default({
      focused: this.focused,
      focus: this.editor.focus,
      commands: this.editor.commands,
      isActive: this.editor.isActive,
      getMarkAttrs: this.editor.getMarkAttrs.bind(this.editor),
      getNodeAttrs: this.editor.getNodeAttrs.bind(this.editor),
    })
  },

}