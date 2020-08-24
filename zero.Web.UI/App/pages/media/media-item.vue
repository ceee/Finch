<template>
  <router-link :to="link" class="media-item">
    <img v-if="value.type === 'image'" :src="value.source" />
    <span class="media-item-content is-file" v-if="value.type !== 'image'">
      <i :class="icons[value.type]" :data-extension="value.source.split('.').pop()"></i>
      <span>{{value.name}}</span>
    </span>
  </router-link>
</template>

<script>
  export default {
    props: {
      value: {
        type: Object,
        default: () => { }
      }
    },

    data: () => ({
      icons: {
        image: 'fth-image',
        video: 'fth-video',
        file: 'fth-file'
      }
    }),

    computed: {
      link()
      {
        return {
          name: 'mediaitem',
          params: { id: this.value.id }
        };
      }
    }
  };
</script>

<style lang="scss">
</style>
