<template>
  <div class="ui-timepicker" :class="{'is-disabled': disabled }">
    <input type="text" class="ui-input ui-timepicker-input" v-localize:placeholder="placeholder" :value="output" @input="onChange" @focus="onFocus" @blur="onBlur" :disabled="disabled" />
    <ui-icon v-if="!clear || !value" symbol="fth-calendar" class="ui-timepicker-icon" :size="17" />
    <button v-if="clear && value" type="button" class="ui-timepicker-input-button" @click="clearInput"><ui-icon symbol="fth-x" :size="15" /></button>

    <ui-dropdown ref="overlay" class="ui-timepicker-overlay" @opened="overlayOpened">
      <ui-calendar :today="value" @change="onSelect" :options="pickerOptions" />
    </ui-dropdown>
  </div>
</template>


<script>
  import { generateId } from '../utils/numbers';
  import { formatDate, toIsoDate } from '../utils/dates';

  const TIME_FORMAT = 'HH:mm';

  export default {
    name: 'uiTimepicker',

    props: {
      value: {
        type: String,
        default: null
      },
      placeholder: {
        type: String,
        default: '@ui.time.select'
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
      amPm: {
        type: Boolean,
        default: false
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
      this.id = 'timepicker-' + generateId();
      this.updateOptions();
      this.updateOutput();
    },


    methods: {

      updateOutput()
      {
        this.output = formatDate(this.value, TIME_FORMAT);
      },


      updateOptions()
      {
        this.pickerOptions = {
          enableTime: true,
          noCalendar: true,
          maxDate: null,
          minDate: null,
          time_24hr: !this.amPm
        };
      },


      onSelect(date)
      {
        let dateStr = toIsoDate(date);
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
  .ui-timepicker
  {
    position: relative;
    max-width: 260px;
  }

  input[type="text"].ui-timepicker-input
  {
    padding-right: 36px;
  }

  .ui-timepicker-icon
  {
    position: absolute;
    right: 13px;
    top: 50%;
    margin-top: -8px;
  }

  .ui-timepicker-overlay .ui-dropdown
  {
    padding: 0;
  }

  .ui-timepicker-input-button
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