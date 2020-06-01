<template>
  <div class="media">
    <div class="media-tree" v-resizable="resizable">
      <ui-tree ref="tree" :get="getTree" :config="treeConfig" :active="id" header="@media.list" />
      <div class="media-tree-resizable ui-resizable"></div>
    </div>

    <div class="media-content">
      <ui-header-bar :title="title" :back-button="!!id">
        <ui-search />
        <ui-button type="white" label="Add folder" icon="fth-plus" @click="addFolder(id)" />
      </ui-header-bar>

      <div class="ui-view-box">
        <div class="media-items">

          <!-- upload field -->
          <!--<div v-if="id" class="media-item is-blank">
            <input class="media-item-upload" type="file" multiple @change="onUpload" />
            <span class="media-item-content">
              <i class="fth-plus"></i>
            </span>
          </div>-->

          <!-- folder list -->
          <div v-if="!id" class="ui-datagrid-items">
            <router-link :to="getLink(item)" class="media-item is-folder" v-for="item in folders" :key="item.id">
              <span class="media-item-content is-folder">
                <i class="fth-folder"></i>
                <span>{{item.name}}</span>
              </span>
            </router-link>
          </div>

          <ui-datagrid v-if="id" ref="grid" v-model="gridConfig" />
        </div>
      </div>
    </div>
  </div>
</template>


<script>
  import MediaApi from 'zero/resources/media.js'
  import MediaFolderApi from 'zero/resources/media-folder.js'
  import Overlay from 'zero/services/overlay.js';
  import AddFolderOverlay from './folder';
  import MediaItemOverlay from './media-overlay-item';
  import MediaItem from './media-item';
  import UploadStatusOverlay from './upload-status';
  import Strings from 'zero/services/strings.js';
  import { each as _each, extend as _extend, debounce as _debounce, isArray as _isArray } from 'underscore';

  export default {

    data: () => ({
      cache: {},
      items: [],
      folders: [],
      treeConfig: {},
      current: null,
      icons: {
        image: 'fth-image',
        video: 'fth-video',
        file: 'fth-file'
      },
      resizable: {
        axis: 'x',
        min: 260,
        max: 520,
        save: 'media-tree',
        handle: '.ui-resizable'
      }
    }),

    created()
    {
      var instance = this;

      this.gridConfig = {
        search: null,
        width: 360,
        component: MediaItem,
        items: this.getItems
      };

      if (!this.id)
      {
        this.getItems();
      }

      this.treeConfig.onActionsRequested = item =>
      {
        let actions = [];

        if (item && item.id === 'recyclebin')
        {
          return [{
            name: 'Empty recycle bin',
            icon: 'fth-trash-2'
          }];
        }

        actions.push({
          name: 'Create',
          icon: 'fth-plus',
          action(action, dropdown)
          {
            dropdown.hide();
            instance.addFolder();
          }
        });

        if (item)
        {
          actions.push({
            name: 'Move',
            icon: 'fth-corner-down-right'
          });
        }

        if (item)
        {
          actions.push({
            type: 'separator'
          });
          actions.push({
            name: 'Delete',
            icon: 'fth-x'
          });
        }

        return actions;
      };
    },

    watch: {
      '$route'()
      {
        this.cache = {};

        if (!this.id)
        {
          this.getItems();
        }
        else
        {
          this.$refs.grid.load();
        }
        //this.getItems();
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

        return MediaApi.getAll(query).then(response =>
        {
          this.items = response.items;
          this.folders = response.folders;
          this.current = response.folder;

          return Promise.resolve(response);
        });
      },


      // get link for folder or media item
      getLink(item, isMediaItem)
      {
        if (item.id === 'recyclebin')
        {
          return { name: 'mediarecyclebin' };
        }

        return {
          name: 'mediafolder',
          params: { id: item.id }
        };
      },


      // load folders in tree
      getTree(parent)
      {
        const key = !parent ? '__root' : parent;

        if (this.cache[key])
        {
          return Promise.resolve(this.cache[key]);
        }

        return MediaFolderApi.getAllAsTree(parent, this.id).then(response =>
        {
          response.forEach(item =>
          {
            item.url = this.getLink(item);
          });
          this.cache[key] = response;
          return response;
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
          setTimeout(() =>
          {
            this.$refs.tree.refresh();
            this.$router.push({ name: 'mediafolder', params: { id: item.model.id } });
          }, 500);
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
          //this.onChange(value);
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
    display: grid;
    grid-template-columns: auto 1fr;
    grid-gap: 2px;
    justify-content: stretch;
  }

  .media-tree
  {
    width: 340px;
    background: var(--color-bg-light);
    padding: 0;
    position: relative;
    overflow-y: auto;
    height: 100vh;

    .ui-header-bar + .ui-tree
    {
      margin-top: -10px;
    }

    .ui-dot-button
    {
      margin-right: -8px;
    }
  }

  .media-tree-resizable
  {
    position: absolute;
    top: 0;
    bottom: 0;
    background: var(--color-fg);
    opacity: 0;
    right: 0;
    width: 6px;
    cursor: ew-resize;
    transition: opacity 0.15s ease 0s;

    &:hover
    {
      transition-delay: 0.2s;
      opacity: 0.04;
    }
  }

  .media-content
  {
    height: 100vh;
    overflow-y: auto;
  }

  .media-items .ui-datagrid-items
  {
    display: flex;
    flex-wrap: wrap;
    grid-gap: 15px;
    //grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
    align-items: stretch;
  }

  a.media-item, .media-item
  {
    display: inline-flex;
    flex: 1 0 auto;
    align-items: center;
    justify-content: center;
    background: var(--color-bg-light);
    height: 160px;
    border-radius: var(--radius);
    overflow: hidden;
    color: var(--color-fg);
    font-size: var(--font-size-xs);
    position: relative;

    img
    {
      width: 100%;
      height: 100%;
      object-fit: cover;
    }

    &.is-blank
    {
      border: 2px dotted var(--color-line);
      background: transparent;
    }

    &.is-folder, &.is-blank
    {
      width: 160px;
      flex: 0 1 auto;
    }
  }

  .media-item-content
  {
    display: grid;
    width: 100%;
    grid-template-rows: 1fr auto;
    text-align: center;
    height: 100%;
  
    /*&.is-folder, &.is-file
    {
      box-shadow: var(--color-shadow-short);
    }*/

    i
    {
      display: flex;
      width: 100%;
      align-items: center;
      justify-content: center;
      font-size: 28px;
      position: relative;

      &:after
      {
        font-family: var(--font);
        content: attr(data-extension);
        font-size: 12px;
        position: absolute;
        left: 50%;
        top: 50%;
        margin-top: 25px;
        background: var(--color-bg);
        display: inline-block;
        padding: 4px 8px;
        border-radius: 3px;
        font-weight: 600;
        transform: translateX(-50%);
      }
    }

    .is-blank & i:after,
    &.is-folder i:after
    {
      display: none;
    }

    span
    {
      display: block;
      /*border-top: 1px solid var(--color-line);*/
      padding: 10px 20px 12px;
    }

    &.is-folder span
    {
      font-weight: bold;
    }
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