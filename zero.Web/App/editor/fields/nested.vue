<template>
  <div>nested</div>
  <!--<input :value="value" @input="$emit('input', $event.target.value)" type="text" class="ui-input" v-localize:placeholder="" />-->
</template>


<script>
  export default {
    props: {
      value: {
        type: [Object, Array]
      }
    }
  }
</script>