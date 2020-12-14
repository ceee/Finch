<template>
  <div class="ui-rte" :disabled="disabled">
    <div ref="editor" :id="id" class="pell" v-show="!raw"></div>
    <div class="pell-raw" v-show="raw">
      <div class="pell-actionbar">
        <button class="pell-button" title="Accept" type="button" @click="acceptHtml"><i class="fth-check"></i></button>
        <button class="pell-button" title="Cancel" type="button" @click="raw = false"><i class="fth-x"></i></button>
      </div>
      <pre class="pell-raw-content"></pre>
    </div>
  </div>
</template>


<script>
  import Strings from 'zero/helpers/strings.js';
  import Pell from './rte.pell.dependency.js';

  export default {
    name: 'uiRte',

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
      id: 'rte-' + Strings.guid(),
      blocked: false,
      actions: [
        'bold',
        'underline',
        'italic',
        'strikethrough',
        'olist',
        'ulist',
        'line'
      ]
    }),

    watch: {
      value()
      {
        this.setValue();
      }
    },

    created()
    {
      this.actions.push({
        name: 'link',
        result: this.onLinkCreate
      });

      this.actions.push({
        name: 'raw',
        icon: '<i class="fth-edit-2"></i>',
        title: 'Edit HTML',
        result: () =>
        {
          this.$el.querySelector('.pell-raw-content').innerText = this.value;
          this.raw = true;
        }
      });
    },

    mounted()
    {
      this.initialize();
    },

    methods: {

      initialize()
      {
        this.$el.querySelector('.pell-raw-content').contentEditable = true;

        this.editor = Pell.init({
          element: this.$refs.editor,
          defaultParagraphSeparator: 'p',
          onChange: html =>
          {
            this.blocked = true;
            this.$emit('input', html);
            this.blocked = false;
            //setValue(html);
          },
          actions: this.actions
        });

        this.editor.content.innerHTML = this.value;
        //this.setValue();
      },


      setValue()
      {
        if (this.editor.content.innerHTML !== this.value)
        {
          this.editor.content.innerHTML = this.value;
        }
      },


      onLinkCreate()
      {

      },


      acceptHtml()
      {
        var html = this.$el.querySelector('.pell-raw-content').innerText;
        this.editor.content.innerHTML = html;
        this.raw = false;
      }

    }
  }
</script>

<style lang="scss">
  .ui-rte
  {
    font-size: var(--font-size);

    .pell,
    .pell-raw
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

      &:focus-within
      {
        background: var(--color-input-focus-bg) !important;
        border: var(--color-input-focus-border);
        box-shadow: var(--color-input-focus-shadow);
        outline: none;

        .pell-button:hover,
        .pell-button-selected
        {
          background: var(--color-box-nested);
          color: var(--color-text);
        }
      }
    }

    .pell, .pell-content, .pell-raw
    {
      box-sizing: border-box;
    }

    .pell-content,
    .pell-raw-content
    {
      min-height: 60px;
      outline: none;
      overflow-y: auto;
      padding: 6px 12px 12px;
      max-height: 400px;
    }

    pre.pell-raw-content
    {
      margin: 0;
      background: none;
      border: none;
      word-wrap: normal;
      white-space: normal;
      font-family: Monaco,Menlo,Consolas,Courier New,monospace;
      font-size: var(--font-size);
    }

    u-rte.-oneline .pell-content
    {
      min-height: 46px;
    }

    .pell-content
    {
      p
      {
        margin: 0;
      }

      h3, h2
      {
        margin-bottom: .5em;
        margin-top: 1em;
      }
    }

    .pell-content a, .pell-content a:hover, .pell-content a:visited, .pell-content a:focus
    {
      text-decoration: underline;
      color: var(--color-text);
    }

    .pell-actionbar
    {
      border-radius: var(--radius) var(--radius) 0 0;
      padding: 5px;
    }

    .pell-button
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

    .pell-button + .pell-button
    {
      margin-left: 15px;
    }

    .pell-button:hover,
    .pell-button-selected
    {
      background: var(--color-box);
      color: var(--color-text);
    }
  }
</style>