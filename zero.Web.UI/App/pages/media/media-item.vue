<template>
  <router-link :to="link" class="media-item">
    <img class="media-item-image" v-if="value.image" :src="value.image" />
    <span class="media-item-icon" v-if="!value.image"><i :class="value.isFolder ? 'fth-folder' : 'fth-file'"></i></span>
    <p class="media-item-text">
      <span :title="value.name">{{value.name}}</span>
      <span class="-minor" v-if="value.size"><br><span v-filesize="value.size"></span></span>
    </p>
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
      
    }),

    computed: {
      link()
      {
        return {
          name: this.value.isFolder ? 'mediafolder' : 'mediaitem',
          params: { id: this.value.id }
        };
      }
    }
  };
</script>

<style lang="scss">
  $media-item-size: 80px;

  .media-item
  {
    width: 100%;
    min-height: $media-item-size;
    display: grid;
    grid-template-columns: auto 1fr;
    gap: 16px;
    align-items: center;
    line-height: 1.4;
    color: var(--color-fg);
    font-size: var(--font-size);
  }

  .media-item-image,
  .media-item-icon
  {
    display: flex;
    align-items: center;
    justify-content: center;
    flex-direction: column;
    height: $media-item-size;
    width: $media-item-size;
    object-fit: cover;
    background: var(--color-bg-bright);
    border-radius: var(--radius);
    position: relative;
    text-align: center;
    font-size: 20px;
  }

  .media-item-text
  {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    margin: 0;

    .-minor
    {
      color: var(--color-fg-dim-two);
    }
  }
</style>
