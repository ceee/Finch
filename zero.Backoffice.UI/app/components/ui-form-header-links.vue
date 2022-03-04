<template>
  <ui-dropdown-button v-if="hasPreview" label="@page.preview.title" icon="fth-eye" :disabled="disabled" :prevent="true" @click="openPreview" />
  <ui-dropdown-button v-if="hasLink" label="@ui.open.title" icon="fth-external-link" :disabled="disabled" :prevent="true" @click="openLink" />
  <!--<a v-if="hasLink" :href="urlLinkAbsolute" target="_blank" :disabled="disabled || !urlLink" type="button" class="ui-dropdown-button has-icon is-multiline" @click="dropdown.hide()">
    <ui-icon symbol="fth-external-link" class="ui-dropdown-button-icon" />
    <span class="-name"><ui-localize value="@ui.open.title" /><span class="-minor" v-if="false && urlLink" :title="urlLink">{{urlLink}}</span></span>
  </a>-->
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
      urlPreview: null
    }),

    watch: {
      'value.lastModifiedDate'()
      {
        this.reloadUrls();
      }
    },

    mounted()
    {
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
        return this.value && typeof this.url === 'function' && this.previewEnabled;
      },
      hasLink()
      {
        return this.value && typeof this.url === 'function';
      },
      urlLink()
      {
        return this.urlDomain && this.urlList.length ? this.urlList[0] : null;
      },
      urlLinkAbsolute()
      {
        return this.urlDomain && this.urlList.length ? this.urlDomain + this.urlList[0] : null;
      }
    },

    methods: {
      async loadUrls()
      {
        const result = await this.url(this.value);
        this.urlList = result.data.urls ? result.data.urls : [result.data.url];
        this.urlDomain = result.data.domain;
      },

      async openPreview(_, opts)
      {
        opts.loading(true);
        await this.loadUrls();
        opts.loading(false);
        if (this.urlLink)
        {
          var resolved = this.$router.resolve({ name: 'preview', query: { path: this.urlLink } });
          window.open(window.location.origin + resolved.href, 'preview');
          opts.hide();
        }
      },

      async openLink(_, opts)
      {
        opts.loading(true);
        await this.loadUrls();
        opts.loading(false);
        if (this.urlLink)
        {
          window.open(this.urlLinkAbsolute, '_blank');
          opts.hide();
        }
      }
    }
  }
</script>


<style lang="scss">
  
</style>