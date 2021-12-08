<template>
  <div class="testcmp">
    testcmp
  </div>
</template>



// @ts-ignore
<script lang="ts">

  import { defineComponent } from 'vue';

  interface Props
  {
    maxLength: number;
    placeholder: string | Function;
  }

  //function TestCmpOptions(maxLength?: number, placeholder?: string)
  //{
  //  this.maxLenght =
  //}

  export default defineComponent({
    name: 'editorTestCmp',

    props: {
      options: {
        type: Object as Props,
        default: {
          maxLength: null,
          placeholder: null
        } as Props
      }
    },

    data: () => ({

    }),

    mounted()
    {

    },

    methods: {

    }

  });
</script>

<style lang="scss">
</style>