-<template>
   <ui-property v-if="loaded && !field.hidden(value)"
                :field="field.path"
                :hide-label="field.hideLabel"
                :label="field.label"
                :description="description"
                :required="!field.optional(value)"
                :disabled="isDisabled"
                :vertical="!field.horizontal"
                :class="{'is-disabled': isDisabled }"
                :locked="isLocked"
                :can-unlock="canUnlock || false"
                @unlock="unlock"
                @lock="lock">
     <component :is="field.component" v-bind="field.options"
                :value="model"
                :config="{ model: value, field, meta, disabled: isDisabled, system }"
                :meta="meta"
                @input="onChange" />
     <template v-slot:after>
       <p v-if="field.helpText" class="ui-property-help" v-localize="field.helpText"></p>
     </template>
   </ui-property>
</template>


<script>
  import { selectorToArray, setObjectValue } from '../utils';
  import { localize } from '../services/localization';
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
      }
    },

    watch: {
      value: {
        deep: true,
        handler: function (val)
        {
          this.model = this.field.getValue(val);
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
      this.model = this.field.getValue(this.value);
      this.loaded = true;
    },

    computed: {
      description()
      {
        return localize(this.field.description, { hideEmpty: true });
      },
      isDisabled()
      {
        return this.manualDisabled || this.disabled || this.field.readonly(this.model, this.value);
      },
      isLocked()
      {
        return false; //this.editor.blueprint && !this.editor.blueprint.unlocked(this.value, this.field);
      },
      canUnlock()
      {
        return false; // this.editor.blueprint && this.editor.blueprint.canUnlock(this.value, this.field);
      }
    },

    methods: {

      onChange(value)
      {
        let oldValue = JSON.parse(JSON.stringify(this.model));

        if (typeof value === 'function')
        {
          value(this.value);
        }
        else
        {
          setObjectValue(this.value, this.field.selector, value);
        }

        this.$emit('input', this.value);

        this.field.onChange({
          value: value,
          model: this.value,
          oldValue,
          component: this
        })
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