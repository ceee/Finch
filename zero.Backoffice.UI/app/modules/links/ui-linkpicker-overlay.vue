<template>
  <ui-trinity class="ui-linkpicker-overlay">
    <template v-slot:header>
      <ui-header-bar title="@links.overlay.title" :back-button="false" :close-button="true" @close="config.close(true)" />
    </template>
    <template v-slot:footer>
      <ui-button type="light onbg" label="@ui.close" :parent="config.rootId" @click="config.close" />
      <ui-button type="primary" label="@ui.confirm" @click="onSave" />
    </template>

    <div v-if="opened">
      <!--<div class="ui-box ui-linkpicker-overlay-options">
        <ui-property label="Open in a new tab">
          <ui-toggle :value="link.target === 'blank'" @input="onTargetChange" />
        </ui-property>
        <template v-if="showOptions">
          <hr />
          <ui-property label="Title" :vertical="true">
            <input v-model="link.title" type="text" class="ui-input" maxlength="160" />
          </ui-property>
          <hr />
          <ui-button label="Hide options" @click="showOptions=false" />
        </template>
        <div v-if="!showOptions">
          <hr />
          <ui-button label="More options" @click="showOptions=true" caret="down" />
        </div>
      </div>-->
      <div class="ui-linkpicker-overlay-area">
        <ui-property :vertical="true">
          <ui-select v-model="current" :items="areaItems"></ui-select>
        </ui-property>
      </div>

      <div class="ui-box">
        <component v-if="area && area.component" :is="area.component" :area="area" v-model="link" />
      </div>
    </div>
  </ui-trinity>
</template>


<script>
  export default {
    props: {
      config: Object
    },

    data: () => ({
      model: null,
      options: {},
      opened: false,
      current: null,
      area: null,
      areas: [],
      areaItems: [],
      link: null,
      template: {
        area: null,
        target: 'default',
        urlSuffix: null,
        title: null,
        values: {}
      },
      showOptions: false
    }),


    watch: {
      current()
      {
        this.reloadSelector();
      }
    },


    mounted()
    {
      this.model = this.config.model.value;
      this.options = this.config.model.options;

      this.areas = this.zero.linkAreas;

      this.areaItems = this.areas.map(x =>
      {
        return {
          value: x.alias,
          label: x.name
        };
      });
      this.link = JSON.parse(JSON.stringify(this.model || this.template));
      if (this.model && this.model.area && this.areas.find(x => x.alias === this.model.area))
      {
        this.area = this.areas.find(x => x.alias === this.model.area);
      }
      else
      {
        this.area = this.areas[0];
      }
      this.current = this.area.alias;
      this.link.area = this.current;
      setTimeout(() => this.opened = true, 300);
    },


    methods: {

      reloadSelector()
      {
        this.area = this.areas.find(x => x.alias === this.current);
      },

      onSave()
      {
        this.link.area = this.current;
        this.config.confirm(this.link);
      },

      onTargetChange(ev)
      {
        this.link.target = ev ? 'blank' : 'default';
      }
    }
  }
</script>

<style>
  .ui-linkpicker-overlay-area
  {
    margin: 0 0 var(--padding-s);
  }

  .ui-linkpicker-overlay .ui-property + .ui-property
  {
    margin-top: 15px;
  }

  .ui-linkpicker-overlay-area .ui-native-select
  {
    background: var(--color-box);
    border-radius: var(--radius);
    box-shadow: var(--shadow-short);
    font-weight: bold;
    border-color: transparent;
  }

  .ui-linkpicker-overlay-area .ui-native-select select
  {
    font-weight: bold;
  }

  .ui-linkpicker-overlay-area .ui-native-select select option
  {
    font-weight: normal;
  }

  .ui-linkpicker-overlay content
  {
    padding-top: 0;
  }

  .ui-linkpicker-overlay-options .ui-property
  {
    display: flex;
    justify-content: space-between;
  }

  .ui-linkpicker-overlay-options .ui-property + .ui-property
  {
    margin-top: var(--padding-m);
  }

  .ui-linkpicker-overlay-options .ui-property-content
  {
    display: inline;
    flex: 0 0 auto;
  }

  .ui-linkpicker-overlay-options .ui-property-label
  {
    padding-top: 1px;
  }
</style>