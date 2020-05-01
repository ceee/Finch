<template>
  <ui-button type="action" :label="config.options.name"></ui-button>
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