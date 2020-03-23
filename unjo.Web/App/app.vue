<template>
  <div class="app">
    count: {{count}} <br>
    <button @click="add()">add</button>
  </div>
</template>


<script>
  import Vue from 'vue'

  export default {
    name: 'app',

    data: () => ({
      count: 0
    }),

    mounted ()
    {
      
    },

    methods: {

      add()
      {
        this.count += 1;
      }
    }
  }

</script>