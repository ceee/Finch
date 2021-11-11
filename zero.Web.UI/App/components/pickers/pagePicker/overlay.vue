<template>
  <ui-overlay-editor class="ui-pagepicker-overlay">
    <template v-slot:header>
      <ui-header-bar :title="config.title" :back-button="false" :close-button="true" />
    </template>
    <template v-slot:footer>
      <ui-button type="light onbg" :label="config.closeLabel" :parent="config.rootId" @click="config.hide"></ui-button>
    </template>

    <div v-if="opened" class="ui-box ui-pagepicker-overlay-items">
      <ui-tree ref="tree" :get="getItems" :parent="config.rootId" @select="onSelect" />
    </div>
  </ui-overlay-editor>
</template>


<script>
  import PagesApi from 'zero/api/pages.js'

  export default {

    props: {
      model: String,
      config: Object
    },

    data: () => ({
      opened: false
    }),

    computed: {
      disabledIds()
      {
        return this.config.disabledIds || [];
      }
    },


    mounted()
    {
      setTimeout(() => this.opened = true, 300);
    },


    methods: {

      onSelect(item)
      {
        this.config.confirm(item);
      },

      // get tree items
      getItems(parent)
      {
        return PagesApi.getChildren(parent, this.model).then(res =>
        {
          res = res.filter(x => x.id !== 'recyclebin');
          
          res.forEach(item =>
          {
            if (item.id === this.model)
            {
              item.isSelected = true;
            }
            if (this.disabledIds.indexOf(item.id) > -1)
            {
              item.disabled = true;
            }
            item.hasActions = false;
          });

          return res;
        });
      }
    }
  }
</script>

<style lang="scss">
  .ui-pagepicker-overlay content
  {
    padding-top: 0;
  }

  .ui-box.ui-pagepicker-overlay-items
  {
    margin: 0;
    padding: 20px 0;

    .ui-tree-item.is-selected, .ui-tree-item:hover:not(.is-disabled)
    {
      background: var(--color-bg-xxlight);
    }

    & +.ui-box
    {
      margin-top: var(--padding);
    }

    .ui-tree-item.is-selected
    {
      &:after
      {
        font-family: "Feather";
        content: "\e83e";
        font-size: 16px;
        color: var(--color-primary);
      }
      
      .ui-tree-item-text
      {
        font-weight: bold;
      }
    }
  }
</style>