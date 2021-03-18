<template>
  <router-link :to="link" class="media-item">
    <div class="media-item-preview" :class="{'media-pattern': value.image }">
      <span class="media-item-check"><ui-icon symbol="fth-check" :size="14" /></span>
      <img class="media-item-image" v-if="value.image" :src="value.image" />
      <span class="media-item-icon" v-if="!value.image"><ui-icon :symbol="shared ? 'fth-globe' : (value.isFolder ? 'fth-folder' : 'fth-file')" :size="36" :stroke-width="1.5" /></span>
    </div>
    <p class="media-item-text" v-if="!shared">
      <span :title="value.name">{{value.name}}</span>
      <span class="-minor" v-if="!value.isFolder"><br><span v-filesize="value.size"></span></span>
      <span class="-minor" v-if="value.isFolder"><br><span v-localize="{ key: value.children === 1 ? '@media.child_count_1' : '@media.child_count_x', tokens: { count: value.children }}"></span></span>
    </p>
    <p class="media-item-text" v-if="shared">
      <span :title="value.name">Shared</span>
      <span class="-minor"><br>Media for all apps</span>
    </p>
  </router-link>
</template>

<script>
  export default {
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
        if (this.shared)
        {
          return {
            name: 'media',
            query: { scope: 'shared' }
          };
        }

        return {
          name: this.value.isFolder ? 'media' : 'media-edit',
          params: {
            id: this.value.id
          },
          query: this.$route.query
        };
      },
      shared()
      {
        return this.value.id === 'shared';
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
    transform: scale(0.95);
    border: 3px solid var(--color-primary);
  }

  .media-item-image
  {
    width: 100%;
    height: 100%;
    object-fit: cover;
    position: relative;
    border-radius: var(--radius);
    z-index: 1;
  }

  .media-item-check
  {
    display: none;
    justify-content: center;
    align-items: center;
    width: 24px;
    height: 24px;
    border-radius: 20px;
    position: absolute;
    z-index: 2;
    left: -12px;
    top: -12px;
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
</style>
