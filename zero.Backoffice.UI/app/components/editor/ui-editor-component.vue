-<template>
  <ui-property v-if="loaded && !field.hidden(value)"
               :field="field.path"
               :hide-label="field.hideLabel"
               :label="field.label"
               :description="description"
               :required="!field.optional(value)"
               :disabled="isDisabled"
               :vertical="!field.horizontal"
               >
    <component :is="field.component" v-bind="field.options" 
               :value="model" 
               :config="config"
               @input="onChange" />
    <p v-if="field.helpText" class="ui-property-help" v-localize="field.helpText"></p>
  </ui-property>

  <!--<ui-property v-if="loaded && !isHidden"
               :vertical="field.configuration.vertical"
               :class="{'is-disabled': isDisabled }"
               :locked="isLocked"
               :can-unlock="canUnlock || false"
               @unlock="unlock"
               @lock="lock">
    <component :is="config.component" v-bind="config.componentOptions" :value="model" :entity="value" :meta="meta" @input="onChange" :disabled="isDisabled" />
    <p v-if="field.configuration.helpText" class="ui-property-help" v-localize="field.configuration.helpText"></p>
  </ui-property>-->
</template>


<script>
  import { selectorToArray, setObjectValue } from '../../utils';
  import { localize } from '../../services/localization';
  import { defineComponent } from 'vue';
  //import Overlay from 'zero/helpers/overlay.js';

  export default defineComponent({
    name: 'uiEditorComponent',

    inject: [ 'meta' ],

    props: {
      field: {
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
      system: {
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
      config: {},
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
      description()
      {
        return localize(this.field.description, { hideEmpty: true });
      },
      isDisabled()
      {
        return this.manualDisabled || this.disabled || this.field.readonly(this.model);
      }
      //isLocked()
      //{
      //  return this.editor.blueprint && !this.editor.blueprint.unlocked(this.value, this.field);
      //},
      //canUnlock()
      //{
      //  return this.editor.blueprint && this.editor.blueprint.canUnlock(this.value, this.field);
      //}
    },

    methods: {

      rebuildModel()
      {
        this.config = {
          model: this.value,
          field: this.field,
          meta: this.meta,
          disabled: this.isDisabled,
          system: this.system
        };

        this.selector = selectorToArray(this.field.path);
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

        // TODO
        //if (typeof this.field.configuration.onChange === 'function')
        //{
        //  this.field.configuration.onChange(value, {
        //    oldValue,
        //    model: this.value,
        //    component: this
        //  });
        //}
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
        //  async () => await this.editor.blueprint.unlock(this.value, this.field),
        //  () => {}
        //);
      },

      async lock()
      {
        //let originalValue = await this.editor.blueprint.lock(this.value, this.field);
        //this.onChange(originalValue);
      }
    }
  })
</script>