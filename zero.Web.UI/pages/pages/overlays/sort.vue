<template>
  <ui-overlay-editor class="pages-sort">
    <template v-slot:header>
      <ui-header-bar title="@ui.sort.title" :back-button="false" :close-button="true" />
    </template>
    <template v-slot:footer>
      <ui-button type="light onbg" label="@ui.close" @click="config.hide"></ui-button>
      <ui-button type="primary" label="@ui.save" @click="onSave" :state="state"></ui-button>
    </template>

    <p class="pages-sort-text" v-localize="'@ui.sort.text'"></p>
    <div class="pages-sort-items" v-sortable="{ handle: '.is-handle', onUpdate: onSortingUpdated }">
      <div v-for="(item, index) in items" :key="item.id" class="pages-sort-item">
        <span>{{index + 1}}. &nbsp; <i class="pages-sort-item-icon" :class="item.icon"></i> {{item.name}}</span>
        <button type="button" class="pages-sort-item-button is-handle">
          <i class="-minor fth-more-vertical"></i>
        </button>
      </div>
    </div>
  </ui-overlay-editor>
</template>


<script>
  import PageTreeApi from '@zero/resources/page-tree.js'
  import PagesApi from '@zero/resources/pages.js';
  import Arrays from '@zero/services/arrays.js'
  import Notification from '@zero/services/notification.js'

  export default {

    props: {
      model: Object,
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
        return this.model ? this.model.parentId : null;
      }
    },


    mounted()
    {
      return PageTreeApi.getChildren(this.parentId).then(response =>
      {
        this.items = response.filter(x => x.id !== 'recyclebin');
      });
    },


    methods: {

      onSave()
      {
        this.state = 'loading';

        PagesApi.saveSorting(this.items.map(x => x.id)).then(res =>
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
      },


      onSortingUpdated(ev)
      {
        this.items = Arrays.move(this.items, ev.oldIndex, ev.newIndex);
      },
    }
  }
</script>

<style lang="scss">
  .pages-sort .ui-box
  {
    margin: 0;
  }
  .pages-sort content
  {
    padding-top: 0;
  }

  .pages-sort-item
  {
    display: grid;
    width: 100%;
    grid-template-columns: 1fr auto;
    gap: 6px;
    align-items: center;
    font-size: var(--font-size);
    height: 46px;
    color: var(--color-text);
    position: relative;
    padding: 0 8px;
    background: var(--color-box);
    border-radius: var(--radius);

    i
    {
      font-size: var(--font-size-l);
      position: relative;
      top: -1px;
      color: var(--color-text-dim);
    }

    span
    {
      padding: 12px 8px;
    }

    &.is-selected
    {
      color: var(--color-text-dim);
    }

    & + .pages-sort-item
    {
      margin-top: 8px;
    }
  }

  button.pages-sort-item-button
  {
    height: 42px;
    width: 24px;
    display: flex;
    justify-content: center;
    align-items: center;
    text-align: center;

    i
    {
      font-size: var(--font-size);
    }

    &.is-handle
    {
      cursor: move;
    }
  }

  .pages-sort-text
  {
    margin: 0 0 20px;
  }

  i.pages-sort-item-icon
  {
    top: 0;
    color: var(--color-text);
    margin-right: 8px;
  }
</style>