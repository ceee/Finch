<template>
  <ui-alias :value="value" :name="entity.name" @input="$emit('input', $event)" :max-length="maxLength" :disabled="disabled" />
</template>


<script>
  export default {
    props: {
      value: {
        type: String,
        default: null
      },
      entity: {
        type: Object,
        required: true
      },
      config: Object,
      disabled: {
        type: Boolean,
        default: false
      }
    },

    computed: {
      maxLength()
      {
        return this.config.maxLength > 0 ? this.config.maxLength : null;
      }
    }
  }
</script>