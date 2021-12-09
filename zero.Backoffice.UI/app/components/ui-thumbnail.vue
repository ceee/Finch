<template>
  <img :src="src" />
</template>


<script lang="ts">
  import { defineComponent } from 'vue';

  export default defineComponent({
    name: 'uiThumbnail',

    props: {
      media: {
        type: String,
        required: true
      },
      size: {
        type: String,
        default: 'thumbnail'
      }
    },

    computed: {
      src()
      {
        return 'zero/api/media/getSource/?id=' + this.media + '&size=' + this.size;
      }
    }
  });
</script>