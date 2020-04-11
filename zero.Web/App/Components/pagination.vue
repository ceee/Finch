<template>
  <div class="ui-pagination" v-if="pages > 1">
    <ui-icon-button class="ui-pagination-next" type="white" title="Previous" icon="fth-chevron-left" :disabled="page < 2" @click="set(page - 1)" />
    <ui-dropdown align="bottom">
      <template v-slot:button>
        <button type="button" class="ui-button type-blank caret-down ui-pagination-select">
          <span class="ui-button-text" v-localize="{ key: '@ui.page_xofy', tokens: { x: page, y: pages }}"></span>
          <i class="ui-button-caret fth-chevron-down"></i>
        </button>
      </template>
      <div>page select</div>
      <!--<ui-dropdown-list />-->
    </ui-dropdown>
    <ui-icon-button class="ui-pagination-next" type="white" title="Next" icon="fth-chevron-right" :disabled="page >= pages" @click="set(page + 1)" />
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

    data: () => ({
      
    }),

    mounted ()
    {
      
    },

    methods: {

      set(page)
      {
        this.$emit('update:page', page);
        this.$emit('change', page);
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
    margin: 0 20px;
  }
</style>