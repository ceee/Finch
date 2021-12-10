<template>
  <div class="ui-colorpicker" :class="{'is-disabled': disabled }">
    <label class="ui-colorpicker-color" :for="id">
      <span class="ui-colorpicker-color-preview" :style="{ 'background-color': value || 'var(--color-box)' }"></span>
      <input :id="id" type="color" :value="value" :disabled="disabled" @input="onChange" />
    </label>
    <input type="text" maxlength="7" class="ui-colorpicker-input" :value="value" @input="onChange" :disabled="disabled" v-localize:placeholder="'@colorpicker.placeholder'" />
  </div>
</template>


<script>
  import { generateId } from '../../utils/numbers';

  export default {
    name: 'uiColorpicker',

    props: {
      value: {
        type: String,
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      },
      options: {
        type: Object,
        default: () =>
        {
          return {

          };
        }
      }
    },

    data: () => ({
      id: null
    }),

    created()
    {
      this.id = 'colorpicker-' + generateId();
    },

    methods: {

      onChange(ev)
      {
        this.$emit('change', ev.target.value);
        this.$emit('input', ev.target.value);
        // TODO this does not trigger the forms dirty flag
      }

      //pick()
      //{
      //  this.$el.
      //}
      
    }
  }
</script>

<style lang="scss">
  .ui-colorpicker
  {
    position: relative;
  }

  .ui-colorpicker-color
  {
    display: inline-block;
    position: absolute;
    overflow: hidden;
    width: 32px;
    height: 100%;
    border-radius: 3px;
    left: 0;
    top: -1px;
    padding: 0 !important;
    cursor: pointer;
  }

  .ui-colorpicker-color-preview
  {
    position: absolute;
    left: 12px;
    top: 50%;
    margin-top: -8px;
    width: 16px;
    height: 16px;
    border-radius: 2px;
    box-shadow: var(--shadow-short);
  }

  .ui-colorpicker-color input
  {
    opacity: 0 !important;
    visibility: hidden !important;
    position: absolute;
    left: 0;
    top: 0;
    bottom: 0;
    right: 0;
  }

  input[type="text"].ui-colorpicker-input
  {
    padding-left: 40px;
    max-width: 322px;    
  }


  .ui-colorpicker-preview
  {
    display: inline-block;
    margin-top: -11px;
    border: 1px solid #eceaea;
    cursor: pointer;
  }

</style>