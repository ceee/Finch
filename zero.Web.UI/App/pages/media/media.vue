<template>
  <div class="media">
    <div class="media-tree" v-resizable="resizable">
      <ui-header-bar title="@media.list" />
      <ui-tree :get="getTree" />
      <div class="media-tree-resizable ui-resizable"></div>
    </div>

    <div class="media-content">
      <ui-header-bar :title="title">
        <ui-search />
        <ui-button type="white" label="Add folder" icon="fth-plus" @click="addFolder(id)" />
        <!--<ui-button label="Add media" icon="fth-plus" />-->
      </ui-header-bar>

      <div class="ui-view-box">
        <div class="media-items">

          <!-- upload field -->
          <div class="media-item is-blank">
            <input class="media-item-upload" type="file" multiple @change="onUpload" />
            <span class="media-item-content">
              <i class="fth-plus"></i>
            </span>
          </div>

          <!-- folder list -->
          <a href="#" class="media-item" v-for="item in folders">
            <span class="media-item-content is-folder">
              <i class="fth-folder"></i>
              <span>{{item.name}}</span>
            </span>
          </a>

          <!-- media list -->
          <a href="#" class="media-item" v-for="item in items">
            <img v-if="item.type === 'image'" :src="item.source" />
            <span class="media-item-content is-file" v-if="item.type !== 'image'">
              <i :class="icons[item.type]" :data-extension="item.extension"></i>
              <span>{{item.source}}</span>
            </span>
          </a>
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
  import MediaItemOverlay from './media-item';
  import UploadStatusOverlay from './upload-status';
  import Strings from 'zero/services/strings.js';
  import { each as _each, extend as _extend, debounce as _debounce, isArray as _isArray } from 'underscore';

  export default {

    data: () => ({
      cache: {},
      items: [],
      folders: [],
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
      },
      filter: {
        orderBy: 'createdDate',
        orderIsDescending: true,
        page: 1,
        pageSize: 20,
        search: null,
        folderId: null
      },
    }),

    created()
    {
      this.getItems();
    },

    watch: {
      '$route'()
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
        return this.id ? 'Media (...)' : '@media.list';
      }
    },

    methods: {

      // get items (media + subfolders) in the current folder
      getItems()
      {
        this.filter.folderId = this.$route.params.id;

        MediaApi.getAll(this.filter).then(response =>
        {
          this.items = response.items;
          this.folders = response.folders;
        });
      },

      // load folders in tree
      getTree(parent)
      {
        const key = !parent ? '__root' : parent;

        if (this.cache[key])
        {
          return Promise.resolve(this.cache[key]);
        }

        return MediaFolderApi.getAllAsTree(parent).then(response =>
        {
          response.forEach(item =>
          {
            item.url = {
              name: 'mediafolder',
              params: { id: item.id }
            };

            if (item.id === "recyclebin")
            {
              item.url = {
                name: 'mediarecyclebin'
              };
            }
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
          width: 700,
          model: { parentId }
        }).then(item =>
        {
          console.info(item);
          // TODO reload?
        }, () =>
        {
          
        });
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
        const files = event.target.files;
        let items = [];

        if (files && files.length > 0)
        {
          for (var i = 0; i < files.length; i++)
          {
            let file = files[i];

            items.push({
              id: 'upload:' + Strings.guid(),
              name: file.name,
              size: file.size,
              mimeType: file.type,
              source: null,
              file: file
            });
            //items.push(this.addFile(files[i]));
          }
        }

        console.info(items);
        //this.update();
      },


      //addFile(file)
      //{
      //  var source = URL.createObjectURL(file);
      //  var media = {
      //    id: 'upload:' + Strings.guid(),
      //    name: file.name,
      //    source: source,
      //    size: file.size,
      //    mimeType: file.type
      //  };

      //  var reader = new FileReader();
      //  reader.onload = function (e)
      //  {
      //    media.source = e.target.result;
      //  };
      //  reader.readAsDataURL(file);

      //  return media;
      //},
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

  .media-items
  {
    display: grid;
    grid-gap: var(--padding);
    grid-template-columns: repeat(auto-fill, minmax(180px, 1fr));
    align-items: stretch;
  }

  a.media-item, .media-item
  {
    display: inline-flex;
    align-items: center;
    justify-content: center;
    background: var(--color-bg-light);
    height: 210px;
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