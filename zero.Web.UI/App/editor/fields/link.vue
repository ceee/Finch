<template>
  <input :value="url" @input="onChange($event.target.value)" type="text" class="ui-input" :disabled="disabled" />
</template>


<script>
  export default {
    props: {
      value: {
        type: Object,
        default: {
          providerAlias: null,
          target: 'default',
          urlSuffix: null,
          label: null,
          title: null,
          values: {
            url: null
          }
        }
      },
      disabled: {
        type: Boolean,
        default: false
      }
    },

    data: () => ({
      url: null
    }),

    methods: {
      onChange(value)
      {
        this.$emit('input', {
          values: {
            url: value
          }
        });
      }
    }
  }
</script>