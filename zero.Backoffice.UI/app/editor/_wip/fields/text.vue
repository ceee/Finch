<template>
  <input :value="value" @input="$emit('input', $event.target.value)" type="text" class="ui-input" v-placeholder="{ placeholder: options.placeholder, model: entity }" :maxlength="options.maxLength" :disabled="disabled" />
</template>


<script lang="ts">
  import { defineComponent, PropType } from 'vue';
  import { TextFieldOptions } from 'zero/editor';

  export default defineComponent({
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
      entity: Object,
      options: Object as PropType<TextFieldOptions>
    }
  });
</script>