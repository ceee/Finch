<template>
  <a :href="_path" class="ui-filelink" download>
    <ui-icon :symbol="_icon" class="ui-filelink-icon" :size="22" />
    <p class="ui-filelink-content">
      <span class="ui-filelink-name">{{_name}}</span>
      <span class="ui-filelink-path">{{_visiblePath}}</span>
    </p>
  </a>
</template>

<script>
  export default {
    props: {
      value: String,
      config: Object,

      name: {
        type: [String, Function],
        default: null
      },
      extension: {
        type: [String, Function],
        default: null
      },
      icon: {
        type: [String, Function],
        default: null
      },
      path: {
        type: [String, Function],
        default: null
      },
      visiblePath: {
        type: [String, Function],
        default: null
      },

      html: {
        type: Boolean,
        default: false
      },
      render: Function
    },

    computed: {
      _path()
      {
        return !this.path ? this.value : (typeof this.path === 'function' ? this.path(this.value, this.config.model) : this.path);
      },
      _visiblePath()
      {
        return !this.visiblePath ? this._path : (typeof this.visiblePath === 'function' ? this.visiblePath(this.value, this.config.model) : this.visiblePath);
      },
      _name()
      {
        return !this.name ? '@filelink.defaultName' : (typeof this.name === 'function' ? this.name(this.value, this.config.model) : this.name);
      },
      _extension()
      {
        return !this.extension ? this._visiblePath.split('.').at(-1) : (typeof this.extension === 'function' ? this.extension(this.value, this.config.model) : this.extension);
      },
      _icon()
      {
        return !this.icon ? 'fth-download' : (typeof this.icon === 'function' ? this.icon(this.value, this.config.model) : this.icon);
      },
    }
  }
</script>

<style lang="scss">
  .ui-filelink
  {
    border-radius: var(--radius-inner);
    background: var(--color-button-light);
    padding: 16px 26px 16px 20px;
    display: inline-grid;
    grid-template-columns: 36px 1fr;
    gap: 12px;
    align-items: center;
  }

  .ui-filelink-icon
  {
    position: relative;
    top: -2px;
    left: 4px;
    color: var(--color-text);
  }

  .ui-filelink-content
  {
    display: flex;
    flex-direction: column;
  }

  .ui-filelink-name
  {
    color: var(--color-text);
    font-weight: 700;
  }

  .ui-filelink-path
  {
    color: var(--color-text-dim);
    font-size: var(--font-size-s);
    margin-top: 3px;
  }
</style>