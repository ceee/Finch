<template>
  <div class="app">
    <input type="text" v-model="input" />
    <br />
    output: {{output}}
  </div>
</template>


<script>
  import { selectorToArray } from './utils';

  export default {
    data: () => ({
      input: null
    }),

    computed: {
      output()
      {
        return selectorToArray(this.input);
      }
    }
  }

</script>