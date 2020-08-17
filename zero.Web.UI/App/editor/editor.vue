<template>
  <div>
    <div class="editor-outer" v-if="loaded && !rendererNotFound" :class="{ 'has-tabs': hasTabs, '-infos-aside': !nested, 'is-page': isPage }" :renderer="config">
      <ui-tabs class="editor" :active="activeTab">
        <ui-tab class="ui-box" :class="tab.class" :label="tab.label" :count="tab.count(value)" v-for="(tab, index) in tabs" :key="index" :depth="depth" :name="tab.name">
          <editor-component v-for="(field, fieldIndex) in tab.fields" :key="fieldIndex" :config="field" :renderer="configuration" v-model="value" @input="onChange" :meta="meta" :depth="depth" />
          <component v-if="tab.component" :is="tab.component" v-model="value" />
        </ui-tab>
      </ui-tabs>
      <aside v-if="!nested && infos && infos != 'none'" class="editor-infos">
        <div class="ui-box editor-active-toggle" v-if="isShared || activeToggle" :class="{'is-active': value.isActive }">
          <slot name="settings">
            <div v-if="isShared" class="editor-global-flag">
              <b>This entity is shared</b> and can be used by all applications.<br>
              <a href="/">More info</a>
              <i class="fth-radio"></i>
            </div>
            <ui-property v-if="activeToggle" label="@ui.active" :is-text="true" class="is-toggle">
              <ui-toggle v-model="value.isActive" class="is-primary" />
            </ui-property>
          </slot>
          <slot name="settings-properties"></slot>
        </div>
        <div class="ui-box">
          <slot name="infos">
            <ui-property v-if="value.id" label="@ui.id" :is-text="true">
              {{value.id}}
            </ui-property>
            <ui-property v-if="value.id" label="@ui.createdDate" :is-text="true">
              <ui-date v-model="value.createdDate" />
            </ui-property>
          </slot>
        </div>
      </aside>
    </div>
    <div class="page page-error editor-error" v-if="loaded && rendererNotFound">
      <i class="page-error-icon fth-cloud-snow"></i>
      <p class="page-error-text">
        <strong class="page-error-headline">Not found</strong><br>
        The renderer <code>[{{config}}]</code> could not be found
      </p>
    </div>
  </div>
</template>


<script>
  import EditorComponent from 'zero/editor/editor-component';
  import { each as _each, map as _map, filter as _filter } from 'underscore';

  export default {
    name: 'uiEditor',

    props: {
      config: {
        type: [ String, Object ],
        required: true
      },
      meta: {
        type: Object,
        default: () => { }
      },
      value: {
        type: Object
      },
      infos: {
        type: String,
        default: 'aside'
      },
      activeToggle: {
        type: Boolean,
        default: true
      },
      nested: {
        type: Boolean,
        default: false
      },
      isPage: {
        type: Boolean,
        default: false
      },
      depth: {
        type: Number,
        default: 0
      },
      onConfigure: {
        type: Function,
        default: () => { }
      },
      activeTab: {
        type: Number,
        default: 0
      }
    },

    components: { EditorComponent },

    data: () => ({
      rendererNotFound: false,
      configuration: {},
      loaded: false,
      hasTabs: false,
      tabs: [],
      fields: [],
      renderInfo: true
    }),

    computed: {
      isShared()
      {
        return this.meta.canBeShared && this.value && this.value.appId === zero.sharedAppId;
      }
    },

    watch: {
      config: {
        deep: true,
        handler: function()
        {
          this.load();
        }
      }
    },  

    created()
    {
      this.load();
    },

    methods: {

      load()
      {
        if (typeof this.config === 'string')
        {
          this.finishLoad(zero.renderers[this.config]);
        }
        else
        {
          this.finishLoad(this.config);
        }
      },


      finishLoad(config)
      {
        this.configuration = config;

        if (!config || !config.fields)
        {
          this.rendererNotFound = true;
          this.loaded = true;
          return;
        }

        this.fields = config.fields;

        let tabs = this.configuration.tabs || [];

        this.tabs = _map(tabs, (tab, index) =>
        {
          let tabConfig = tab;
          tabConfig.count = typeof tabConfig.count === 'function' ? tabConfig.count : () => null;
          tabConfig.fields = _filter(this.configuration.fields, x => index === 0 ? !x.tab || x.tab === tab.name : x.tab === tab.name);
          return tabConfig;
        });

        this.hasTabs = this.tabs.length > 0 && !this.nested;

        if (this.tabs.length < 1)
        {
          this.tabs.push({
            label: '@ui.tab_general',
            fields: this.configuration.fields,
            count: () => null
          });

          if (this.isPage)
          {
            this.tabs[0].label = '@ui.tab_content';
            this.hasTabs = true;
          }
        }

        this.onConfigure(this);

        this.rendererNotFound = false;
        this.loaded = true;
      },


      onChange()
      {
        this.$emit('input', this.value);
      }
    }
  }
</script>

<style lang="scss">
  .editor > .ui-view-box
  {
    padding-top: 0;
  }

  .editor
  {
    .ui-tabs-list
    {
      padding-top: 0;
    }
  }

  .editor-outer
  {
    &.-infos-aside:not(.is-page)
    {
      display: grid;
      grid-template-columns: 1fr 340px var(--padding);
      align-items: flex-start;
    }

    &:not(.has-tabs) 
    {
      .ui-tabs-list
      {
        display: none;
      }

      .ui-tab, .editor-infos
      {
        margin-top: 0;
      }
    }
  }

  .editor-infos
  {
    margin: 50px 0 0;

    .ui-box
    {
      margin-left: 0;
      margin-right: 0;
      margin-bottom: 0;
    }

    .ui-box:first-child
    {
      margin-top: 0;
    }

    .ui-box + .ui-box
    {
      margin-top: 16px;
    }

    .ui-box + .ui-box.is-light
    {
      margin-top: 1px;
      border-top-left-radius: 0;
      border-top-right-radius: 0;
    }

    .ui-property + .ui-property
    {
      margin-top: 10px;
      padding-top: 10px;
      border-top: none;
    }

    .is-toggle
    {
      align-items: center;
    }

    .ui-property-label
    {
      width: auto;
      padding-right: 20px;
    }

    .ui-property-content
    {
      flex: 0 1 auto;
    }

    .ui-property
    {
      display: flex;
      justify-content: space-between;
    }
  }

  .editor-active-toggle
  {
    border-bottom-left-radius: 0;
    border-bottom-right-radius: 0;
  }

  /*.editor-active-toggle.is-active
  {
    background: var(--color-accent-info-bg);   

    .ui-property-label
    {
      color: var(--color-accent-info);
    }
  }*/


  .editor-global-flag
  {
    font-size: var(--font-size);
    line-height: 1.5;
    padding-right: 70px;
    position: relative;

    i
    {
      position: absolute;
      top: 50%;
      margin-top: -28px;
      right: -3px;
      font-size: 42px;
      color: var(--color-fg-dim);
      opacity: 0.2;
    }

    a
    {
      color: var(--color-fg-dim);
      text-decoration: underline dotted;
      font-size: var(--font-size-s);
    }

    & + .ui-property
    {
      margin-top: 32px;
      padding-top: 32px;
      border-top: 1px solid var(--color-line);
    }

    &.is-block
    {
      padding: 20px;
      padding-right: 70px;
      border-radius: var(--radius);
      background: var(--color-bg-bright-two);
      margin-bottom: var(--padding);

      i
      {
        margin-top: -22px;
        right: 20px;
      }
    }
  }

  .editor-error
  {
    background: transparent;
    min-height: calc(100vh - 100px);
  }
</style>