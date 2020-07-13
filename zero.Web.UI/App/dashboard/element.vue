<template>
  <div class="ui-dashboard-element" :class="classList">
    <header class="ui-dashboard-element-head">
      <h2 class="ui-headline ui-dashboard-element-title">Element</h2>
      <ui-icon-button icon="fth-more-horizontal" />
    </header>
    <content class="ui-dashboard-element-content">

    </content>
  </div>
</template>


<script>
  export default {
    name: 'uiDashboardElement',

    props: {
      value: {
        type: String,
        default: null
      },
      wide: {
        type: Boolean,
        default: false
      },
      high: {
        type: Boolean,
        default: false
      },
      theme: {
        type: String,
        default: 'default'
      }
    },

    data: () => ({
      id: null
    }),

    computed: {
      classList()
      {
        return {
          'is-high': this.high,
          'is-wide': this.wide,
          ['theme-' + this.theme]: true
        };
      }
    }
  }
</script>

<style lang="scss">
  $dashboard-element-padding: 20px;

  .ui-dashboard-element
  {
    display: grid;
    grid-template-rows: auto 1fr;
    position: relative;
    background: var(--color-bg-light);
    border-radius: var(--radius);
    box-shadow: var(--color-shadow-short);
    color: var(--color-fg);

    &.is-wide
    {
      grid-column-end: span 2;
    }

    &.is-high
    {
      grid-row-end: span 2;
    }
  }

  .ui-dashboard-element-content
  {
    padding: $dashboard-element-padding;
  }

  .ui-dashboard-element-head
  {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 12px $dashboard-element-padding;
    /*border-bottom: 1px solid var(--color-line-light);*/

    .ui-icon-button
    {
      background: none;
      margin-right: -8px;
    }
  }

  h2.ui-dashboard-element-title
  {
    margin: 0;
    font-size: var(--font-size);
  }
</style>