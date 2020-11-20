<template>
  <div class="media-content">
    <ui-header-bar :back-button="!!id" :count="count">
      <template v-slot:title>
        <h2 class="ui-header-bar-title">
          <router-link :to="{ name: 'media' }" class="media-items-hierarchy-item" v-if="!!id"><i class="fth-home"></i></router-link>
          <router-link :to="{ name: 'media', params: { id: item.id } }" v-for="(item, index) in hierarchy" :key="item.id" class="media-items-hierarchy-item" v-localize="item.name"></router-link>
        </h2>
      </template>
      <template>
        <ui-search v-if="!selecting" v-model="gridConfig.search" class="onbg" />
        <ui-dropdown v-if="!!id && !selecting" align="right" :disabled="!!gridConfig.search">
          <template v-slot:button>
            <ui-button type="light onbg" label="Folder" caret="down" :disabled="!!gridConfig.search" />
          </template>
          <ui-dropdown-button label="@ui.edit.title" icon="fth-edit-2" @click="edit(current, true)" />
          <ui-dropdown-button label="@ui.move.title" icon="fth-corner-down-right" @click="move(current, true)" />
          <ui-dropdown-separator />
          <ui-dropdown-button label="@ui.delete" icon="fth-trash" @click="remove(current, true)" />
        </ui-dropdown>
        <ui-dropdown v-if="selecting" align="right">
          <template v-slot:button>
            <ui-button type="light onbg" :label="selectedText" caret="down" />
          </template>
          <!--<slot name="actions"></slot>-->
        </ui-dropdown>
        <ui-button v-if="!selecting" type="primary" label="Add folder" @click="addFolder(id)" />
        <div v-if="!!id && !selecting" type="button" class="ui-button has-state type-primary state-default has-icon">
          <span class="ui-button-text" v-localize="'Add file'"></span>
          <input class="media-item-upload" type="file" multiple @change="onUpload" />
        </div>
      </template>
    </ui-header-bar>

    <div class="ui-view-box" v-if="scope">
      <div class="media-items">
        <ui-datagrid ref="grid" v-model="gridConfig" @select="onSelected" @count="count = $event">
          <template v-slot:actions="props">
            <ui-dropdown-button v-if="props.item && props.item.isFolder" label="@ui.open.title" icon="fth-arrow-right" @click="goToFolder(props.item.id)" />
            <ui-dropdown-button label="@ui.edit.title" icon="fth-edit-2" @click="edit(props.item, props.item.isFolder)" />
            <ui-dropdown-button label="@ui.move.title" icon="fth-corner-down-right" @click="move(props.item, props.item.isFolder)" />
            <ui-dropdown-button label="Select" icon="fth-check-circle" @click="$refs.grid.select(props.item)" />
            <ui-dropdown-separator />
            <ui-dropdown-button label="@ui.delete" icon="fth-trash" @click="remove(props.item, props.item.isFolder)" />
          </template>
        </ui-datagrid>
      </div>
    </div>
  </div>
</template>


