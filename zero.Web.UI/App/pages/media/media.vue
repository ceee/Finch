<template>
  <div class="media">
    <div class="media-tree" v-resizable="resizable">
      <ui-header-bar title="Media" />
      <ui-tree :get="getTree" />
      <div class="media-tree-resizable ui-resizable"></div>
    </div>

    <div class="media-content">
      <ui-header-bar title="Media">
        <ui-search />
        <ui-button type="white" label="Add folder" icon="fth-plus" @click="addFolder($route.params.id)" />
        <ui-button label="Add media" icon="fth-plus" />
      </ui-header-bar>

      <div class="ui-view-box">
        <div class="media-items">
          <a href="#" class="media-item is-blank">
            <span class="media-item-content">
              <i class="fth-plus"></i>
            </span>
          </a>
          <a href="#" class="media-item" v-for="item in items">
            <img v-if="item.type === 'image'" :src="item.source" />
            <span class="media-item-content" v-if="item.type !== 'image'">
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
  import MediaFolderApi from 'zero/resources/media-folder.js'
  import Overlay from 'zero/services/overlay.js';
  import AddFolderOverlay from './folder';

  export default {

    props: ['id'],

    data: () => ({
      cache: {},
      items: [],
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
    }),

    created()
    {
      
    },

    watch: {
      '$route'()
      {
        
      }
    },

    methods: {

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
      }
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

  a.media-item
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

    .is-blank & i:after
    {
      display: none;
    }

    span
    {
      display: block;
      border-top: 1px solid var(--color-line);
      padding: 10px 20px 12px;
    }
  }
</style>