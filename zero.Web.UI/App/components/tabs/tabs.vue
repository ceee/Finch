<template>
  <div class="ui-tabs">
    <div role="tablist" class="ui-tabs-list">
      <button type="button" v-for="(tab, index) in tabs" class="ui-tabs-list-item" :key="index" :class="{ 'is-active': tab.active, 'has-errors': tab.hasErrors }" :disabled="tab.disabled" 
              :aria-selected="tab.active" role="tab" @click="select(index)">
        <i v-if="tab.hasErrors" class="ui-tabs-list-item-error fth-alert-circle"></i>
        <ui-localize :value="tab.label" />
        <i v-if="tab.countOutput > 0" class="ui-tabs-list-item-count">{{tab.countOutput}}</i>
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
      cache: {
        type: String
      },
      active: {
        type: Number,
        default: 0
      }
    },

    data: () => ({
      storageKey: null,
      tabs: []
    }),

    mounted()
    {
      this.cacheKey = this.cache ? `zero.ui-tabs.cache.${this.cache}` : null;
      this.tabs = this.$children;

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
  .ui-tabs
  {
    
  }

  .ui-header-bar + .ui-tabs > .ui-tabs-list
  {
    padding-top: 0;
  }

  .ui-tabs-list
  {
    /*border-bottom: 1px solid var(--color-line);*/
    padding: var(--padding) var(--padding) 0;
    margin-bottom: calc(var(--padding) * -1);
    //margin-left: -1px;
  }

  /*.ui-tabs-items > .ui-tab:first-child.ui-box:not(.is-blank)
  {
    border-top-left-radius: 0;
  }*/

  .ui-tabs-list-item
  {
    display: inline-flex;
    align-items: center;
    height: 58px;
    //overflow: hidden;
    padding: 0 var(--padding);
    font-size: var(--font-size);
    color: var(--color-text);
    position: relative;
    transition: color 0.2s ease;
    border-radius: var(--radius) var(--radius) 0 0;
    background: var(--color-box-light);
    //border: 1px solid transparent;
    //border-bottom: none;
    & + .ui-tabs-list-item
    {
      margin-left: 4px;
    }

    &:hover
    {
      color: var(--color-text);
    }

    &[disabled]
    {
      cursor: default;
      color: var(--color-text-dim);
    }

    &.is-active
    {
      font-weight: 700;
      color: var(--color-text);
      background: var(--color-box);
      box-shadow: var(--shadow-short);
      //border-top: 3px solid var(--color-primary);

      .ui-tabs-list-item-count
      {
        background: var(--color-box-light);
      }

      /*&:before
      {
        content: '';
        position: absolute;
        left: 0;
        right: 0;
        top: 0;
        height: 3px;
        border-top-left-radius: 3px;
        border-top-right-radius: 3px;
        background: var(--color-primary);
        display: inline-block;
      }*/
    }

    &.has-errors
    {
      color: var(--color-accent-error);

      &.is-active
      {
        color: var(--color-accent-error);

        &:before
        {
          background: var(--color-accent-error);
        }
      }
    }

    &.is-active:after
    {
      content: '';
      position: absolute;
      left: 0;
      right: 0;
      bottom: -4px;
      height: 5px;
      background: var(--color-box);
      z-index: 1;
    }
  }

  .ui-tabs-list-item-count
  {
    font-style: normal;
    font-size: 12px;
    overflow: hidden;
    float: right;
    padding: 2px 6px;
    background: var(--color-box);
    border-radius: 10px;
    margin-left: 8px;
    margin-right: -4px;
    margin-top: -1px;
    font-weight: bold;
    color: var(--color-text);
  }

  .ui-tabs-list-item-error
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