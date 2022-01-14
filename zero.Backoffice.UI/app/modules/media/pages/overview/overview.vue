<template>
  <div class="media-content">
    <ui-header-bar :back-button="!!parentId">
      <template v-slot:title>
        <h2 class="ui-header-bar-title">
          <span class="media-items-hierarchy-item">
            <ui-link :to="{ name: 'media' }" v-localize="'@media.name'"></ui-link>
            <ui-icon class="-chevron" v-if="hierarchy.length > 0 || searchMode" symbol="fth-chevron-right" />
          </span>
          <span v-for="(item, index) in hierarchy" :key="item.id" class="media-items-hierarchy-item">
            <ui-link :to="{ name: 'media', params: { parentId: item.id } }" v-localize="item.name"></ui-link>
            <ui-icon class="-chevron" v-if="index < hierarchy.length - 1 || searchMode" symbol="fth-chevron-right" />
          </span>
          <span v-if="searchMode" class="media-items-hierarchy-item">
            <ui-link :to="{ name: 'media', params: { parentId: id }, query: { search: (search || '') } }"><ui-icon symbol="fth-search" /></ui-link>
          </span>
        </h2>
      </template>

      <template v-if="selected.length < 1">
        <ui-search v-model="search" class="onbg" />
        <ui-button :disabled="searchMode" type="accent" label="@media.addfolder" @click="editFolder()" />
      </template>
      <media-selection v-else :selected="selected" @clear="clearSelection" @move="move" @remove="remove" />
    </ui-header-bar>

    <div class="ui-view-box">
      <div class="media-items" :class="{ 'is-selecting': selected.length > 0 }">
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
              <ui-dropdown-button label="@ui.open.title" icon="fth-arrow-right" @click="goToFolder(props.item.id)" />
              <ui-dropdown-button label="@ui.move.title" icon="fth-corner-down-right" @click="move([props.item])" />
              <ui-dropdown-button label="@ui.selection.select" icon="fth-check-circle-2" @click="$refs.grid.select(props.item)" />
              <ui-dropdown-separator />
              <ui-dropdown-button label="@ui.delete" icon="fth-trash" @click="remove([props.item])" />
            </template>
          </template>

          <template v-slot:default="props">
            <media-item :value="props.item" :selected="props.selected" :link="getLink(props.item)" />
          </template>
        </ui-datagrid>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
  import { defineComponent } from 'vue';
  import api from '../../api';
  import MediaItem from './overview-item.vue';
  import MediaSelection from './overview-selection.vue';
  import MediaDrop from './drop.vue';
  import actions from './overview-actions';
  import * as overlays from '../../../../services/overlay';

  export default defineComponent({
    props: ['parentId'],

    components: { MediaItem, MediaSelection, MediaDrop },

    data: () => ({
      loaded: false,
      paging: {},
      hierarchy: [],
      search: null,
      selected: [],
      gridConfig: {
        width: 180,
        selectable: true,
        items: null
      },
    }),


    computed: {
      id()
      {
        return this.$route.params.parentId || 'root';
      },
      searchMode()
      {
        return !!this.search;
      }
    },


    watch: {
      '$route': function (val)
      {
        this.clearSelection();
      },
      search(val)
      {
        if (this.loaded && this.$refs.grid)
        {
          this.$refs.grid.search(val);
        }
      }
    },


    created()
    {
      this.gridConfig.items = this.getItems;
      this.search = this.$route.query.search;
      this.loaded = true;
    },


    methods: {

      async getItems(query)
      {
        if (!query)
        {
          query = {};
        }

        query.pageSize = 50;

        if (!query.search || !this.hierarchy.length)
        {
          const hierarchy = await api.folders.getHierarchy(this.id);
          this.hierarchy = hierarchy.data;
        }

        if (query.search)
        {
          return await api.search(this.search, this.id, query);
        }

        return await api.folders.getChildren(this.id, true, query);
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
          component: () => import('../../overlays/editfolder.vue'),
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

    a
    {
      color: var(--color-text-dim);
    }

    &:last-child a
    {
      font-weight: 700;
      color: var(--color-text);
    }

    a:hover
    {
      color: var(--color-text);
    }

    a .ui-icon
    {
      position: relative;
      top: 2px;
    }
  }
</style>