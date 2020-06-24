<template>
  <div class="editor-outer" v-if="loaded" :class="'-infos-' + infos">
    <component :is="rootNode" class="editor">
      <ui-tab :class="renderInfo && index === -1 ? 'ui-view-box has-sidebar' : 'ui-box'" :label="tab.label" :count="tab.count(value)" v-for="(tab, index) in tabs" :key="index">
        <editor-component v-for="(field, fieldIndex) in tab.fields" :key="fieldIndex" :config="field" :renderer="configuration" v-model="value" />
      </ui-tab>
    </component>
    <aside v-if="infos && infos != 'none'" class="editor-infos">
      

      <div class="ui-box editor-active-toggle" :class="{'is-active': value.isActive }">
        <slot name="settings">
          <div v-if="isShared" class="editor-global-flag">
            <b>This entity is shared</b> and can be used by all applications.<br>
            <a href="/">More info</a>
            <i class="fth-radio"></i>
          </div>
          <ui-property label="@ui.active" :is-text="true" class="is-toggle">
            <ui-toggle v-model="value.isActive" class="is-primary" />
          </ui-property>
        </slot>
      </div>
      <div class="ui-box is-light is-connected">
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
</template>


<script>
  import EditorComponent from 'zero/editor/editor-component';
  import RendererApi from 'zero/resources/renderer';
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
      }
    },

    components: { EditorComponent },

    data: () => ({
      configuration: {},
      loaded: false,
      hasTabs: false,
      tabs: [],
      fields: [],
      renderInfo: true
    }),

    computed: {
      rootNode()
      {
        return this.hasTabs ? 'ui-tabs' : 'div';
      },
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

        if (!config.fields)
        {
          return;
        }

        this.fields = config.fields;
        this.hasTabs = typeof this.configuration.tabs !== 'undefined';

        let tabs = this.configuration.tabs || [];

        this.tabs = _map(tabs, (tab, index) =>
        {
          let tabConfig = tab;
          tabConfig.count = typeof tabConfig.count === 'function' ? tabConfig.count : () => null;
          tabConfig.fields = _filter(this.configuration.fields, x => index === 0 ? !x.tab || x.tab === tab.name : x.tab === tab.name);
          return tabConfig;
        });

        if (!this.tabs.length)
        {
          this.tabs.push({
            fields: this.configuration.fields
          });
        }



        this.loaded = true;
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
    &.-infos-aside
    {
      display: grid;
      grid-template-columns: 1fr 340px var(--padding);
      align-items: flex-start;
    }
  }

  .editor-infos
  {
    margin: 58px 0 0;

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
    margin-bottom: 32px;
    padding-bottom: 32px;
    border-bottom: 1px solid var(--color-line-light);
    padding-right: 70px;
    position: relative;

    i
    {
      position: absolute;
      top: 50%;
      margin-top: -36px;
      right: -3px;
      font-size: 42px;
      color: var(--color-fg-xlight);
      opacity: 0.2;
    }

    a
    {
      color: var(--color-fg-light);
      text-decoration: underline dotted;
      font-size: var(--font-size-s);
    }
  }
</style>