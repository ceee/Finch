<template>
  <input :value="modelValue" @input="$emit('input', $event.target.value)" type="text" class="ui-input" v-placeholder="{ placeholder, model: entity }" :maxlength="maxLength" :disabled="disabled" />
</template>


<script>
  declare module 'zero/schemas'
  {
    export interface ZeroEditorField
    {
      /**
       * Set this field as required
       * @param {function|boolean} [condition] - Optionally only require this field when a condition is fulfilled or reset the required state with true/false
       */
      text(options?: TextFieldOptions): ZeroEditorField;
    }
  }

  export default {
    props: {
      value: {
        type: String,
        default: null
      },
      maxLength: {
        type: Number,
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      },
      placeholder: {
        type: [String, Function],
        default: null
      },
      entity: Object
    }
  }
</script>