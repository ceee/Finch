<template>
  <!--<ui-dropdown-button v-if="hasPreview" label="@page.preview.title" icon="fth-eye" :disabled="disabled || !urlLink" @click="openPreview" />
  <ui-dropdown-button v-if="hasLink" label="@ui.open.title" icon="fth-external-link" :disabled="disabled || !urlLink" @click="openLink" />-->

  <a v-if="hasPreview" :href="previewLink" target="preview" :disabled="disabled || !urlLink" type="button" class="ui-dropdown-button has-icon" @click.prevent="openPreview">
    <ui-icon symbol="fth-eye" class="ui-dropdown-button-icon" />
    <span class="-name"><ui-localize value="@page.preview.title" /></span>
  </a>
  <a v-if="hasLink" :href="urlLinkAbsolute" target="_blank" :disabled="disabled || !urlLink" type="button" class="ui-dropdown-button has-icon" @click="dropdown.hide()">
    <ui-icon symbol="fth-external-link" class="ui-dropdown-button-icon" />
    <span class="-name"><ui-localize value="@ui.open.title" /></span>
    <span class="-minor -link" v-if="urlLink && urlList.length > 1" :title="urlLink">{{urlList.length}} URLs</span>
  </a>
  <ui-dropdown-separator v-if="hasPreview || hasLink" />
</template>


<script>
  export default {
    name: 'uiFormHeaderLinks',

    props: {
      disabled: {
        type: Boolean,
        default: false
      },
      url: {
        type: Function,
        default: null
      },
      previewEnabled: {
        type: Boolean,
        default: false
      },
      value: {
        type: Object,
        default: null
      }
    },

    data: () => ({
      dropdown: null,
      urlList: [],
      urlDomain: null,
      urlPreview: null,
      failed: false
    }),

    watch: {
      'value.lastModifiedDate'()
      {
        this.loadUrls();
      }
    },

    mounted()
    {
      this.loadUrls();

      let current = this;
      do
      {
        if (current.$options.name === 'uiDropdown')
        {
          this.dropdown = current;
          break;
        }
      }
      while (current = current.$parent);

      if (!this.dropdown)
      {
        console.warn('ui-dropdown-button: Could not find parent <ui-dropdown />');
      }
    },

    computed: {
      hasPreview()
      {
        return !this.failed && this.value && typeof this.url === 'function' && this.previewEnabled;
      },
      hasLink()
      {
        return !this.failed && this.value && typeof this.url === 'function';
      },
      urlLink()
      {
        return this.urlDomain && this.urlList.length ? this.urlList.reduce((a, b) => a.length <= b.length ? a : b) : null;
      },
      urlLinkAbsolute()
      {
        return this.urlDomain && this.urlList.length ? this.urlDomain + this.urlList.reduce((a, b) => a.length <= b.length ? a : b) : null;
      },
      previewLink()
      {
        return this.urlLink ? this.$router.resolve({ name: 'preview', query: { path: this.urlLink } }).href : null;
      }
    },

    methods: {
      async loadUrls()
      {
        const result = await this.url(this.value);
        this.urlList = result.data.urls ? result.data.urls : (result.data.url ? [result.data.url] : []);
        this.urlDomain = result.data.domain;
        this.failed = this.urlList.length < 1 || !result.data.domain;
      },

      openPreview()
      {
        window.open(this.previewLink, 'preview');
        this.dropdown.hide();
      }
    }
  }
</script>


<style lang="scss">
  
</style>