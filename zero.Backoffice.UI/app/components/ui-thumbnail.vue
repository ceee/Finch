<template>
  <img v-if="media && !hasError" :src="src" @error="onError" />
  <span v-else><slot></slot></span>
</template>


<script lang="ts">
  import { defineComponent } from 'vue';
  import { paths } from '../options';

  export default defineComponent({
    name: 'uiThumbnail',

    props: {
      media: {
        type: String,
        required: true
      },
      size: {
        type: String,
        default: 'thumb'
      }
    },

    data: () => ({
      hasError: false
    }),

    computed: {
      src()
      {
        return this.media ? (paths.api.replace('{app}', 'hofbauer') + '/media/' + this.media + '/' + this.size + '.tmp') : null;
      },

      onError(ev)
      {
        this.hasError = true;
      }
    }
  });
</script>