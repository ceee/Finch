<template>
  <ui-trinity class="media-overlay">
    <template v-slot:header>
      <ui-header-bar :close-button="true" @close="config.close(true)">
        <template v-slot:title>
          <h2 class="ui-header-bar-title">
            <span class="media-items-hierarchy-item">
              <button type="button" v-localize="'@media.list'" @click="selectItem(null)"></button>
              <ui-icon class="-chevron" v-if="hierarchy.length > 0 || searchMode" symbol="fth-chevron-right" />
            </span>
            <span v-for="(item, index) in hierarchy" :key="item.id" class="media-items-hierarchy-item">
              <button type="button" v-localize="item.name" @click="selectItem(item.id)"></button>
              <ui-icon class="-chevron" v-if="index < hierarchy.length - 1 || searchMode" symbol="fth-chevron-right" />
            </span>
          </h2>
        </template>

        <template v-if="selected.length < 1">
          <ui-search v-model="search" class="onbg" />
          <ui-button :disabled="searchMode" type="accent" label="@media.addfolder" @click="editFolder()" />
        </template>
        <media-selection v-else :selected="selected" @clear="clearSelection" @move="move" @remove="remove" />
      </ui-header-bar>
    </template>

    <template v-slot:footer>
      <ui-button type="light onbg" label="@ui.close" @click="config.close" />
    </template>

    <div v-if="loaded" class="media-items" :class="{ 'is-selecting': selected.length > 0 }">
      <ui-datagrid ref="grid" v-model="gridConfig" @select="onSelected" @count="count = $event">

        <template v-slot:before>
          <media-drop v-if="!searchMode" :folder-id="parentId" @completed="refresh" />
        </template>

        <template v-if="selected.length < 1" v-slot:actions="props">
          <template v-if="props.item && props.item.isFolder">
            <ui-dropdown-button label="@ui.rename.title" icon="fth-edit-2" @click="editFolder(props.item)" />
            <ui-dropdown-button label="@ui.move.title" icon="fth-corner-down-right" @click="move([props.item])" />
            <ui-dropdown-button label="@ui.selection.select" icon="fth-check-circle-2" @click="$refs.grid.select(props.item)" />
            <ui-dropdown-separator />
            <ui-dropdown-button label="@ui.delete" icon="fth-trash" @click="remove([props.item])" />
          </template>
          <template v-if="props.item && !props.item.isFolder">
            <ui-dropdown-button label="@ui.move.title" icon="fth-corner-down-right" @click="move([props.item])" />
            <ui-dropdown-button label="@ui.selection.select" icon="fth-check-circle-2" @click="$refs.grid.select(props.item)" />
            <ui-dropdown-separator />
            <ui-dropdown-button label="@ui.delete" icon="fth-trash" @click="remove([props.item])" />
          </template>
        </template>

        <template v-slot:default="props">
          <media-item :value="props.item" :selected="props.selected" @click="onItemClick" />
        </template>
      </ui-datagrid>
    </div>
  </ui-trinity>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import api from '../api';
  import MediaItem from '../pages/overview/overview-item.vue';
  import MediaSelection from '../pages/overview/overview-selection.vue';
  import MediaDrop from '../pages/overview/drop.vue';
  import actions from '../pages/overview/overview-actions';
  import * as overlays from '../../../services/overlay';

  export default defineComponent({

    props: {
      model: Object,
      config: Object
    },

    components: { MediaItem, MediaSelection, MediaDrop },

    data: () => ({
      loaded: false,
      paging: {},
      hierarchy: [],
      search: null,
      selected: [],
      parentId: null,
      gridConfig: {
        width: 180,
        selectable: true,
        items: null,
        page: 1,
        pageSize: 50,
        setQuery: false
      },
    }),


    computed: {
      id()
      {
        return this.parentId || 'root';
      },
      searchMode()
      {
        return !!this.search;
      }
    },


    watch: {
      parentId: function (val)
      {
        this.clearSelection();
      },
      search(val)
      {
        if (this.loaded && this.$refs.grid)
        {
          this.gridConfig.page = 1;
          this.$refs.grid.filter.page = 1;
          this.$refs.grid.search(val);
        }
      }
    },


    mounted()
    {
      this.parentId = this.config.model.parentId;
      this.search = this.config.model.search;
      this.gridConfig.page = this.config.model.page || 1;
      this.gridConfig.items = this.getItems;
      this.gridConfig.scrollContainerSelector = '.media-overlay content';
      this.loaded = true;
    },


    methods: {

      async getItems(query)
      {
        if (!query)
        {
          query = {};
        }

        if (!query.search || !this.hierarchy.length)
        {
          const hierarchy = await api.folders.getHierarchy(this.id);
          this.hierarchy = hierarchy.data;
        }


        if (query.search)
        {
          return await api.search(this.search, this.id, query);
        }

        return await api.folders.getChildren(this.id, true, { ...query });
      },


      async onItemClick(item)
      {
        if (item.isFolder)
        {
          await this.selectItem(item.id);
        }
        else
        {
          this.config.confirm({
            item,
            query: {
              search: this.search,
              parentId: this.parentId,
              page: this.$refs.grid.filter.page
            }
          });
        }
      },


      async selectItem(id)
      {
        this.parentId = id;
        this.gridConfig.page = 1;
        this.$refs.grid.filter.page = 1;
        this.$refs.grid.filter.search = null;
        this.search = null;
        await this.refresh();
      },


      async refresh()
      {
        await this.$refs.grid.update();
      },


      getLink(item)
      {
        if (item.isFolder)
        {
          return { name: 'media', params: { parentId: item.id } };
        }

        return { name: 'media-edit', params: { id: item.id } };
      },


      onSelected(selection)
      {
        this.selected = selection;
      },


      clearSelection()
      {
        if (this.$refs.grid)
        {
          this.$refs.grid.clearSelection();
        }
      },


      async move(items: any[])
      {
        const updates = await actions.move(items);
        if (updates)
        {
          this.clearSelection();
          await this.$refs.grid.update();
        }
      },


      async remove(items: any[])
      {
        const deletes = await actions.remove(items);
        if (deletes)
        {
          this.clearSelection();
          await this.$refs.grid.update();
        }
      },


      async editFolder(item)
      {
        const result = await overlays.open({
          component: () => import('./editfolder.vue'),
          model: item || { parentId: this.parentId },
          display: 'dialog'
        });

        if (result.eventType === 'confirm' && result.value)
        {
          await this.$refs.grid.update();
        }

        //.then(item => this.goToFolder(item.model.id));
      }
    }

  });
</script>



<style lang="scss">
  .media
  {
    width: 100%;
    height: 100vh;
    overflow-y: auto;
  }

  .media-content
  {
    height: 100vh;
    overflow-y: auto;
  }

  .media-items-hierarchy-item
  {
    font-family: var(--font);
    margin: 0;
    font-size: var(--font-size-xl);
    font-weight: 400;
    color: var(--color-text-dim);
    min-height: 22px;
    display: flex;
    align-items: center;

    a, button
    {
      color: var(--color-text-dim);
    }

    button
    {
      color: var(--color-text-dim);
      font-size: var(--font-size-l);
    }

    &:last-child a, &:last-child button
    {
      font-weight: 700;
      color: var(--color-text);
    }

    a:hover, button:hover
    {
      color: var(--color-text);
    }

    a .ui-icon, button .ui-icon
    {
      position: relative;
      top: 2px;
    }
  }
</style>