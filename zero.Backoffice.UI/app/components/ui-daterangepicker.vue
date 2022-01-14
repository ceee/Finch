<template>
  <div class="ui-daterangepicker" :class="{'is-disabled': disabled }">
    <button v-if="!inline" type="button" class="ui-link" @click="schedule" v-localize="scheduleLocalize" :disabled="disabled"></button>
    <div v-if="inline && value" class="ui-daterangepicker-inline">
      <div class="ui-daterangepicker-group">
        <ui-property :vertical="true">
          <ui-datepicker v-model="value.from" :time="time" />
        </ui-property>
      </div>
      <div class="ui-daterangepicker-group" v-if="rangeEnd">
        <ui-property :vertical="true">
          <ui-datepicker v-model="value.to" :time="time" />
        </ui-property>
      </div>
    </div>
  </div>
</template>


<script lang="ts">
  import { generateId } from '../utils/numbers';
  import { formatDate, toIsoDate } from '../utils/dates';
  import * as overlays from '../services/overlay';

  const DATETIME_FORMAT = 'DD.MM.YY HH:mm';
  const DATE_FORMAT = 'DD.MM.YY';


  export default {
    name: 'uiDaterangepicker',

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
      maxDate: {
        type: [String, Date],
        default: null
      },
      minDate: {
        type: [String, Date],
        default: null
      },
      fromLabel: {
        type: String,
        default: '@ui.date.range_from'
      },
      toLabel: {
        type: String,
        default: '@ui.date.range_to'
      },
      amPm: {
        type: Boolean,
        default: false
      },
      inline: {
        type: Boolean,
        default: false
      },
      rangeEnd: {
        type: Boolean,
        default: true
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
          key: !this.value || (!this.value.from && !this.value.to) ? '@ui.date.set' :
            (this.value.from && !this.value.to ? '@ui.date.x' :
              (!this.value.from && this.value.to ? '@ui.date.y' : '@ui.date.xtoy')),
          tokens: {
            x: this.value ? formatDate(this.value.from, this.format || (this.time ? DATETIME_FORMAT : DATE_FORMAT)) : null,
            y: this.value ? formatDate(this.value.to, this.format || (this.time ? DATETIME_FORMAT : DATE_FORMAT)) : null
          }
        };
      }
    },

    created()
    {
      this.id = 'daterangepicker-' + generateId();
    },

    methods: {

      async schedule()
      {
        const result = await overlays.open({
          component: DaterangepickerOverlay,
          model: {
            from: this.value.from,
            to: this.value.to,
            options: {
              format: this.format,
              time: this.time,
              max: this.maxDate,
              min: this.minDate,
              fromText: this.fromLabel,
              toText: this.toLabel,
              amPm: this.amPm,
              rangeEnd: this.rangeEnd
            },
          },
        });

        if (result.eventType == 'confirm')
        {
          this.$emit('change', result.value);
          this.$emit('input', result.value);
        }
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

  .ui-daterangepicker-inline
  {
    display: flex;
    gap: var(--padding-s);
  }
</style>