<template>
  <div class="ui-pagination" v-if="pages > 1">
    <ui-icon-button class="ui-pagination-next" type="white" title="@ui.pagination.previous" icon="fth-chevron-left" :disabled="page < 2" @click="set(page - 1)" />
    <div class="ui-pagination-select">
      <select :value="page" @change="selectChanged">
        <option v-for="value in values" v-bind:value="value">{{value}}</option>
      </select>
      <button type="button" class="ui-button type-blank caret-down">
        <span class="ui-button-text" v-localize="{ key: '@ui.pagination.xofy', tokens: { x: page, y: pages }}"></span>
        <i class="ui-button-caret fth-chevron-down"></i>
      </button>
    </div>
    <ui-icon-button class="ui-pagination-next" type="white" title="@ui.pagination.next" icon="fth-chevron-right" :disabled="page >= pages" @click="set(page + 1)" />
  </div>
</template>


<script>
  export default {
    name: 'uiPagination',

    props: {
      pages: {
        type: Number,
        default: 1
      },
      page: {
        type: Number,
        default: 1
      }
    },

    computed: {
      values: function ()
      {
        return Array.apply(null, Array(this.pages)).map(function (_, i) { return i + 1; });
      }
    },

    methods: {

      set(page)
      {
        this.$emit('update:page', page);
        this.$emit('change', page);
      },

      selectChanged(ev, a)
      {
        this.set(+ev.target.value);
      }

    }
  }
</script>

<style lang="scss">
  .ui-pagination
  {
    display: flex;
    justify-content: center;
    margin-top: var(--padding);
    align-items: center;
  }

  .ui-pagination-select
  {
    margin: 0 30px;
    position: relative;

    .ui-button
    {
      padding: 0 2px;
    }

    select
    {
      width: 100%;
      position: absolute;
      z-index: 1;
      left: 0;
      right: 0;
      top: 0;
      bottom: 0;
      opacity: 0;
    }
  }
</style>