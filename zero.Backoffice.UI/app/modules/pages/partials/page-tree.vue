<template>
  <div ref="scrollable" class="app-tree page-tree" xv-resizable="resizable">
    <ui-tree ref="tree" :get="getItems" :config="treeConfig" @select="onSelect" :active="id" :header="'Pages'">
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
  import * as notifications from '../../../services/notification';
  import actions from '../actions';

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


    mounted()
    {
      this.zero.events.off('page.update');
      this.zero.events.on('page.update', props =>
      {
        this.$nextTick(() =>
        {
          this.cache = [];
          this.$refs.tree.refresh();
        });
      });
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
        if (item.id === 'recyclebin')
        {
          item.hasActions = false;
          item.disabled = true;
          //item.url = {
          //  name: 'recyclebin'
          //};
        }
        else
        {
          item.url = {
            name: 'pages-edit',
            params: { id: item.id }
          };
        }

        return item;
      },


      onSelect(item)
      {
        if (item.id == 'recyclebin')
        {
          notifications.error('Not implemented', 'Recycle bin has not been implemented yet');
        }
      },


      async create(item)
      {
        await actions.create(this.$router, item);
      },


      async move(item)
      {
        await actions.move(item);
      },


      async copy(item)
      {
        await actions.copy(this.$router, item);
      },


      async sort(item)
      {
        await actions.sort(item);
      },


      async remove(item)
      {
        await actions.remove(item);
      }

      //watch: {
    //  '$route'()
    //  {
    //    var $ela = document.querySelector('.app-tree .ui-tree-item.is-active');
    //    var centerOffset = this.$el.offsetHeight * 0.5 - $ela.offsetHeight * 0.5;

    //    this.$nextTick(() =>
    //    {
    //      this.$el.scrollTo({ top: $ela.offsetTop, behavior: 'smooth' });
    //    });

    //    console.info({
    //      elOffsetHeight: this.$el.offsetHeight,
    //      aOffsetHeight: $ela.offsetHeight,
    //      aOffsetTop: $ela.offsetTop, 
    //      centerOffset,
    //      scrollTop: $ela.offsetTop - centerOffset
    //    });
    //  }
    //},

      //onActiveSet(val)
      //{
      //  this.$nextTick(() =>
      //  {
      //    let container = this.$refs.scrollable;
      //    let child = val.$el;
      //    let centerOffset = container.offsetHeight * 0.5 - child.offsetHeight * 0.5;
      //    let threshold = container.offsetHeight * 0.1;

      //    let activeRange = {
      //      from: container.scrollTop + threshold,
      //      to: container.scrollTop + container.offsetHeight - threshold
      //    };

      //    let rect = child.getBoundingClientRect();
      //    let scrollTop = rect.top + container.offsetTop;
      //    let scrollTo = child.offsetTop;

      //    //console.info(child.offsetTop + ' [ ' + activeRange.from + ' - ' + activeRange.to + ' ]', rect);

      //    //if (activeRange.from <= scrollTo && activeRange.to >= scrollTo)
      //    //{
      //    //  console.info('in range');
      //    //  return;
      //    //}

      //    //console.info('scroll to: ' + scrollTo);
      //    container.scrollTo({ top: scrollTo, behavior: 'smooth' });
      //  });

      //},

    }

  });
</script>

<style lang="scss">

</style>