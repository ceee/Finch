<template>
  <div class="ui-tabs">
    <div role="tablist" class="ui-tabs-list">
      <button type="button" v-for="(tab, index) in tabs" class="ui-tabs-list-item" :key="index" :class="{ 'is-active': tab.active }" :disabled="tab.disabled" 
              :aria-selected="tab.active" role="tab" @click="select(index)">
        {{ tab.label | localize }}
      </button>
    </div>
    <div class="ui-tabs-items">
      <slot />
    </div>
  </div>
</template>


<script>
  export default {
    name: 'uiTabs',

    props: {
      
    },

    data: () => ({
      tabs: [],
      active: 0
    }),

    computed: {
      storageKey()
      {
        return `vue-tabs-component.cache.${window.location.host}${window.location.pathname}`;
      }
    },

    created()
    {
      this.tabs = this.$children;
    },

    mounted()
    {
      this.select(this.active);
    },

    methods: {

      select(index, ev)
      {
        if (ev)
        {
          ev.preventDefault();
        }

        const currentTab = this.tabs[index];

        if (currentTab.disabled)
        {
          return;
        }

        this.tabs.forEach((tab, tabIndex) =>
        {
          tab.active = index === tabIndex;
        });

        //this.$emit('changed', { tab: selectedTab });
        //this.activeTabHash = selectedTab.hash;
        //this.activeTabIndex = this.getTabIndex(selectedTabHash);
        //this.lastActiveTabHash = this.activeTabHash = selectedTab.hash;
        //expiringStorage.set(this.storageKey, selectedTab.hash, this.cacheLifetime);
      },
    }
  }
</script>

<style lang="scss">
  .ui-tabs
  {
    
  }

  .ui-tabs-list
  {
    border-bottom: 1px solid var(--color-line);
    padding: 0 10px;
  }

  .ui-tabs-list-item
  {
    display: inline-flex;
    align-items: center;
    height: 60px;
    padding: 0 20px;
    margin: 0;
    font-size: var(--font-size);
    color: var(--color-fg-light);
    position: relative;
    overflow: hidden;
    transition: color 0.2s ease;

    &:hover
    {
      color: var(--color-fg);
    }

    &:before
    {
      display: none;
    }

    &[disabled]
    {
      cursor: default;
      color: var(--color-fg-light);
    }

    &:after
    {
      content: '';
      height: 4px;
      border-radius: 4px 4px 0 0;
      background: var(--color-line);
      position: absolute;
      left: 18px;
      right: 18px;
      bottom: 0;
      transform: translateY(5px) scaleX(0.5);
      transition: transform 0.2s ease;
    }

    &.is-active
    {
      font-weight: 700;
      color: var(--color-fg);

      &:after
      {
       transform: translateY(0) scaleX(1);
      }
    }
  }
</style>