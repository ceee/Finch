<template>
  <div class="ui-daterangepicker" :class="{'is-disabled': disabled }">
    <button v-if="!inline" type="button" class="ui-link" @click="schedule" v-localize="scheduleLocalize" :disabled="disabled"></button>
    <div v-if="inline" class="ui-daterangepicker-inline">
      <div class="ui-split">
        <div class="ui-daterangepicker-group">
          <ui-property :label="fromText" :vertical="true">
            <ui-datepicker v-model="value.from" :time="time" />
          </ui-property>
        </div>
        <div class="ui-daterangepicker-group">
          <ui-property :label="toText" :vertical="true">
            <ui-datepicker v-model="value.to" :time="time" />
          </ui-property>
        </div>
      </div>
    </div>
  </div>
</template>


<script>
  import Strings from 'zero/services/strings';
  import { extend as _extend } from 'underscore';
  import DaterangepickerOverlay from './overlay';
  import Overlay from 'zero/services/overlay.js';
  import dayjs from 'dayjs';

  const DATETIME_FORMAT = 'DD.MM.YY HH:mm';
  const DATE_FORMAT = 'DD.MM.YY';

  export default {
    name: 'uiDaterangepicker',

    emits: ['change', 'input'],

    components: { DaterangepickerOverlay },

    props: {
      value: {
        type: Object,
        default: {
          from: null,
          to: null
        }
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
      fromText: {
        type: String,
        default: '@page.schedule.publish'
      },
      toText: {
        type: String,
        default: '@page.schedule.unpublish'
      },
      amPm: {
        type: Boolean,
        default: false
      },
      inline: {
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


    computed: {
      scheduleLocalize()
      {
        return {
          key: !this.value.from && !this.value.to ? '@ui.date.set' :
            (this.value.from && !this.value.to ? '@ui.date.x' :
              (!this.value.from && this.value.to ? '@ui.date.y' : '@ui.date.xtoy')),
          tokens: {
            x: Strings.date(this.value.from, this.format || (this.time ? DATETIME_FORMAT : DATE_FORMAT)),
            y: Strings.date(this.value.to, this.format || (this.time ? DATETIME_FORMAT : DATE_FORMAT))
          }
        };
      }
    },


    created()
    {
      this.id = 'daterangepicker-' + Strings.guid();
    },


    methods: {

      schedule()
      {
        Overlay.open({
          component: DaterangepickerOverlay,
          options: {
            format: this.format,
            time: this.time,
            max: this.max,
            min: this.min,
            fromText: this.fromText,
            toText: this.toText,
            amPm: this.amPm
          },
          model: {
            from: this.value.from,
            to: this.value.to
          },
        }).then(res =>
        {
          this.$emit('change', res);
          this.$emit('input', res);
        }, () => { });
      }
    }
  }
</script>

<style lang="scss">
  .ui-daterangepicker.is-primary .ui-link
  {
    color: var(--color-primary);
    font-weight: 700;
    text-decoration-color: var(--color-primary) !important;
  }
</style>