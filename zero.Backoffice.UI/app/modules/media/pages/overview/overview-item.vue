<template>
  <div class="media-item" :class="{ 'is-selected': selected}">
    <component :is="component" :to="link" type="button" class="media-item-preview" :class="{'media-pattern': !value.isFolder, 'is-covered': covered }" @click="$emit('click', value)">
      <span class="media-item-check"><ui-icon symbol="fth-check" :size="14" /></span>
      <img class="media-item-image" v-if="value.preview" :src="value.preview" />
      <span class="media-item-icon" v-if="!value.preview"><ui-icon :symbol="(value.isFolder ? 'fth-folder' : 'fth-file')" :size="26" :stroke-width="2" /></span>
    </component>
    <p v-if="value.isFolder" class="media-item-text" :title="value.name">
      <!--<input type="text" v-model="value.name" />-->
      {{value.name}} <!--<ui-icon symbol="fth-cloud" v-if="value.isShared" :size="15" class="media-item-shared" />-->
      <span class="-minor"> ({{value.children}})<!--<span v-localize="{ key: value.children === 1 ? '@media.child_count_1' : '@media.child_count_x', tokens: { count: value.children }}"></span>--></span>
    </p>
    <p v-else class="media-item-text -minor">
      <span v-filesize:0="value.size"></span> | <span :title="value.name">{{value.name}}</span>
    </p>
  </div>
</template>

<script>
  export default {
    name: 'mediaOverviewItem',

    props: {
      value: {
        type: Object,
        default: () => { }
      },
      selected: {
        type: Boolean,
        default: false
      },
      link: {
        type: [Object, String],
        default: null
      }
    },

    computed: {
      component()
      {
        return this.link ? 'ui-link' : 'button';
      },
      covered()
      {
        return false; //this.value.aspectRatio < 1.2 && this.value.aspectRatio > 0.8;
      }
    }
  };
</script>

<style lang="scss">
  .media-item
  {
    width: 100%;
    min-height: 140px;
    display: grid;
    grid-template-rows: auto 1fr;
    gap: 10px;
    align-items: flex-start;
    line-height: 1.4;
    color: var(--color-text);
    font-size: var(--font-size);
    border-radius: var(--radius);
  }

  .ui-datagrid-outer.is-selecting .media-item
  {
    opacity: .55;
  }

  .ui-datagrid-outer.is-selecting .media-item.is-selected
  {
    opacity: 1;
  }

  .media-item-preview
  {
    display: flex;
    align-items: center;
    justify-content: center;
    flex-direction: column;
    height: 130px;
    width: 100%;
    background: var(--color-box);
    border-radius: var(--radius);
    overflow: visible !important;
    position: relative;
    text-align: center;
    box-shadow: var(--shadow-short);
    border: 2px solid transparent;
    color: var(--color-text);
  }

  .media-item.is-selected .media-item-preview
  { 
    border-color: var(--color-accent); 
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

  .media-item-icon
  {
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
    width: 22px;
    height: 22px;
    border-radius: 20px;
    position: absolute;
    z-index: 2;
    left: -10px;
    top: -10px;
    background: var(--color-accent);
    color: var(--color-accent-fg);
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

    &.-minor, .-minor
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

  .media-item input[type="text"]
  {
    border: none;
    height: auto;
    padding: 0;
    background: none;
    width: auto;
  }
</style>
