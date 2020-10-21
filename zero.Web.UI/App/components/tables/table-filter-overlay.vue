<template>
  <ui-form class="ui-table-filter-overlay" ref="form" v-slot="form" @submit="onSubmit" @load="onLoad">
    <ui-overlay-editor class="ui-module-overlay">

      <template v-slot:header>
        <ui-header-bar :title="config.title" :back-button="false" :close-button="true" />
      </template>

      <template v-slot:footer>
        <ui-button type="light onbg" label="@ui.close" @click="config.hide"></ui-button>
        <ui-button v-if="!config.isCreate" type="light onbg" label="@ui.remove" @click="onRemove"></ui-button>
        <ui-button type="primary" :submit="true" label="@ui.confirm" :state="form.state" :disabled="loading || disabled"></ui-button>
      </template>

      <ui-loading v-if="loading" :is-big="true" />

      <!--<div v-if="!loading" class="ui-box">
        <ui-property label="Name" :required="true">
          <input v-model="model.name" type="text" class="ui-input" maxlength="300" />
        </ui-property>
      </div>-->

      <div v-if="!loading" class="ui-box">
        <div v-for="(field, index) in fields" class="ui-table-filter-overlay-group" :class="{ 'is-open': activeFilter === index, 'has-value': field.hasValue(model[field.path]) }">
          <div class="ui-table-filter-overlay-group-head">
            <ui-select-button :label="getFieldLabel(field.field)" :icon="field.icon" :description="field.preview(model[field.path])" @click="toggleFilter(field, index)" />
            <ui-button class="ui-table-filter-overlay-group-clear" type="blank" label="Clear" @click="clearFilter(field, index)" />
          </div>
          <div v-show="activeFilter === index" class="ui-table-filter-overlay-group-content">
            <editor-component :config="field.field" :editor="editor" v-model="model" @input="onFieldChange" :disabled="disabled" />
          </div>
        </div>
      </div>

      <div v-if="!loading" class="ui-box ui-table-filter-overlay-filtername">
        <ui-property label="Save as..." description="You can optionally save this filter for future reference" :vertical="true">
          <input v-model="filterName" type="text" class="ui-input" maxlength="40" />
        </ui-property>
      </div>

    </ui-overlay-editor>
  </ui-form>
</template>


<script>
  import EditorComponent from 'zero/editor/editor-component';
  import { find as _find } from 'deps/underscore';

  export default {

    props: {
      config: Object
    },

    provide: function ()
    {
      return {
        meta: {},
        disabled: false
      };
    },

    data: () => ({
      loading: false,
      disabled: false,
      model: {},
      template: null,
      editor: null,
      fields: [],
      filterName: null,
      activeFilter: null
    }),

    components: { EditorComponent },

    methods: {

      toggleFilter(filter, index)
      {
        this.activeFilter = this.activeFilter === index ? null : index;
      },

      onFieldChange(value)
      {
        //this.$emit('input', this.value);
      },

      clearFilter(filter, index)
      {
        this.model[filter.path] = JSON.parse(JSON.stringify(this.template[filter.path]));
      },

      getFieldLabel(field)
      {
        return field.options.label || this.editor.templateLabel(field.path);
      },
      
      onLoad()
      {
        this.model = JSON.parse(JSON.stringify(this.config.model.filter || {}));
        this.template = JSON.parse(JSON.stringify(this.config.template || {}));
        this.editor = this.config.editor;
        this.filterName = this.config.model.name;
        this.fields = this.editor.fields.map(x =>
        {
          return {
            field: x,
            path: x.path,
            ...x.previewOptions
          };
        });

        this.loading = false;
      },

      onSubmit()
      {
        this.config.confirm({
          model: this.model,
          filterName: this.filterName
        });
      },

      onRemove()
      {
        this.config.confirm({
          remove: true
        });
      }
    }

  }
</script>

<style lang="scss">
  .ui-table-filter-overlay
  {
    .ui-box + .ui-box
    {
      margin-top: var(--padding-s);
    }
  }

  .ui-table-filter-overlay-group
  {
    &:not(.is-open) + .ui-table-filter-overlay-group
    {
      margin-top: var(--padding-s);
      border-top: 1px solid var(--color-line);
      padding-top: var(--padding-s);
    }

    &:not(.has-value) .ui-table-filter-overlay-group-clear
    {
      display: none;
    }
  }

  .ui-table-filter-overlay-group-head
  {
    display: grid;
    grid-template-columns: minmax(0, 1fr) auto;
    grid-gap: 20px;
  }

  .ui-table-filter-overlay-group-clear .ui-button-text
  {
    font-weight: 400;
    color: var(--color-text-dim);
  }

  .ui-table-filter-overlay-group-content
  {
    background: var(--color-box);
    margin: var(--padding-s) 0;
    padding: var(--padding-m) 0;
    border-top: 1px solid var(--color-line);
    border-bottom: 1px solid var(--color-line);

    > .ui-property > .ui-property-label
    {
      display: none;
    }
  }

  .ui-table-filter-overlay-filtername .ui-property-content
  {
    margin-top: 12px !important;
  }
</style>