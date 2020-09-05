<template>
  <router-link :to="link" class="media-item">
    <div class="media-item-preview" :class="{'media-bg-pattern': value.image }">
      <img class="media-item-image" v-if="value.image" :src="value.image" />
      <span class="media-item-icon" v-if="!value.image"><i :class="value.isFolder ? 'fth-folder' : 'fth-file'"></i></span>
    </div>
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
    border-radius: var(--radius);
    //background: var(--color-bg-bright);
  }

  .media-item-preview
  {
    display: flex;
    align-items: center;
    justify-content: center;
    flex-direction: column;
    height: $media-item-size;
    width: $media-item-size;
    background: var(--color-bg-bright);
    border-radius: var(--radius);
    overflow: hidden;
    position: relative;
    text-align: center;
    font-size: 20px;
    box-shadow: var(--color-shadow-short);
  }

  .media-item-image
  {
    width: 100%;
    height: 100%;
    object-fit: contain;
    position: relative;
    border-radius: var(--radius);
    z-index: 1;
  }

  .media-item-text
  {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    margin: 0;
    padding-right: 16px;

    .-minor
    {
      color: var(--color-fg-dim-two);
    }
  }

  .media-bg-pattern
  {
    position: relative;
    z-index: 0;
    overflow: hidden;
    background: transparent;

    $media-pattern-size: 10px;
    $media-pattern-color-a: #ddd;
    $media-pattern-color-b: #aaa;

    &:before
    {
      content: '';
      position: absolute;
      width: 100%;
      height: 100%;
      left: 0;
      top: 0;
      background-position: 0px 0px, #{$media-pattern-size / 2} #{$media-pattern-size / 2};
      background-size: #{$media-pattern-size} #{$media-pattern-size};
      background-image: linear-gradient(45deg, #{$media-pattern-color-b} 25%, transparent 25%, transparent 75%, #{$media-pattern-color-b} 75%, #{$media-pattern-color-b} 100%),
        linear-gradient(45deg, #{$media-pattern-color-b} 25%, #{$media-pattern-color-a} 25%, #{$media-pattern-color-a} 75%, #{$media-pattern-color-b} 75%, #{$media-pattern-color-b} 100%);
    }
  }
</style>
