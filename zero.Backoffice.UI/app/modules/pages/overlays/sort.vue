<template>
  <ui-trinity class="shop-catalogues-sort">
    <template v-slot:header>
      <ui-header-bar title="@ui.sort.title" :back-button="false" :close-button="true" @close="config.close(true)" />
    </template>
    <template v-slot:footer>
      <ui-button type="light onbg" label="@ui.close" @click="config.close"></ui-button>
      <ui-button type="accent" label="@ui.save" @click="onSave" :state="state"></ui-button>
    </template>

    <p class="shop-catalogues-sort-text" v-localize="'@ui.sort.text'"></p>
    <div class="shop-catalogues-sort-items" v-sortable="{ onUpdate: onSortingUpdated }">
      <div v-for="(item, index) in items" :key="item.id" class="shop-catalogues-sort-item">
        <span class="-content">
          <span>{{index + 1}}.</span>
          <ui-icon :symbol="item.icon" class="shop-catalogues-sort-item-icon" :size="16"></ui-icon>
          <span>{{item.name}}</span>
        </span>
        <button type="button" class="shop-catalogues-sort-item-button is-handle">
          <ui-icon class="-minor" symbol="fth-grip-vertical" :size="14" /> 
        </button>
      </div>
    </div>
  </ui-trinity>
</template>


<script>
  import api from '../api';
  import * as notifications from '../../../services/notification';
  import { arrayMove } from '../../../utils';

  export default {

    props: {
      config: Object
    },

    data: () => ({
      items: [],
      selected: [],
      state: 'default'
    }),


    computed: {
      parentId()
      {
        return this.config.model ? this.config.model.parentId : null;
      }
    },


    mounted()
    {
      return api.tree.getChildren(this.parentId || 'root').then(response =>
      {
        this.items = response.data.filter(x => x.id && x.id !== 'recyclebin');
      });
    },


    methods: {

      async onSave()
      {
        this.state = 'loading';

        const result = await api.sort(this.items.map(x => x.id));

        if (result.success)
        {
          this.state = 'success';
          this.config.confirm(result);
        }
        else
        {
          this.state = 'error';
          notifications.error('@errors.onsort.title', result.errors[0].message);
        }
      },


      onSortingUpdated(ev)
      {
        this.items = arrayMove(this.items, ev.oldIndex, ev.newIndex);
      },
    }
  }
</script>

<style lang="scss">
  .shop-catalogues-sort .ui-box
  {
    margin: 0;
  }
  .shop-catalogues-sort content
  {
    padding-top: 0;
  }

  .shop-catalogues-sort-item
  {
    display: grid;
    width: 100%;
    grid-template-columns: 1fr auto;
    gap: 6px;
    align-items: center;
    font-size: var(--font-size);
    height: 60px;
    color: var(--color-text);
    position: relative;
    padding: 0 8px;
    background: var(--color-box);
    border-radius: var(--radius);
    border: 2px solid transparent;
    cursor: move;

    i
    {
      font-size: var(--font-size-l);
      position: relative;
      top: -1px;
      color: var(--color-text-dim);
    }

    .-content
    {
      display: flex;
      flex-direction: row;
      align-items: center;
      gap: 10px;
      padding: 12px 8px;
    }

    &.is-selected
    {
      color: var(--color-text-dim);
    }

    & + .shop-catalogues-sort-item
    {
      margin-top: 8px;
    }

    &.sortable-chosen
    {
      box-shadow: var(--shadow-short);
      border: 2px solid var(--color-accent); 

      .-content
      {
        font-weight: 700;
      }
    }
  }

  button.shop-catalogues-sort-item-button
  {
    height: 48px;
    width: 24px;
    display: flex;
    justify-content: center;
    align-items: center;
    text-align: center;
    pointer-events: none;
  }

  .shop-catalogues-sort-text
  {
    margin: 0 0 20px;
  }

  .shop-catalogues-sort-item-icon
  {
    position: relative;
    top: -1px;
    color: var(--color-text);
  }
</style>