<template>
  <input :value="value" @input="onChange($event.target.value)" type="text" class="ui-input" v-localize:placeholder="placeholder" :maxlength="maxLength" :disabled="disabled" />
</template>


<script>
  export default {
    props: {
      value: Number,
      maxLength: Number,
      placeholder: String,
      disabled: Boolean
    },

    methods: {
      onChange(value)
      {
        var parsedValue = parseFloat(value);

        if (isNaN(parsedValue))
        {
          return;
        }

        this.$emit('input', parsedValue);
      }
    }
  }
</script>