<template>
  <div class="media-content">
    <ui-header-bar :back-button="!!parentId">
      <template v-slot:title>
        <h2 class="ui-header-bar-title">
          <span class="media-items-hierarchy-item">
            <router-link :to="{ name: 'media' }" v-localize="'@media.name'"></router-link>
            <ui-icon class="-chevron" v-if="hierarchy.length > 0" symbol="fth-chevron-right" />
          </span>
          <span v-for="(item, index) in hierarchy" :key="item.id" class="media-items-hierarchy-item">
            <router-link :to="{ name: 'media', params: { parentId: item.id } }" v-localize="item.name"></router-link>
            <ui-icon class="-chevron" v-if="index < hierarchy.length - 1" symbol="fth-chevron-right" />
          </span>
        </h2>
      </template>

      <template v-if="selected.length < 1">
        <ui-search v-model="gridConfig.search" class="onbg" />
      </template>
      <media-selection v-else :selected="selected" @clear="clearSelection" @move="move" @remove="remove" />
    </ui-header-bar>

    <div class="ui-view-box">
      <div class="media-items" :class="{ 'is-selecting': selected.length > 0 }">
        <ui-datagrid ref="grid" v-model="gridConfig" @select="onSelected" @count="count = $event">
          <template v-if="selected.length < 1" v-slot:actions="props">
            <ui-dropdown-button v-if="props.item && props.item.isFolder" label="@ui.open.title" icon="fth-arrow-right" @click="goToFolder(props.item.id)" />
            <ui-dropdown-button label="@ui.edit.title" icon="fth-edit-2" @click="edit(props.item, props.item.isFolder)" />
            <ui-dropdown-button label="@ui.move.title" icon="fth-corner-down-right" @click="move([props.item])" />
            <ui-dropdown-button label="@ui.selection.select" icon="fth-check-circle-2" @click="$refs.grid.select(props.item)" />
            <ui-dropdown-separator />
            <ui-dropdown-button label="@ui.delete" icon="fth-trash" @click="remove(props.item, props.item.isFolder)" />
          </template>

          <template v-slot:default="props">
            <media-item :value="props.item" :selected="props.selected" />
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
  import actions from './overview-actions';

  export default defineComponent({
    props: ['parentId'],

    components: { MediaItem, MediaSelection },

    data: () => ({
      paging: {},
      hierarchy: {},
      search: null,
      selected: [],
      gridConfig: {
        search: null,
        width: 180,
        selectable: true,
        items: null
      },
    }),


    computed: {
      id()
      {
        return this.$route.params.parentId || 'root';
      }
    },


    watch: {
      '$route': function (val)
      {
        this.clearSelection();
      }
    },


    created()
    {
      this.gridConfig.items = this.getItems;
    },


    methods: {
      async getItems(query)
      {
        if (!query)
        {
          query = {};
        }

        query.search = this.gridConfig.search;
        query.folderId = this.parentId;
        query.searchIsGlobal = true;
        query.pageSize = 50;

        const hierarchy = await api.getHierarchy(this.id);
        this.hierarchy = hierarchy.data;

        return await api.getChildren(this.id, query);
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
        this.clearSelection();
        if (updates)
        {
          await this.$refs.grid.update();
        }
      },


      async remove(items: any[])
      {
        const deleted = await actions.remove(items);
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
    font-size: var(--font-size-l);
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
  }
</style>