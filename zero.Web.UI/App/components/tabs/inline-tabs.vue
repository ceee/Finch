<template>
  <div class="ui-inline-tabs">
    <nav role="tablist" class="ui-inline-tabs-list">
      <button type="button" v-for="(tab, index) in tabs" class="ui-inline-tabs-list-item" :key="index" :class="{ 'is-active': tab.active, 'has-errors': tab.error }" :disabled="tab.disabled" 
              :aria-selected="tab.active" role="tab" @click="select(index)">
        <i v-if="tab.error" class="ui-inline-tabs-list-item-error fth-alert-circle"></i>
        <span v-localize="tab.label"></span>
        <i v-if="forceCount || tab.count > 0" class="ui-inline-tabs-list-item-count">{{tab.count}}</i>
      </button>
    </nav>
    <div class="ui-inline-tabs-items">
      <slot />
    </div>
  </div>
</template>


<script>
  export default {
    name: 'uiTabs',

    props: {
      cache: {
        type: String
      },
      active: {
        type: Number,
        default: 0
      },
      forceCount: {
        type: Boolean,
        default: false
      }
    },

    data: () => ({
      storageKey: null,
      tabs: []
    }),

    created()
    {
      this.cacheKey = this.cache ? `zero.ui-inline-tabs.cache.${this.cache}` : null;
      this.tabs = this.$children;
    },

    mounted()
    {
      if (this.cache)
      {
        const cachedActiveTab = localStorage.getItem(this.cacheKey);
        if (cachedActiveTab !== null)
        {
          this.select(+cachedActiveTab);
          return;
        }
      }

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

        if (this.cache)
        {
          localStorage.setItem(this.cacheKey, index);
        }

        //this.$emit('changed', { tab: selectedTab });
        //this.activeTabHash = selectedTab.hash;
        //this.activeTabIndex = this.getTabIndex(selectedTabHash);
        //this.lastActiveTabHash = this.activeTabHash = selectedTab.hash;
        //expiringStorage.set(this.storageKey, selectedTab.hash, this.cacheLifetime);
      }
    }
  }
</script>

<style lang="scss">
  .ui-inline-tabs-list
  {
    display: flex;
    flex-wrap: wrap;
    gap: 16px;
    margin-bottom: var(--padding);
    margin-left: -2px;
  }

  .ui-inline-tabs-list-item
  {
    display: inline-flex;
    align-items: center;
    padding: 6px 6px 6px 12px;
    background: transparent;
    border-radius: 30px;
    font-size: var(--font-size-s);

    .ui-inline-tabs-list-item-count
    {
      display: inline-flex;
      justify-content: center;
      align-items: center;
      width: 20px;
      height: 20px;
      border-radius: 10px;
      background: var(--color-button-light);
      margin-left: 12px;
      font-size: 11px;
      line-height: 1;
      color: var(--color-text-dim);
      font-style: normal;
    }

    &.is-active
    {
      background: var(--color-button-light);

      .ui-inline-tabs-list-item-count
      {
        background: var(--color-box);
        color: var(--color-text);
      }
    }
  }

  .ui-inline-tabs-list-item-error
  {
    display: inline-block;
    float: left;
    font-size: 16px;
    margin-right: 6px;
    margin-left: -4px;
    position: relative;
    margin-top: -4px;
    top: 1px;
  }
</style>