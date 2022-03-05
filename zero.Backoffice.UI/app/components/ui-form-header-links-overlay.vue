<template>
  <ui-trinity class="ui-form-header-links-list">
    <template v-slot:header>
      <ui-header-bar title="@ui.links" :back-button="false" :close-button="true" @close="config.close(true)" />
    </template>
    <template v-slot:footer>
      <ui-button type="light onbg" label="@ui.close" @click="config.close"></ui-button>
    </template>

    <div class="ui-form-header-links-list-items">
      <a :href="urlPrefix + url" target="_blank" v-for="(url, index) in urls" class="ui-form-header-links-list-item">
        <span class="-text">{{url}}</span>
        <ui-icon symbol="fth-external-link" class="ui-form-header-links-list-item-icon" :size="16"></ui-icon>
      </a>
    </div>
  </ui-trinity>
</template>


<script>
  export default {

    props: {
      config: Object
    },

    data: () => ({
      urls: [],
      urlPrefix: ''
    }),


    mounted()
    {
      this.urlPrefix = this.config.model.urlPrefix || '';
      this.urls = this.config.model.urls;
    }
  }
</script>

<style lang="scss">
  .ui-form-header-links-list .ui-box
  {
    margin: 0;
  }

  .ui-form-header-links-list content
  {
    padding-top: 0;
  }

  .ui-form-header-links-list-item
  {
    display: grid;
    width: 100%;
    grid-template-columns: 1fr auto;
    gap: var(--padding-xs);
    align-items: center;
    font-size: var(--font-size);
    height: 60px;
    color: var(--color-text);
    position: relative;
    padding: 0 var(--padding-s);
    background: var(--color-box);
    border-radius: var(--radius);
    border: 2px solid transparent;

    .-text
    {
      display: block;
      padding: 12px 8px;
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
    }

    & + .ui-form-header-links-list-item
    {
      margin-top: 8px;
    }
  }

  .ui-form-header-links-list-text
  {
    margin: 0 0 20px;
  }

  .ui-form-header-links-list-item-icon
  {
    position: relative;
    top: -1px;
    color: var(--color-text);
  }
</style>