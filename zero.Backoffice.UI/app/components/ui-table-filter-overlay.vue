<template>
  <ui-form class="ui-table-filter-overlay" ref="form" v-slot="form" @submit="onSubmit" @load="onLoad">
    <ui-trinity class="ui-module-overlay">

      <template v-slot:header>
        <ui-header-bar title="@listfilter.headline" :back-button="false" :close-button="true" @close="config.close(true)" />
      </template>

      <template v-slot:footer>
        <ui-button type="light onbg" label="@ui.close" @click="config.close"></ui-button>
        <ui-button v-if="!model.isCreate" type="light onbg" label="@ui.remove" @click="onRemove"></ui-button>
        <ui-button type="accent" :submit="true" label="@ui.confirm" :state="form.state" :disabled="loading || disabled || !filterName"></ui-button>
      </template>

      <ui-loading v-if="loading" :is-big="true" />

      <!--<div v-if="!loading" class="ui-box">
        <ui-property label="Name" :required="true">
          <input v-model="model.name" type="text" class="ui-input" maxlength="300" />
        </ui-property>
      </div>-->

      <div v-if="!loading" class="ui-box">
        <div v-for="(field, index) in fields" class="ui-table-filter-overlay-group" :class="{ 'is-open': activeFilter === index, 'has-value': field.preview.selected(value[field.path]) }">
          <div class="ui-table-filter-overlay-group-head">
            <ui-select-button :label="field.label" :icon="field.preview.icon" :description="field.preview.value(value[field.path])" @click="toggleFilter(field, index)" />
            <ui-button class="ui-table-filter-overlay-group-clear" type="blank" label="@listfilter.clearitem" @click="clearFilter(field, index)" />
          </div>
          <div v-show="activeFilter === index" class="ui-table-filter-overlay-group-content">
            <ui-editor-component :field="field" :value="value" @input="onFieldChange" :disabled="disabled" />
          </div>
        </div>
      </div>

      <div v-if="!loading" class="ui-box ui-table-filter-overlay-filtername">
        <ui-property label="@listfilter.saveas" description="@listfilter.saveas_text" :vertical="true">
          <input v-model="filterName" type="text" class="ui-input" maxlength="40" />
        </ui-property>
      </div>

    </ui-trinity>
  </ui-form>
</template>


<script>
  import { compileEditor } from '../editor/compile';

  export default {

    props: {
      model: Object,
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
      value: {},
      loading: true,
      disabled: false,
      template: null,
      editor: null,
      fields: [],
      filterName: null,
      activeFilter: null
    }),

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
        this.value[filter.path] = JSON.parse(JSON.stringify(this.template[filter.path]));
      },
      
      onLoad()
      {
        this.loading = true;
        this.template = JSON.parse(JSON.stringify(this.model.template || {}));
        this.value = JSON.parse(JSON.stringify(this.model.value.filter || this.template));
        this.editor =  compileEditor(this.zero, this.model.editor);
        this.filterName = this.model.value.name;
        this.fields = [];


        this.editor.tabs.forEach(tab =>
        {
          tab.fieldsets.forEach(set =>
          {
            set.fields.forEach(field =>
            {
              if (!field.preview)
              {
                console.warn('[zero] All fields in a filter need filled-out preview options');
              }
              else
              {
                this.fields.push(field);
              }
            });
          });
        });

        this.loading = false;
      },

      onSubmit()
      {
        this.config.confirm({
          model: this.value,
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

  .ui-table-filter-overlay .ui-select-button-description
  {
    color: var(--color-primary);
  }
</style>