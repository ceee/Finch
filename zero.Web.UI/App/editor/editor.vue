<template>
  <div class="editor-outer" v-if="loaded" :class="'-infos-' + infos">
    <component :is="rootNode" class="editor">
      <ui-tab :class="renderInfo && index === -1 ? 'ui-view-box has-sidebar' : 'ui-box'" :label="tab.label" :count="tab.count(value)" v-for="(tab, index) in tabs" :key="index">
        <editor-component v-for="(field, fieldIndex) in tab.fields" :key="fieldIndex" :config="field" :renderer="configuration" v-model="value" />
      </ui-tab>
    </component>
    <aside v-if="infos && infos != 'none'" class="editor-infos">
      <div class="ui-box">
        <slot name="settings">
          <ui-property label="@ui.active" :is-text="true" class="is-toggle">
            <ui-toggle v-model="value.isActive" />
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
      grid-template-columns: 1fr 320px var(--padding);
      align-items: flex-start;
    }
  }

  .editor-infos
  {
    margin: 58px 0 0;

    .ui-box
    {
      margin: 0;
    }

    .ui-box + .ui-box
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
</style>