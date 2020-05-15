<template>
  <ui-form ref="form" class="ui-mediapicker-overlay" v-slot="form">
    <h2 class="ui-headline">Select or upload media</h2>

    <ui-search class="ui-mediapicker-overlay-search" v-model="query" />

    <nav class="ui-mediapicker-overlay-hierarchy">
      <button type="button" v-for="item in hierarchy" class="ui-mediapicker-overlay-hierarchy-item" @click="selectFolder(item.id)">{{item.name}}</button>
      <button type="button" class="ui-mediapicker-overlay-hierarchy-item" @click="addFolder"><i class="fth-plus"></i></button>
    </nav>

    <div class="ui-mediapicker-overlay-items">
      <button type="button" class="ui-mediapicker-overlay-item is-upload" @click="upload">
        <span class="-preview"><i class="fth-plus"></i></span>
        <p class="ui-mediapicker-overlay-item-text">
          Upload media
        </p>
      </button>

      <button type="button" v-for="item in items" class="ui-mediapicker-overlay-item" @click="select(item)">
        <img class="-preview" v-if="item.type === 'image'" :src="item.source" />
        <span class="-preview" v-if="item.type !== 'image'"><i :class="item.type !== 'folder' ? 'fth-file' : 'fth-folder'"></i></span>
        <p class="ui-mediapicker-overlay-item-text">
          {{item.name}}
          <span class="-minor" v-if="item.size"><br>{{item.size}}</span>
        </p>
      </button>
    </div>

    <!--<div class="app-confirm-buttons">
      <ui-button v-if="!disabled" type="light" :submit="true" :state="form.state" label="@ui.save"></ui-button>
      <ui-button type="light" :label="config.closeLabel" :disabled="loading" @click="config.close"></ui-button>
    </div>-->
  </ui-form>
</template>


<script>
  import { debounce as _debounce, filter as _filter } from 'underscore';

  export default {

    props: {
      model: String,
      config: Object
    },

    data: () => ({
      icon: null,
      query: '',
      items: [],
      hierarchy: [
        { id: 0, name: 'Media' },
        { id: 1, name: 'News' },
        { id: 2, name: '2019' }
      ]
    }),

    watch: {
      model()
      {
        //this.init();
      },
      query()
      {
        this.debouncedSearch();
      }
    },

    created()
    {
      this.debouncedSearch = _debounce(this.search, 100);
      //this.items = this.config.items;
      //this.init();

      const sources = [
        'http://nolbert.com/wp-content/uploads/2018/04/nolbert_logitech_thumb_s.jpg',
        'http://nolbert.com/wp-content/uploads/2019/06/nolbert_orange_stack_01_thumb_sn.jpg',
        'http://nolbert.com/wp-content/uploads/2019/04/nolbert_vyvyd_thumb_violet_n.jpg',
        'http://nolbert.com/wp-content/uploads/2018/08/nolbert_oppo_r15_thumb_s.jpg'
      ];

      this.items.push({
        name: 'Unfinished',
        type: 'folder'
      });

      sources.forEach(source =>
      {
        this.items.push({
          source: source,
          name: source.split('/')[7],
          size: '100 kB',
          type: 'image'
        });
      });

      this.items.push({
        source: 'zeromagic-loop.webm',
        name: 'zeromagic-loop-zeromagic-loop-zeromagic-loop-zeromagic-loop-zeromagic-loop.webm',
        size: '2.3 MB',
        type: 'video',
        extension: '.webm'
      });

      this.items.push({
        source: 'documentation.docx',
        name: 'documentation.docx',
        size: '723 kB',
        type: 'file',
        extension: '.docx'
      });
    },

    mounted()
    {
      
    },

    methods: {

      // switches view to the selected folder
      selectFolder(id)
      {

      },

      // adds a new folder within the current parent
      addFolder()
      {

      },

      // uploads a new media item within the current parent
      upload()
      {

      },

      // select an item, this can either be a folder or a media item
      select(item)
      {
        if (item.type === 'folder')
        {
          return this.selectFolder(item.id);
        }
      }
    }
  }
</script>

<style lang="scss">
  .ui-mediapicker-overlay
  {
    text-align: left;
  }

  .ui-mediapicker-overlay-search.ui-searchinput .ui-input
  {
    background: var(--color-bg-mid);
    margin-top: 25px;
  }

  .ui-mediapicker-overlay-hierarchy
  {
    text-align: left;
    margin-top: 20px;
  }

  .ui-mediapicker-overlay-hierarchy-item
  {
    line-height: 1.4;

    & + .ui-mediapicker-overlay-hierarchy-item:before
    {
      content: '/';
      margin: 0 0.5em;
      color: var(--color-fg-light);
    }
  }

  .ui-mediapicker-overlay-items
  {
    margin-top: 25px;
    max-height: 495px;
    overflow-y: auto;
  }

  .ui-mediapicker-overlay-item
  {
    width: 100%;
    height: 70px;
    display: grid;
    grid-template-columns: 70px 1fr;
    grid-gap: 15px;
    align-items: center;
    line-height: 1.4;

    & + .ui-mediapicker-overlay-item
    {
      margin-top: 15px;
    }

    .-preview
    {
      display: flex;
      align-items: center;
      justify-content: center;
      flex-direction: column;
      height: 70px;
      width: 70px;
      object-fit: cover;
      background: var(--color-bg-mid);
      border-radius: var(--radius);
      position: relative;
      text-align: center;
      font-size: 22px;
    }

    /*&.is-upload .-preview
    {
      background: var(--color-primary);
      color: var(--color-primary-fg);
    }*/

    .-extension
    {
      font-size: 12px;
      font-style: normal;
      margin-top: 8px;
    }

    .-minor
    {
      color: var(--color-fg-light);
      font-size: var(--font-size-s);
    }
  }

  .ui-mediapicker-overlay-item-text
  {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
    margin: 0;
  }
</style>