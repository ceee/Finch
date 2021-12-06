<template>
  <div class="app">
    <input type="text" v-model="input" />
    <br />
    output: {{output}}
    <br />
    date: <span v-date="'2020-12-11T00:28:00'"></span>
    <br />
    name: <span v-localize="'@name'"></span>
  </div>
</template>


<script lang="ts">
  import { selectorToArray } from './utils';
  import { localize } from './services/localization';

  export default {
    data: () => ({
      input: null,
      name: localize('@name')
    }),

    computed: {
      output()
      {
        return selectorToArray(this.input);
      }
    }
  }

</script>