<template>
  <ui-property v-if="!isHidden" 
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
    <component :is="config.component" v-bind="config.componentOptions" :model-value="model" :entity="modelValue" :meta="meta" @input="onChange" :disabled="isDisabled" />
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
      modelValue: {
        type: Object,
        required: true
      },
      disabled: {
        type: Boolean,
        default: false
      },
    },

    watch: {
      modelValue: {
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
        return this.loaded && typeof this.config.options.condition === 'function' && !this.config.options.condition(this.modelValue, this);
      },
      isRequired()
      {
        return typeof this.config.isRequired === 'function' ? this.config.isRequired(this.modelValue) : this.config.isRequired;
      },
      isDisabled()
      {
        return this.manualDisabled || this.disabled || (typeof this.config.options.disabled === 'boolean' && this.config.options.disabled) || (typeof this.config.options.disabled === 'function' && this.config.options.disabled(this.modelValue, this.model));
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
        return !this.editor.blueprint.unlocked(this.modelValue, this.config);
      },
      canUnlock()
      {
        return this.editor.blueprint.canUnlock(this.modelValue, this.config);
      }
    },

    methods: {

      rebuildModel()
      {
        this.selector = selectorToArray(this.config.path);
        let currentValue = this.modelValue;
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
            if (key in currentValue)
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
          value(this.modelValue);
        }
        else
        {
          setObjectValue(this.modelValue, this.selector, value);
        }
        this.$emit('input', this.modelValue);

        if (typeof this.config.options.onChange === 'function')
        {
          this.config.options.onChange(value, {
            oldValue,
            model: this.modelValue,
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
        let originalValue = await this.editor.blueprint.lock(this.modelValue, this.config);
        this.onChange(originalValue);
      }
    }
  }
</script>