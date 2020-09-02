<template>
  <div class="media">
    <router-view v-if="!isOverview"></router-view>

    <div v-if="isOverview" class="media-content">
      <ui-header-bar :back-button="!!id">
        <template v-slot:title>
          <h2 class="ui-header-bar-title">
            <router-link :to="{ name: 'media' }" class="media-items-hierarchy-item" v-if="!!id"><i class="fth-home"></i></router-link>
            <router-link :to="{ name: 'mediafolder', params: { id: item.id } }" v-for="(item, index) in hierarchy" :key="item.id" class="media-items-hierarchy-item" v-localize="item.name"></router-link>
          </h2>
        </template>
        <template>
          <ui-search />
          <ui-button type="white" label="Add folder" @click="addFolder(id)" />
          <div type="button" class="ui-button has-state type-action state-default has-icon">
            <span class="ui-button-text">Add</span>
            <input class="media-item-upload" type="file" multiple @change="onUpload" />
          </div>
        </template>
      </ui-header-bar>

      <div class="ui-view-box">
        <div class="media-items">
          <ui-datagrid ref="grid" v-model="gridConfig" />
        </div>
      </div>
    </div>
  </div>
</template>


<script>
  import MediaApi from 'zero/resources/media.js';
  import MediaFolderApi from 'zero/resources/media-folder.js';
  import Overlay from 'zero/services/overlay.js';
  import AddFolderOverlay from './folder';
  import MediaItemOverlay from './media-overlay-item';
  import MediaItem from './media-item';
  import UploadStatusOverlay from './upload-status';
  import { each as _each, extend as _extend, debounce as _debounce, isArray as _isArray } from 'underscore';

  export default {

    props: ['id'],

    data: () => ({
      current: null,
      hierarchy: []
    }),

    created()
    {
      var instance = this;

      this.gridConfig = {
        search: null,
        width: 280,
        component: MediaItem,
        items: this.getItems
      };
    },

    computed: {
      isOverview()
      {
        return this.$route.name !== 'mediaitem' && this.$route.name !== 'recyclebin';
      }
    },

    methods: {

      // get items (media + subfolders) in the current folder
      getItems(query)
      {
        if (!query)
        {
          query = {};
        }

        query.folderId = this.$route.params.id;

        this.getFolderHierarchy(query.folderId);

        return MediaApi.getListByQuery(query).then(response =>
        {
          return Promise.resolve(response);
        });
      },


      getFolderHierarchy(id)
      {
        if (!id)
        {
          this.current = {
            id: null,
            name: '@media.list'
          };
          this.hierarchy = [this.current];
          return;
        }

        MediaFolderApi.getHierarchy(id).then(res =>
        {
          this.hierarchy = res;
          this.current = res[res.length - 1];
        });
      },


      // navigate to a folder
      goToFolder(id)
      {
        this.$router.push({
          name: 'mediafolder',
          params: !id ? {} : { id: id }
        });
      },


      // adds a new folder
      addFolder(parentId)
      {
        Overlay.open({
          component: AddFolderOverlay,
          model: { parentId },
          theme: 'dark'
        }).then(item => this.goToFolder(item.model.id));
      },


      // adds a new file
      addFile(folderId)
      {
        let options = {
          title: '@iconpicker.title',
          closeLabel: '@ui.close',
          component: MediaItemOverlay,
          isCreate: true,
          model: {
            folderId: folderId
          },
          theme: 'dark',
          width: 520
        };

        return Overlay.open(options).then(value =>
        {
          console.info(value);
        }, () => { });
      },


      onUpload(event)
      {
        let options = {
          title: 'Upload status',
          closeLabel: '@ui.close',
          component: UploadStatusOverlay,
          isCreate: true,
          model: event.target.files,
          folderId: this.id,
          theme: 'dark',
          width: 520
        };

        return Overlay.open(options).then(value =>
        {
          console.info(value);
        }, () => { });
      },
    }
  }
</script>

<style lang="scss">
  .media
  {
    width: 100%;
    height: 100vh;
    background: var(--color-bg);
    overflow-y: auto;
  }

  .media-content
  {
    height: 100vh;
    overflow-y: auto;
  }

  .media-items .ui-datagrid-items
  {
    gap: 16px var(--padding);
  }

  input[type="file"].media-item-upload
  {
    position: absolute;
    height: 100%;
    top: 0;
    left: 0;
    width: 100%;
    z-index: 1;
    bottom: 0;
    opacity: 0.001;
    cursor: pointer;
  }

  .media-items-hierarchy-item
  {
    font-family: var(--font);
    margin: 0;
    font-size: var(--font-size-l);
    font-weight: 400;
    color: var(--color-fg-dim);

    &:last-child
    {
      font-weight: 700;
      color: var(--color-fg);
    }

    & + .media-items-hierarchy-item:before
    {
      content: '/';
      margin: 0 0.5em;
      color: var(--color-fg-dim);
      font-weight: 400;
    }

    &:hover
    {
      color: var(--color-fg);
    }
  }
</style>