<template>
  <ui-trinity class="pages-move">
    <template v-slot:header>
      <ui-header-bar title="@ui.move.title" :back-button="false" :close-button="true" />
    </template>
    <template v-slot:footer>
      <ui-button type="light onbg" label="@ui.close" @click="config.close" />
      <ui-button type="accent" label="@ui.move.action" @click="onSave" :state="state" :disabled="!loaded" />
    </template>

    <div v-if="loaded">
      <p class="pages-move-text" v-localize:html="{ key: '@ui.move.text', tokens: { name: model.name } }"></p>
      <div class="ui-box pages-move-items">
        <ui-tree v-if="loaded" ref="tree" :get="getItems" @select="onSelect" :selection="selected" />
      </div>
    </div>
    <ui-loading v-else />
  </ui-trinity>
</template>


<script>
  import api from '../api';
  //import MediaFolderApi from 'zero/api/media-folder.js';
  //import MediaApi from 'zero/api/media.js';
  //import Notification from 'zero/helpers/notification.js'

  export default {

    props: {
      model: Object,
      config: Object
    },

    data: () => ({
      hierarchy: [],
      items: [],
      selected: [],
      state: 'default',
      cache: {},
      prevItem: null,
      loaded: false,
      ids: [],
      newParentId: null
    }),


    async mounted()
    {
      this.newParentId = this.model.parentId;
      this.selected = [this.model.parentId];
      this.hierarchy = (await api.getHierarchy(this.model.parentId)).data.map(x => x.id);
      this.hierarchy.splice(this.hierarchy.length - 1, 1);
      this.ids = this.model.items.map(x => x.id);
      this.loaded = true;
    },


    computed: {
      multiple()
      {
        return this.ids.length > 1;
      }
    },


    methods: {

      onSelect(item)
      {
        item.isSelected = true;

        if (this.prevItem && this.prevItem.id != item.id)
        {
          this.prevItem.isSelected = false;
        }

        this.prevItem = item;
        this.newParentId = item.id;
        this.selected = [item];
        //this.config.confirm(item);
      },

      async getItems(parent)
      {
        const id = !parent ? 'root' : parent;

        if (this.cache[id])
        {
          return Promise.resolve(this.cache[id]);
        }

        const response = await api.folders.getChildren(id, false, { pageSize: 50 }); // TODO we need paging support for <ui-tree />
        const items = response.data;

        if (!parent)
        {
          items.splice(0, 0, {
            id: null,
            parentId: null,
            image: null,
            name: '@page.root',
            children: 0,
            isFolder: true,
            root: true
          });
        }

        const result = items.map(item =>
        {
          return {
            id: item.id,
            parentId: item.parentId,
            sort: 0,
            name: item.name,
            icon: item.root ? 'fth-arrow-down-circle' : (item.isFolder ? 'fth-folder' : 'fth-file'),
            isOpen: this.hierarchy.indexOf(item.id) > -1,
            modifier: null,
            hasChildren: item.children > 0,
            childCount: item.children,
            isInactive: false,
            hasActions: false,
            disabled: item.id == 'recyclebin' || this.ids.indexOf(item.id) > -1
          };
        })

        this.cache[id] = result;
        return result;
      },

      async onSave()
      {
        if (this.model.parentId == this.newParentId)
        {
          this.config.close();
          return;
        }

        this.state = 'loading';

        if (this.multiple)
        {
          const result = await api.bulk.move(this.ids, this.newParentId);
          this.config.confirm(result);
        }
        else
        {
          const result = await api.move(this.ids[0], this.newParentId);
          this.config.confirm(result);
        }


        //(this.config.isFolder ? MediaFolderApi : MediaApi).move(this.model.id, this.selected.id).then(res =>
        //{
        //  if (res.success)
        //  {
        //    this.state = 'success';
        //    this.config.confirm(res.model);
        //  }
        //  else
        //  {
        //    this.state = 'error';
        //    Notification.error(res.errors[0].message);
        //  }
        //});
      }
    }
  }
</script>

<style lang="scss">
  .pages-move .ui-box
  {
    padding: 16px 0;

    .ui-tree-item.is-disabled
    {
      opacity: .5;
    }

    .ui-tree-item.is-selected, .ui-tree-item:hover:not(.is-disabled)
    {
      background: var(--color-tree-selected);
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

  .pages-move-text
  {
    margin: 0 0 20px;
  }
</style>