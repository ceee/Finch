<template>
  <div class="ui-datepicker" :class="{'is-disabled': disabled }">
    <input type="text" class="ui-input ui-datepicker-input" :value="output" @input="onChange" @focus="onFocus" @blur="onBlur" :disabled="disabled" />
    <i class="fth-calendar ui-datepicker-icon"></i>

    <ui-dropdown ref="overlay" class="ui-datepicker-overlay" @opened="overlayOpened">
      <div>
        <datepicker-overlay />
      </div>
    </ui-dropdown>
  </div>
</template>


<script>
  import Strings from 'zero/services/strings';
  import DatepickerOverlay from './overlay';
  import { extend as _extend } from 'underscore';

  export default {
    name: 'uiDatepicker',

    components: { DatepickerOverlay },

    props: {
      value: {
        type: String,
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      },
      format: {
        type: String,
        default: 'DD.MM.YY HH:mm'
      },
      options: {
        type: Object,
        default: () => { }
      }
    },


    data: () => ({
      id: null,
      output: null
    }),


    watch: {
      value()
      {
        this.updateOutput();
      },
      format()
      {
        this.updateOutput();
      }
    },


    created()
    {
      this.id = 'datepicker-' + Strings.guid();
    },


    methods: {

      updateOutput()
      {
        this.output = Strings.date(this.value, this.format);
      },


      onChange(ev)
      {
        this.$emit('change', ev.target.value);
        this.$emit('input', ev.target.value);
        // TODO this does not trigger the forms dirty flag
      },


      onFocus()
      {
        this.$refs.overlay.show();
      }, 


      onBlur()
      {
        this.$refs.overlay.hide();
      },


      overlayOpened()
      {

      }

      //pick()
      //{
      //  this.$el.
      //}
      
    }
  }
</script>

<style lang="scss">
  .ui-datepicker
  {
    position: relative;
    max-width: 260px;
  }

  input[type="text"].ui-datepicker-input
  {
    padding-right: 36px;
  }

  .ui-datepicker-icon
  {
    position: absolute;
    right: 13px;
    top: 50%;
    margin-top: -8px;
    font-size: var(--font-size-l);
  }

</style>