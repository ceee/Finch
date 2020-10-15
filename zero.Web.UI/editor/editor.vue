<template>
  <div>
    <div class="editor-outer" v-if="loaded && !rendererNotFound" :class="{ 'has-tabs': hasTabs, '-infos-aside': !nested, 'is-page': isPage }" :renderer="config">
      <ui-tabs class="editor" :active="activeTab">
        <ui-tab class="ui-box" :class="tab.class" :label="tab.label" :count="tab.count(modelValue)" v-for="(tab, index) in tabs" :key="index" :depth="depth" :name="tab.name">
          <div v-if="!tabDisabled(tab)">
            <editor-component v-for="(field, fieldIndex) in tab.fields" :key="fieldIndex" :config="field" :renderer="configuration" v-model="modelValue" @input="onChange" :meta="meta" :depth="depth" :disabled="disabled" />
            <component v-if="tab.component" :is="tab.component" v-model="modelValue" :disabled="disabled" />
          </div>
        </ui-tab>
      </ui-tabs>
      <aside v-if="!nested && infos && infos != 'none'" class="editor-infos">
        <slot name="info-boxes"></slot>
        <div class="ui-box" v-if="isShared" :class="{'is-active': modelValue.isActive }">
          <div class="editor-global-flag">
            <i class="fth-radio"></i>
            <p>
              <b>This entity is bound to a parent</b> and automatically synchronised.<br>
              <!--<a href="/">Edit parent</a>-->
            </p>
          </div>
          <ui-button type="light small" label="Settings" @click="editBlueprint(modelValue.blueprint)" />
          <ui-button type="light small" label="Edit parent" @click="editBlueprint(modelValue.blueprint)" />
        </div>
        <div class="ui-box editor-active-toggle" v-if="activeToggle" :class="{'is-active': modelValue.isActive }">
          <slot name="settings">
            <ui-property v-if="activeToggle" label="@ui.active" :is-text="true" class="is-toggle">
              <ui-toggle v-model="modelValue.isActive" class="is-primary" :disabled="disabled" />
            </ui-property>
          </slot>
          <slot name="settings-properties"></slot>
        </div>
        <div class="ui-box is-light" v-if="modelValue.id">
          <slot name="infos">
            <ui-property v-if="modelValue.id" label="@ui.id" :is-text="true">
              {{modelValue.id}}
            </ui-property>
            <ui-property v-if="modelValue.id && modelValue.lastModifiedDate" label="@ui.modifiedDate" :is-text="true">
              <ui-date v-model="modelValue.lastModifiedDate" />
            </ui-property>
            <ui-property v-if="modelValue.id" label="@ui.createdDate" :is-text="true">
              <ui-date v-model="modelValue.createdDate" />
            </ui-property>
            <slot name="infos-more"></slot>
          </slot>
        </div>
        <slot name="infos-after"></slot>
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
  import EditorComponent from '@zero/editor/editor-component.vue';
  import { each as _each, map as _map, filter as _filter, isArray as _isArray } from 'underscore';

  export default {
    name: 'uiEditor',

    emits: ['input'],

    props: {
      config: {
        type: [ String, Object ],
        required: true
      },
      meta: {
        type: Object,
        default: () => { }
      },
      modelValue: {
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
      },
      disabled: {
        type: Boolean,
        default: false
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
        return this.modelValue && this.modelValue.blueprint != null;
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
        this.$emit('input', this.modelValue);
        this.$emit('update:modelValue', this.modelValue);
      },


      tabDisabled(tab)
      {
        if (typeof tab.disabled !== 'function')
        {
          return false;
        }

        return tab.disabled(this.modelValue);
      },


      editBlueprint(blueprint)
      {
        let params = this.$route.params;
        params.id = blueprint.id;

        this.$router.push({
          name: this.$route.name,
          params: params
        });
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
    position: sticky;
    top: var(--padding);

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
    display: grid;
    grid-template-columns: auto minmax(0, 1fr);
    grid-gap: 16px;
    padding-bottom: 10px;
    align-items: center;
    font-size: var(--font-size);
    line-height: 1.5;
    position: relative;

    p
    {
      margin: 0;
      color: var(--color-text);
    }

    i
    {
      font-size: 24px;
      color: var(--color-primary);
      margin-top: -2px;
    }

    & + .ui-button
    {
      margin-top: 1.2rem;
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
      background: var(--color-box-nested);
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