<script>
  import MediaApi from 'zero/api/media.js';
  import MediaFolderApi from 'zero/api/media-folder.js';
  import Overlay from 'zero/helpers/overlay.js';
  import FolderOverlay from './overlays/folder.vue';
  import MoveOverlay from './overlays/move.vue';
  import CreateItemOverlay from './overlays/create.vue';
  import MediaItem from './item.vue';
  import UploadStatusOverlay from './overlays/upload-status.vue';
  import EventHub from 'zero/helpers/eventhub.js';
  import Notification from 'zero/helpers/notification.js';
  import { each as _each, extend as _extend, debounce as _debounce, isArray as _isArray } from 'underscore';

  export default {

    props: ['id', 'scope'],

    data: () => ({
      count: 0,
      current: null,
      hierarchy: [],
      gridConfig: {
        search: null,
        width: 280,
        component: MediaItem,
        selectable: true
      },
      selectedCount: 0
    }),

    computed: {
      selectedText()
      {
        return this.selectedCount + ' selected';
      },
      selecting()
      {
        return this.selectedCount > 0;
      },
      shared()
      {
        return this.scope === 'shared'
      }
    },

    watch: {
      search(value)
      {
        this.$refs.grid.debouncedUpdate();
      },
      '$route': function (val)
      {
        this.initialize();
      }
    },

    created()
    {
      this.gridConfig.items = this.getItems;
      this.initialize();
    },

    methods: {

      initialize()
      {
        if (!this.scope)
        {
          this.$route.params.scope = 'local';
          this.$router.replace(this.$route);
        }
      },

      // get items (media + subfolders) in the current folder
      getItems(query)
      {
        if (!query)
        {
          query = {};
        }

        query.search = this.gridConfig.search;
        query.folderId = this.$route.params.id;
        query.searchIsGlobal = true;
        query.isShared = this.shared;

        this.getFolderHierarchy(query.folderId, !!query.search);

        return MediaApi.getListByQuery(query).then(response => // .scope(this.shared)
        {
          return Promise.resolve(response);
        });
      },


      getFolderHierarchy(id, isSearch)
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
        if (isSearch)
        {
          this.current = {
            id: null,
            name: 'Search results' // TODO translate
          };
          this.hierarchy = [this.current];
          return;
        }

        MediaFolderApi.getHierarchy(id, this.shared).then(res =>
        {
          this.hierarchy = res;
          this.current = res[res.length - 1];
        });
      },


      // navigate to a folder
      goToFolder(id)
      {
        this.$router.push({
          name: 'media',
          params: !id ? {} : { id: id }
        });
      },


      // adds a new folder
      addFolder(parentId)
      {
        Overlay.open({
          component: FolderOverlay,
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
          component: CreateItemOverlay,
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


      onSelected(items)
      {
        this.selectedCount = items.length;
      },


      edit(item, isFolder)
      {
        if (!isFolder)
        {
          return this.$router.push({
            name: 'media-edit',
            params: { id: item.id }
          });
        }

        Overlay.open({
          component: FolderOverlay,
          model: item,
          theme: 'dark'
        }).then(res =>
        {
          if (res.deleted === true)
          {
            return this.remove(item, true);
          }
          else
          {
            EventHub.$emit('media.update');
            this.$refs.grid.update();
          }
        });
      },


      move(item, isFolder)
      {
        return Overlay.open({
          component: MoveOverlay,
          display: 'editor',
          model: item,
          isFolder: isFolder
        }).then(value =>
        {
          EventHub.$emit('page.move', value);
          EventHub.$emit('page.update');
          this.$refs.grid.update();
        });
      },


      remove(item, isFolder)
      {
        Overlay.confirmDelete(item.name, isFolder ? '@media.deleteoverlay.folder_text' : '@deleteoverlay.text').then(opts =>
        {
          opts.state('loading');

          (isFolder ? MediaFolderApi : MediaApi).delete(item.id).then(response =>
          {
            if (response.success)
            {
              opts.state('success');
              opts.hide();

              EventHub.$emit('media.delete', response.model);
              EventHub.$emit('media.update');

              Notification.success('@deleteoverlay.success', isFolder ? '@media.deleteoverlay.folder_success_text' : '@deleteoverlay.success_text');
              this.$refs.grid.update();

              if (isFolder && item.id === this.id)
              {
                this.$router.go(-1);
              }
              else
              {
                this.$refs.grid.update();
              }
            }
            else
            {
              opts.errors(response.errors);
            }
          });
        });
      }
    }
  }
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

  .media-items .ui-datagrid-items
  {
    gap: var(--padding);
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
    color: var(--color-text-dim);
    &:last-child

  {
    font-weight: 700;
    color: var(--color-text);
  }

  & + .media-items-hierarchy-item:before
  {
    content: '/';
    margin: 0 0.5em;
    color: var(--color-text-dim);
    font-weight: 400;
  }

  &:hover
  {
    color: var(--color-text);
  }
  }
</style>