<template>
  <div class="editor-nested">
    <div v-for="item in items" class="editor-nested-item">
      <div class="editor-nested-item-header">
        <router-link :to="{ name: 'space', params: { alias: item.alias } }" class="editor-nested-item-header-link">
          <i class="editor-nested-item-header-icon fth-truck" :class="item.icon"></i>
          <span class="editor-nested-item-header-text">
            {{item.name | localize}}
            <span v-if="item.description" v-localize="item.description"></span>
          </span>
        </router-link>
        <ui-dot-button class="editor-nested-item-header-actions" />
      </div>
    </div>
    <ui-button type="light" :label="config.addLabel || '@ui.add'" @click="addItem" />
  </div>
</template>

<script>
  export default {
    props: {
      value: {
        type: [Array, Object]
      },
      config: Object
    },


    data: () => ({
      items: []
    }),


    methods: {

      addItem()
      {
        this.items.push({
          name: 'Österreichische Post',
          description: 'All countries'
        });
      }

    }
  }
</script>

<style lang="scss">
  .editor-nested-item:first-child .editor-nested-item-header
  {
    padding-top: 0;
  }

  .editor-nested-item-header
  {
    display: grid;
    grid-template-columns: 1fr auto;
    align-items: center;
    font-size: var(--font-size);
    padding: 10px 0;
    color: var(--color-fg);
    position: relative;
    transition: color 0.2s ease;
    line-height: 1.5;

    &:hover > .editor-nested-item-header-actions
    {
      transition-delay: 0.2s;
      opacity: 1;
    }

    & + .editor-nested-item-header
    {
      margin-top: 15px;
    }

    &.has-line
    {
      border-bottom: 1px solid var(--color-bg);
      padding-bottom: 25px;
    }
  }

  .editor-nested-item-header-link
  {
    display: grid;
    grid-template-columns: 30px 1fr auto;
    grid-gap: 6px;
    height: 100%;
    align-items: center;
    position: relative;
    color: var(--color-fg);

    &.is-active
    {
      font-weight: bold;
      color: var(--color-secondary);

      .editor-nested-item-header-text span
      {
        font-weight: 400;
      }
    }
  }

  .editor-nested-item-header-text
  {
    display: flex;
    flex-direction: column;

    span
    {
      color: var(--color-fg-light);
      margin-top: 3px;
    }
  }

  .editor-nested-item-header-toggle
  {
    position: absolute;
    color: var(--color-fg-mid);
    height: 100%;
    top: 0;
    left: 0;
    width: 30px;
    text-align: right;
    padding-right: 5px;
    outline: none !important;
    transition: color 0.2s ease;

    &:hover
    {
      color: var(--color-fg);
    }
  }

  .editor-nested-item-header-icon
  {
    font-size: 18px;
    line-height: 1;
    font-weight: 400;
    position: relative;
    top: -2px;
    color: var(--color-fg);
    transition: color 0.2s ease;
  }

  .editor-nested-item-header-actions
  {
    transition: opacity 0.2s ease 0;
    opacity: 0;
    color: var(--color-fg-mid);
  }
</style>