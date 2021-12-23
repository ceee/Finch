<template>
  <ui-link :to="link" class="media-item">
    <div class="media-item-preview" :class="{'media-pattern': value.image, 'is-covered': covered }">
      <span class="media-item-check"><ui-icon symbol="fth-check" :size="14" /></span>
      <img class="media-item-image" v-if="value.image" :src="value.image" />
      <span class="media-item-icon" v-if="!value.image"><ui-icon :symbol="(value.isFolder ? 'fth-folder' : 'fth-file')" :size="36" :stroke-width="1.5" /></span>
    </div>
    <p class="media-item-text">
      <span :title="value.name">{{value.name}} <ui-icon symbol="fth-cloud" v-if="value.isShared" :size="15" class="media-item-shared" /></span>
      <span class="-minor" v-if="!value.isFolder"><br><span v-filesize="value.size"></span></span>
      <span class="-minor" v-if="value.isFolder"><br><span v-localize="{ key: value.children === 1 ? '@media.child_count_1' : '@media.child_count_x', tokens: { count: value.children }}"></span></span>
    </p>
  </ui-link>
</template>

<script>
  export default {
    name: 'mediaItem',

    props: {
      value: {
        type: Object,
        default: () => { }
      },
      selected: {
        type: Boolean,
        default: false
      }
    },

    computed: {
      link()
      {
        return {
          name: this.value.isFolder ? 'media' : 'media-edit',
          params: {
            id: this.value.id
          },
          query: this.$route.query
        };
      },
      covered()
      {
        return this.value.aspectRatio < 1.2 && this.value.aspectRatio > 0.8;
      }
    }
  };
</script>

<style lang="scss">
  .media-item
  {
    width: 100%;
    min-height: 200px;
    display: grid;
    grid-template-rows: auto 1fr;
    gap: 10px;
    align-items: center;
    line-height: 1.4;
    color: var(--color-text);
    font-size: var(--font-size);
    border-radius: var(--radius);
  }

  .media-items.is-selecting .media-item
  {
    opacity: .5; 
  }

  .media-items.is-selecting .media-item.is-selected
  {
    opacity: 1;
  }

  .media-item-preview
  {
    display: flex;
    align-items: center;
    justify-content: center;
    flex-direction: column;
    height: 200px;
    width: 100%;
    background: var(--color-box);
    border-radius: var(--radius);
    overflow: visible !important;
    position: relative;
    text-align: center;
    box-shadow: var(--shadow-short);
  }

  .media-item.is-selected .media-item-preview
  { 
    //border: 3px solid var(--color-primary); 
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

  .media-item-preview.is-covered .media-item-image
  {
    object-fit: cover;
  }

  .media-item-check
  {
    display: none;
    justify-content: center;
    align-items: center;
    width: 30px;
    height: 30px;
    border-radius: 20px;
    border: 5px solid var(--color-bg);
    position: absolute;
    z-index: 2;
    left: -13px;
    top: -13px;
    background: var(--color-primary);
    color: var(--color-primary-text);
    box-shadow: 1px 1px 0 1px var(--color-shadow);
    font-size: 11px;
    .is-selected &
    {
      display: inline-flex;
    }
  }

  .media-item-text
  {
    white-space: nowrap; 
    overflow: hidden;
    text-overflow: ellipsis;
    margin: 0;
    padding-right: 16px;
    font-weight: bold;

    .-minor
    {
      font-weight: 400;
      color: var(--color-text-dim);
    }
  }

  .media-item-shared
  {
    margin-left: .4em;
    color: var(--color-synchronized);
    position: relative;
    top: 2px;
  }
</style>
