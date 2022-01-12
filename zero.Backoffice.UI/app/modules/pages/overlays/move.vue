<template>
  <ui-trinity class="pages-move">
    <template v-slot:header>
      <ui-header-bar title="@ui.move.title" :back-button="false" :close-button="true" @close="config.close(true)" />
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


<script lang="ts">
  import api from '../api';
  import * as notifications from '../../../services/notification';

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
      id: null,
      newParentId: null
    }),


    mounted()
    {
      this.newParentId = this.model.parentId;
      this.selected = [this.model.parentId];
      this.hierarchy = []; //(await api.getHierarchy(this.model.parentId)).data.map(x => x.id);
      //this.hierarchy.splice(this.hierarchy.length - 1, 1);
      this.id = this.model.id;
      this.loaded = true;
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

        const response = await api.tree.getChildren(id, this.model.parentId);
        let items = response.data;

        items.forEach(item =>
        {
          if (this.model.id == item.id || item.id === 'recyclebin')
          {
            item.disabled = true;
          }
        });

        if (!parent)
        {
          items.splice(0, 1, {
            id: null,
            parentId: null,
            image: null,
            name: '@page.root',
            children: 0,
            root: true,
            sort: 0,
            icon: 'fth-arrow-down-circle',
            root: true
          });
        }

        this.cache[id] = items;
        return items;
      },

      async onSave()
      {
        if (this.model.parentId == this.newParentId)
        {
          this.config.close();
          return;
        }

        this.state = 'loading';

        const result = await api.move(this.id, (this.newParentId || 'root'));

        if (result.success)
        {
          this.state = 'success';
          this.config.confirm(result.data);
        }
        else
        {
          this.state = 'error';
          notifications.error('@errors.onmove.title', result.errors[0].message);
        }
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