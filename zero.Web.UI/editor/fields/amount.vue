<template>
  <div class="ui-box">
    <input :value="value" @input="$emit('input', $event.target.value)" type="text" class="ui-input" v-localize:placeholder="config.placeholder" :maxlength="maxLength" :disabled="disabled" />
  </div>
</template>


<script>
  export default {
    emits: ['input'],

    props: {
      value: {
        type: Number,
        default: null
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