-<template>
  <ui-property v-if="loaded && !isHidden" 
               :field="config.path" 
               :label="label" 
               :hide-label="config.options.hideLabel"
               :description="description" 
               :required="isRequired" 
               :disabled="isDisabled"
               :vertical="config.options.vertical"
               :class="{'is-disabled': isDisabled }"
               :locked="isLocked"
               :can-unlock="canUnlock || false"
               @unlock="unlock"
               @lock="lock">
    <component :is="config.component" v-bind="config.componentOptions" :value="model" :entity="value" :meta="meta" @input="onChange" :disabled="isDisabled" />
    <p v-if="config.options.helpText" class="ui-property-help" v-localize="config.options.helpText"></p>
  </ui-property>
</template>


<script>
  import { selectorToArray } from '../utils/arrays';
  import { setObjectValue } from '../utils/objects';
  import { localize } from '../services/localization';
  //import Overlay from 'zero/helpers/overlay.js';

  export default {
    name: 'uiEditorComponent',

    inject: [ 'meta' ],

    props: {
      config: {
        type: Object,
        required: true
      },
      editor: {
        type: Object,
        required: true
      },
      value: {
        type: Object,
        required: true
      },
      disabled: {
        type: Boolean,
        default: false
      },
    },

    watch: {
      value: {
        deep: true,
        handler: function ()
        {
          this.rebuildModel();
        }
      }
    },

    data: () => ({
      model: null,
      loaded: false,
      manualDisabled: false,
      selector: null
    }),

    mounted()
    {
      this.rebuildModel();
      this.loaded = true;
    },

    computed: {
      isHidden()
      {
        return this.loaded && typeof this.config.options.condition === 'function' && !this.config.options.condition(this.value, this);
      },
      isRequired()
      {
        return typeof this.config.isRequired === 'function' ? this.config.isRequired(this.value) : this.config.isRequired;
      },
      isDisabled()
      {
        return this.manualDisabled || this.disabled || (typeof this.config.options.disabled === 'boolean' && this.config.options.disabled) || (typeof this.config.options.disabled === 'function' && this.config.options.disabled(this.value, this.model));
      },
      label()
      {
        return this.config.options.label || this.editor.templateLabel(this.config.path);
      },
      description()
      {
        return localize(this.config.options.description || this.editor.templateDescription(this.config.path), { hideEmpty: true });
      },
      isLocked()
      {
        return this.editor.blueprint && !this.editor.blueprint.unlocked(this.value, this.config);
      },
      canUnlock()
      {
        return this.editor.blueprint && this.editor.blueprint.canUnlock(this.value, this.config);
      }
    },

    methods: {

      rebuildModel()
      {
        this.selector = selectorToArray(this.config.path);
        let currentValue = this.value;
        let found = false;

        if (!this.selector || !this.selector.length || !currentValue)
        {
          found = true;
          this.model = null;
        }
        else
        {
          for (var key of this.selector)
          {
            if (currentValue && key in currentValue)
            {
              found = true;
              currentValue = currentValue[key];
            }
            else
            {
              break;
            }
          }

          this.model = found ? currentValue : null;
        }
      },


      onChange(value)
      {
        let oldValue = JSON.parse(JSON.stringify(this.model));

        if (typeof value === 'function')
        {
          value(this.value);
        }
        else
        {
          setObjectValue(this.value, this.selector, value);
        }
        this.$emit('input', this.value);

        if (typeof this.config.options.onChange === 'function')
        {
          this.config.options.onChange(value, {
            oldValue,
            model: this.value,
            component: this
          });
        }
      },


      setDisabled(disabled)
      {
        this.manualDisabled = disabled;
      },


      async unlock()
      {
        //Overlay.confirm({
        //  title: 'Unlock property',
        //  text: 'Unlock this property to override the value passed by the blueprint',
        //  confirmLabel: 'Confirm',
        //  closeLabel: 'Cancel'
        //}).then(
        //  async () => await this.editor.blueprint.unlock(this.value, this.config),
        //  () => {}
        //);
      },

      async lock()
      {
        let originalValue = await this.editor.blueprint.lock(this.value, this.config);
        this.onChange(originalValue);
      }
    }
  }
</script>