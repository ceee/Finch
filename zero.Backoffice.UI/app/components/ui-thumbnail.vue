<template>
  <img v-if="media && !hasError" :src="src" @error="onError" />
  <span v-else><slot></slot></span>
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

    data: () => ({
      hasError: false
    }),

    computed: {
      src()
      {
        return this.media ? ('/zero/api/backoffice/ui/mediapreview/' + this.media + (this.size ? ('/' + this.size) : '')) : null;
      },

      onError(ev)
      {
        this.hasError = true;
      }
    }
  });
</script>