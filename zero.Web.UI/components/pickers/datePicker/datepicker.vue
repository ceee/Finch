<template>
  <div class="ui-datepicker" :class="{'is-disabled': disabled }">
    <input type="text" class="ui-input ui-datepicker-input" v-localize:placeholder="'@ui.date.select'" :value="output" @input="onChange" @focus="onFocus" @blur="onBlur" :disabled="disabled" />
    <i v-if="!clear || !value" class="fth-calendar ui-datepicker-icon"></i>
    <button v-if="clear && value" type="button" class="ui-datepicker-input-button" @click="clearInput"><i class="fth-x"></i></button>

    <ui-dropdown ref="overlay" class="ui-datepicker-overlay" @opened="overlayOpened">
      <datepicker-overlay :value="value" @change="onSelect" :options="pickerOptions" />
    </ui-dropdown>
  </div>
</template>


<script>
  import Strings from '@zero/services/strings.js';
  import DatepickerOverlay from './overlay.vue';
  import { extend as _extend } from 'underscore';
  import dayjs from 'dayjs';

  const DATETIME_FORMAT = 'DD.MM.YY HH:mm';
  const DATE_FORMAT = 'DD.MM.YY';

  export default {
    name: 'uiDatepicker',

    emits: ['change', 'input'],

    components: { DatepickerOverlay },

    props: {
      value: {
        type: String,
        default: null
      },
      clear: {
        type: Boolean,
        default: true
      },
      disabled: {
        type: Boolean,
        default: false
      },
      format: {
        type: String,
        default: null
      },
      time: {
        type: Boolean,
        default: false,
      },
      max: {
        type: [String, Date],
        default: null
      },
      min: {
        type: [String, Date],
        default: null
      },
      amPm: {
        type: Boolean,
        default: false
      },
      options: {
        type: Object,
        default: () => { }
      }
    },


    data: () => ({
      id: null,
      output: null,
      pickerOptions: {}
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


    mounted()
    {
      this.id = 'datepicker-' + Strings.guid();
      this.updateOptions();
      this.updateOutput();
    },


    methods: {

      updateOutput()
      {
        this.output = Strings.date(this.value, this.format || (this.time ? DATETIME_FORMAT : DATE_FORMAT));
      },


      updateOptions()
      {
        this.pickerOptions = {
          enableTime: this.time,
          maxDate: this.max,
          minDate: this.min,
          time_24hr: !this.amPm
        };
      },


      onSelect(date)
      {
        let dateStr = dayjs(date).toISOString();
        this.setValue(dateStr);
        this.$refs.overlay.hide();
        document.activeElement.blur();
      },


      onChange(ev)
      {
        this.setValue(ev.target.value);
      },


      setValue(value)
      {
        this.$emit('change', value);
        this.$emit('input', value);
      },


      onFocus()
      {
        this.$refs.overlay.show();
      }, 


      onBlur()
      {
        if (!this.$refs.overlay.open)
        {
          this.$refs.overlay.hide();
        }
      },


      clearInput()
      {
        this.setValue(null);
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

  .ui-datepicker-overlay .ui-dropdown
  {
    padding: 0;
  }

  .ui-datepicker-input-button
  {
    position: absolute;
    right: 0;
    top: 0;
    height: 100%;
    width: 40px;
    text-align: center;
    font-size: var(--font-size);
    padding-top: 1px;
  }
</style>