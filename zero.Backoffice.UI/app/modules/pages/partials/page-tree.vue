<template>
  <div ref="scrollable" class="app-tree page-tree" xv-resizable="resizable">
    <ui-tree ref="tree" :get="getItems" :config="treeConfig" :active="id" :header="'Pages'">
      <template v-slot:actions="props">
        <template v-if="!props.item || props.id !== 'recyclebin'">
          <ui-dropdown-button label="@ui.create" icon="fth-plus" @click="create(props.item)" />
          <ui-dropdown-button v-if="props.item" label="@ui.move.title" icon="fth-corner-down-right" @click="move(props.item)" />
          <ui-dropdown-button v-if="props.item" label="@ui.copy.title" icon="fth-copy" @click="copy(props.item)" />
          <ui-dropdown-button label="@ui.sort.title" icon="fth-arrow-down" @click="sort(props.item)" />
          <ui-dropdown-separator v-if="props.item" />
          <ui-dropdown-button v-if="props.item" label="@ui.delete" icon="fth-trash" @click="remove(props.item)" />
        </template>
      </template>
    </ui-tree>
    <div class="app-tree-resizable ui-resizable"></div>
  </div>
</template>

<script lang="ts">
  import api from '../api';
  import { defineComponent } from 'vue';
  import { useUiStore } from '../../../ui/store';

  export default defineComponent({

    props: {
      id: {
        type: String,
        default: null
      }
    },

    data: () => ({
      flavors: [],
      cache: {},
      resizable: {
        axis: 'x',
        min: 260,
        max: 520,
        save: 'page-tree',
        handle: '.ui-resizable'
      },
      actions: [],
      treeConfig: {

      }
    }),


    created()
    {
      const ui = useUiStore();
      this.flavors = ui.flavors.pages.flavors;
    },


    methods: {

      async getItems(parent)
      {
        const key = !parent ? '__root' : parent;

        if (this.cache[key])
        {
          return this.cache[key];
        }

        const response = await api.tree.getChildren(parent || 'root', this.id);
        const items = response.data.map(x => this.buildItem(x));

        this.cache[key] = items;
        return items;
      },


      buildItem(item)
      {
        item.url =  {
          name: 'pages-edit',
          params: { id: item.id }
        };

        return item;
      }

    }

  });
</script>

<style lang="scss">

</style>