<template>
  <ui-state-button :items="config.items" :value="value" @input="$emit('input', $event)" />
</template>


<script>
  export default {
    props: {
      value: {
        type: [ String, Number ]
      },
      config: Object
    },

    data: () => ({
      items: []
    })
  }
</script>