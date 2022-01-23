<template>
  <ui-trinity class="ui-videopicker-overlay">
    <template v-slot:header>
      <ui-header-bar title="@videopicker.headline" :back-button="false" :close-button="true" @close="config.close(true)" />
    </template>
    <template v-slot:footer>
      <ui-button type="light onbg" label="@ui.close" :parent="config.rootId" @click="config.close" />
      <ui-button type="primary" label="@ui.save" @click="onSave" :state="state" />
    </template>

    <div v-if="opened">
      <div class="ui-box ui-videopicker-overlay-options">
        <ui-property label="@videopicker.fields.provider" :vertical="true" :required="true">
          <ui-select v-model="item.provider" :items="providers" />
        </ui-property>
        <ui-property v-if="item.provider != 'html'" label="@videopicker.fields.videoUrl" :vertical="true">
          <input v-model="item.videoUrl" @input="onUrlChange($event.target.value)" type="text" class="ui-input" />
          <p v-if="item.videoId" class="ui-property-help" v-localize:html="{ key: '@videopicker.fields.foundParsed', tokens: { id: item.videoId } }"></p>
        </ui-property>
        <ui-property v-if="item.provider == 'html'" label="@videopicker.fields.videoId" :required="true">
          <ui-mediapicker v-model="item.videoId" />
        </ui-property>
        <ui-property label="@videopicker.fields.title">
          <input type="text" v-model="item.title" maxlength="60" />
        </ui-property>
        <ui-property v-if="item.provider == 'html'" label="@videopicker.fields.previewImageId">
          <ui-mediapicker v-model="item.previewImageId" />
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
  </ui-trinity>
</template>


<script>
  import { getVideoId, getVimeoMetadata, getYoutubeMetadata } from '../videoparser';
  import { debounce } from '../../../utils';

  const PROVIDERS = [
    { label: '@videopicker.providers.html', value: 'html' },
    { label: '@videopicker.providers.youtube', value: 'youtube' },
    { label: '@videopicker.providers.vimeo', value: 'vimeo' },
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
      'item.provider': function (val)
      {
        if (this.opened)
        {
          this.item.videoId = null;
          this.item.videoUrl = null;
          this.item.videoPreviewImageUrl = null;
          this.item.title = null;
          this.item.previewImageId = null;
          this.preview = null;
        }
      }
    },


    mounted()
    {
      this.debouncedReloadPreview = debounce(this.reloadPreview, 300);
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

      onUrlChange(url)
      {
        this.parseUrl(url);
      },

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
  .ui-videopicker-overlay content
  {
    padding-top: 0; 
  }

  .ui-videopicker-overlay-options .ui-property
  {
    display: flex;
    justify-content: space-between;
  }

  .ui-videopicker-overlay-options .ui-property + .ui-property
  {
    margin-top: var(--padding-m); 
  }

  .ui-videopicker-overlay-options .ui-property-content
  {
    display: inline;
    flex: 0 0 auto;
  }

  .ui-videopicker-overlay-options .ui-property-label
  {
    padding-top: 1px;
  }
</style>