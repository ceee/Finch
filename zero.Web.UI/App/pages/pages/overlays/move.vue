<template>
  <ui-overlay-editor class="pages-move">
    <template v-slot:header>
      <ui-header-bar title="@ui.move.title" :back-button="false" :close-button="true" />
    </template>
    <template v-slot:footer>
      <ui-button type="light onbg" label="@ui.close" @click="config.hide" />
      <ui-button type="primary" label="@ui.move.action" @click="onSave" :state="state" />
    </template>

    <p class="pages-move-text" v-localize:html="{ key: '@ui.move.text', tokens: { name: model.name } }"></p>
    <div class="ui-box pages-move-items">
      <ui-tree ref="tree" :get="getItems" mode="select" @select="onSelect" />
    </div>
  </ui-overlay-editor>
</template>


<script>
  import PageTreeApi from 'zero/api/page-tree.js'
  import PagesApi from 'zero/api/pages.js';
  import Notification from 'zero/helpers/notification.js'

  export default {

    props: {
      isCopy: {
        type: Boolean,
        default: false
      },
      model: Object,
      config: Object
    },

    data: () => ({
      items: [],
      selected: null,
      state: 'default',
      cache: {},
      prevItem: null,
      selected: null
    }),


    mounted()
    {
      this.selected = this.model;
    },


    methods: {

      onSelect(id)
      {
        this.selected = id;
      },

      getItems(parent)
      {
        const key = !parent ? '__root' : parent;

        if (this.cache[key])
        {
          return Promise.resolve(this.cache[key]);
        }

        return PageTreeApi.getChildren(parent).then(response =>
        {
          if (!parent)
          {
            response.splice(0, 0, {
              id: null,
              parentId: null,
              sort: 0,
              name: '@page.root',
              icon: 'fth-arrow-down-circle',
              isOpen: false,
              modifier:	null,
              hasChildren: false,
              childCount: 0,
              isInactive: false,
              hasActions: false
            });
          }

          response.forEach(item =>
          {
            item.disabled = item.id === 'recyclebin' || item.id == this.model.id;
            item.hasActions = false;
          });

          this.cache[key] = response;
          return response;
        });
      },

      onSave()
      {
        if (this.model.parentId == this.selected)
        {
          this.config.close();
          return;
        }

        this.state = 'loading';

        PagesApi.move(this.model.id, this.selected).then(res =>
        {
          if (res.success)
          {
            this.state = 'success';
            this.config.confirm(res.model);
          }
          else
          {
            this.state = 'error';
            Notification.error(res.errors[0].message);
          }
        });
      }
    }
  }
</script>

<style lang="scss">
  .pages-move .ui-box
  {
    margin: 0;
    padding: 16px 0;

    .ui-tree-item.is-disabled
    {
      opacity: .5;
    }
  }

  .pages-move content
  {
    padding-top: 0;
  }

  .pages-move-text
  {
    margin: 0 0 20px;
  }
</style>