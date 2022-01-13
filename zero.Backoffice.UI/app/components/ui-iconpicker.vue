<template>
  <div v-if="render" class="ui-iconpicker" :class="{'is-disabled': disabled }">
    <input ref="input" type="hidden" :value="value" />
    <ui-select-button :icon="previewIcon" label="@ui.icon" :description="buttonDescription" @click="pick" :disabled="disabled" />
  </div>
</template>


<script>
  import { open as openOverlay } from '../services/overlay';
  import { extendObject } from '../utils';
  import { useUiStore } from '../ui/store';
  import * as notifications from '../services/notification';
  import { localize } from '../services/localization';

  export default {
    name: 'uiIconpicker',

    props: {
      value: {
        type: String,
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      },
      colors: {
        type: Boolean,
        default: false
      },
      render: {
        type: Boolean,
        default: true
      },
      set: {
        type: String,
        default: 'feather'
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
      iconSet: null,
      store: null
    }),

    watch: {
      set()
      {
        this.loadSet();
      }
    },

    created()
    {
      this.store = useUiStore();
      this.loadSet();
    },

    computed: {
      buttonDescription()
      {
        return this.value ? this.value.split(' ')[0] : '@ui.icon_select';
      },
      previewIcon()
      {
        return this.value || 'fth-plus';
      }
    },

    methods: {

      onChange(value)
      {
        this.$emit('change', value);
        this.$emit('input', value);
        this.$emit('update:value', value);
        // TODO this does not trigger the forms dirty flag
      },

      loadSet()
      {
        const alias = this.set || 'feather';
        this.iconSet = this.store.iconSets.find(x => x.alias === alias);
      },

      async pick()
      {
        if (this.disabled)
        {
          return;
        }

        if (!this.iconSet)
        {
          notifications.error("@iconpicker.notfound.title", localize("@iconpicker.notfound.text", { tokens: { name: this.set || '' } }));
          return;
        }

        const result = await openOverlay(extendObject({
          model: {
            set: this.iconSet,
            value: this.value,
            colors: this.colors
          },
          component: () => import('./ui-iconpicker-overlay.vue'),
          display: 'editor',
          width: 660
        }, typeof this.options === 'object' ? this.options : {}));

        if (result.eventType === 'confirm')
        {
          this.onChange(result.value);
        }
      }
    }
  }
</script>