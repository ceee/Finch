<template>
  <ui-select :value="value" :items="targets" @input="$emit('input', $event)" />
</template>


<script>
  export default {
    name: 'uiLinkTarget',

    props: {
      value: {
        type: String,
        default: 'default'
      }
    },

    data: () => ({
      targets: [
        { label: '@links.target.default', value: 'default' },
        { label: '@links.target.self', value: 'self' },
        { label: '@links.target.blank', value: 'blank' }
      ]
    })
  }
</script>