<template>
  <div class="media">
    <router-view v-if="!isOverview"></router-view>

    <div v-if="isOverview" class="media-content">
      <ui-header-bar :title="title" :back-button="!!id">
        <ui-search />
        <ui-button type="white" label="Add folder" @click="addFolder(id)" />
        <div type="button" class="ui-button has-state type-action state-default has-icon">
          <span class="ui-button-text">Add</span>
          <input class="media-item-upload" type="file" multiple @change="onUpload" />
        </div>
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
  import MediaApi from 'zero/resources/media.js'
  import Overlay from 'zero/services/overlay.js';
  import AddFolderOverlay from './folder';
  import MediaItemOverlay from './media-overlay-item';
  import MediaItem from './media-item';
  import UploadStatusOverlay from './upload-status';
  import { each as _each, extend as _extend, debounce as _debounce, isArray as _isArray } from 'underscore';

  export default {

    data: () => ({
      items: [],
      current: null
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

      if (!this.id)
      {
        this.getItems();
      }
    },

    computed: {
      id()
      {
        return this.$route.params.id;
      },
      title()
      {
        return this.current ? this.current.name : '@media.list';
      },
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

        return MediaApi.getListByQuery(query).then(response =>
        {
          this.items = response.items;
          return Promise.resolve(response);
        });
      },


      // adds a new folder
      addFolder(parentId)
      {
        Overlay.open({
          component: AddFolderOverlay,
          model: { parentId },
          theme: 'dark'
        }).then(item =>
        {
          this.$router.push({ name: 'mediafolder', params: { id: item.model.id } });
        }, () => { });
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
</style>