<template>
  <input :value="value" @input="$emit('input', $event.target.value)" type="text" class="ui-input" v-localize:placeholder="" :maxlength="maxLength" />
</template>


<script>
  export default {
    props: {
      value: {
        type: String,
        default: null
      },
      config: Object
    },

    computed: {
      maxLength()
      {
        return this.config.options.maxLength > 0 ? this.config.options.maxLength : null;
      }
    }
  }
</script>