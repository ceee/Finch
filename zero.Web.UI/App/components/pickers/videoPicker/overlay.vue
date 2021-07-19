<template>
  <ui-overlay-editor class="ui-videpicker-overlay">
    <template v-slot:header>
      <ui-header-bar title="@videopicker.headline" :back-button="false" :close-button="true" />
    </template>
    <template v-slot:footer>
      <ui-button type="light onbg" :label="config.closeLabel" :parent="config.rootId" @click="config.hide" />
      <ui-button type="primary" label="@ui.save" @click="onSave" :state="state" />
    </template>

    <div v-if="opened">
      <div class="ui-box ui-videpicker-overlay-options">
        <ui-property label="Provider" :vertical="true">
          <ui-select v-model="item.provider" :items="providers" />
        </ui-property>
        <ui-property label="Video URL" :vertical="true">
          <input v-model="item.videoUrl" type="text" class="ui-input" />
          <p v-if="item.videoId" class="ui-property-help">Found video ID is <b>{{item.videoId}}</b></p>
        </ui-property>
      </div>
      <div class="ui-box" v-if="preview">
        <div>
          <img :src="preview.image" />
          <p>
            <strong>{{preview.title}}</strong><br />
            {{preview.description}}
          </p>
        </div>
      </div>
    </div>
  </ui-overlay-editor>
</template>


<script>
  import PageTreeApi from 'zero/api/page-tree.js';
  import { getVideoId, getVimeoMetadata, getYoutubeMetadata } from 'zero/helpers/videoparser.js';
  import { debounce as _debounce } from 'underscore';

  const PROVIDERS = [
    { value: '@videopicker.providers.html', key: 'html' },
    { value: '@videopicker.providers.youtube', key: 'youtube' },
    { value: '@videopicker.providers.vimeo', key: 'vimeo' },
  ];

  export default {

    props: {
      model: Object,
      config: Object
    },

    data: () => ({
      opened: false,
      state: 'default',
      providers: PROVIDERS,
      item: null,
      videoIdParsing: false,
      template: {
        provider: 'youtube',
        videoId: null,
        videoUrl: null,
        videoPreviewImageUrl: null,
        title: null,
        previewImageId: null
      },
      preview: null,
      debouncedReloadPreview: null
    }),


    watch: {
      'item.videoUrl': function (val)
      {
        this.parseUrl(val);
      },
      'item.provider': function (val)
      {
        if (this.opened)
        {
          this.item.videoId = null;
          this.item.videoUrl = null;
          this.item.videoPreviewImageUrl = null;
          this.preview = null;
        }
      }
    },


    mounted()
    {
      this.debouncedReloadPreview = _debounce(this.reloadPreview, 300);
      this.item = JSON.parse(JSON.stringify(this.model || this.template));
      setTimeout(() =>
      {
        this.opened = true;
        if (this.model && this.model.provider !== 'html' && this.model.videoId)
        {
          this.reloadPreview(this.model.videoId);
        }
      }, 300);
    },


    methods: {

      parseUrl(url)
      {
        this.item.videoId = getVideoId(url);
        this.debouncedReloadPreview(this.item.videoId);
      },

      reloadPreview(id)
      {
        if (!id)
        {
          this.preview = null;
          return;
        }

        if (this.item.provider === 'vimeo')
        {
          getVimeoMetadata(id).then(res =>
          {
            this.preview = res.success ? res : null;
            this.handlePreview(this.preview);
          });
        }
        else
        {
          getYoutubeMetadata(id).then(res =>
          {
            this.preview = res.success ? res : null;
            this.handlePreview(this.preview);
          });
        }
      },

      handlePreview(preview)
      {
        if (!preview)
        {
          return;
        }

        this.item.title = this.preview.title;
        this.item.videoPreviewImageUrl = this.preview.image;
        //this.item.videoUrl = this.preview.url;
      },

      onSave()
      {
        this.config.confirm(this.item);
      }
    }
  }
</script>

<style lang="scss">
  .ui-videpicker-overlay content
  {
    padding-top: 0; 
  }

  .ui-videpicker-overlay-options .ui-property
  {
    display: flex;
    justify-content: space-between;
  }

  .ui-videpicker-overlay-options .ui-property + .ui-property
  {
    margin-top: var(--padding-m); 
  }

  .ui-videpicker-overlay-options .ui-property-content
  {
    display: inline;
    flex: 0 0 auto;
  }

  .ui-videpicker-overlay-options .ui-property-label
  {
    padding-top: 1px;
  }
</style>