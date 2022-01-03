<template>
  <ui-trinity class="media-overlay">
    <template v-slot:header>
      <ui-header-bar title="Select media" :close-button="true" @close="config.close">
        <template v-slot:title>
          <h2 class="ui-header-bar-title">
            <span class="media-items-hierarchy-item">
              <button type="button" v-localize="'@media.name'" @click="selectItem('root')"></button>
              <ui-icon class="-chevron" v-if="hierarchy.length > 0" symbol="fth-chevron-right" />
            </span>
            <span v-for="(item, index) in hierarchy" :key="item.id" class="media-items-hierarchy-item">
              <button type="button" v-localize="item.name" @click="selectItem(item.id)"></button>
              <ui-icon class="-chevron" v-if="index < hierarchy.length - 1" symbol="fth-chevron-right" />
            </span>
          </h2>
        </template>
      </ui-header-bar>
    </template>
    <template v-slot:footer>
      <ui-button type="light onbg" label="@ui.close" @click="config.close" />
    </template>

    <div v-if="loaded">
      <div class="media-items" :class="{ 'is-selecting': selected.length > 0 }">
        <ui-datagrid ref="grid" v-model="gridConfig" @select="onSelected" @count="count = $event">

          <template v-slot:before>
            <media-drop :folder-id="parentId" @completed="refresh" />
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
              <ui-dropdown-button label="@ui.open.title" icon="fth-arrow-right" @click="goToFolder(props.item.id)" />
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
    </div>
    <ui-loading v-else />
  </ui-trinity>
</template>


<script>
  import api from '../api';
  import MediaItem from '../pages/overview/overview-item.vue';
  import MediaSelection from '../pages/overview/overview-selection.vue';
  import MediaDrop from '../pages/overview/drop.vue';
  //import MediaFolderApi from 'zero/api/media-folder.js';
  //import MediaApi from 'zero/api/media.js';
  //import Notification from 'zero/helpers/notification.js'

  export default {

    props: {
      model: Object,
      config: Object
    },

    components: { MediaItem, MediaSelection, MediaDrop },

    data: () => ({
      id: null,
      hierarchy: {},
      search: null,
      selected: [],
      gridConfig: {
        search: null,
        width: 180,
        selectable: true,
        items: null,
        setQuery: false,
        scrollContainer: null
      },
    }),


    created()
    {
      this.gridConfig.items = this.getItems;
      this.gridConfig.scrollContainerSelector = '.media-overlay content';
      this.id = this.config.parentId || 'root';
      this.loaded = true;
    },


    methods: {

      async getItems(query)
      {
        if (!query)
        {
          query = {};
        }

        query.search = this.gridConfig.search;
        query.pageSize = 50;

        const hierarchy = await api.folders.getHierarchy(this.id);
        this.hierarchy = hierarchy.data;

        return await api.folders.getChildren(this.id, true, query);
      },


      async refresh()
      {
        await this.$refs.grid.update();
      },


      async onItemClick(item)
      {
        if (item.isFolder)
        {
          await this.selectItem(item.id);
        }
        else
        {
          this.config.confirm(item);
        }
      },


      async selectItem(id)
      {
        this.id = id;
        await this.refresh();
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

    }
  }
</script>

<style lang="scss">
  .media-items-hierarchy-item
  {
    font-family: var(--font);
    margin: 0;
    font-size: var(--font-size-l);
    font-weight: 400;
    color: var(--color-text-dim);
    min-height: 22px;
    display: flex;
    align-items: center;

    button
    {
      color: var(--color-text-dim);
      font-size: var(--font-size-l);
    }

    &:last-child button
    {
      font-weight: 700;
      color: var(--color-text);
    }

    button:hover
    {
      color: var(--color-text);
    }
  }
</style>