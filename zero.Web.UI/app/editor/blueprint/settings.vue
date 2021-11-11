<template>
  <ui-overlay-editor class="blueprint-settings">
    <template v-slot:header>
      <ui-header-bar title="Synchronisation" :back-button="false" :close-button="true" />
    </template>
    <template v-slot:footer>
      <ui-button type="light onbg" label="@ui.close" @click="config.hide" />
      <ui-button type="primary" label="@ui.confirm" @click="onSave" :state="state" />
    </template>

    <p class="blueprint-settings-text">By default all properties of your entity are synced with its blueprint.<br />You can disable synchronisation per property so it won't be overridden on changes.</p>

    <div class="ui-box">
      <ui-property v-for="(item, index) in items" :key="index" :label="item.label" :vertical="false" :class="{'not-synced': !item.synced}"> 
        <ui-toggle :value="item.synced" @input="onChange(item)" />
      </ui-property>
    </div>
  </ui-overlay-editor>
</template>


<script>
  import PagesApi from 'zero/api/pages.js';
  import Notification from 'zero/helpers/notification.js';
  import Arrays from 'zero/helpers/arrays.js';
  import BlueprintApi from 'zero/api/blueprint.js';

  export default {

    props: {
      isCopy: {
        type: Boolean,
        default: false
      },
      model: Object,
      config: Object
    },

    data: () => ({
      desync: [],
      items: [],
      state: 'default',
      editor: null
    }),


    mounted()
    {
      this.desync = JSON.parse(JSON.stringify(this.model.desync));

      this.editor = this.config.editor;

      let processed = ['blueprint'];

      this.editor.fields.forEach(field =>
      {
        if (processed.indexOf(field.path) < 0)
        {
          processed.push(field.path);
          //field.options.disabled = true;

          this.items.push({
            path: field.path,
            label: field.options.label || this.editor.templateLabel(field.path),
            synced: this.model.desync.indexOf(field.path) < 0
          });
        }
      });
    },


    methods: {

      onChange(item)
      {
        item.synced = !item.synced;

        if (item.synced)
        {
          Arrays.remove(this.model.desync, item.path);
        }
        else
        {
          this.model.desync.push(item.path);
        }
      },

      onSave()
      {
        this.state = 'loading';

        // we need to revert changed values which were switch backed to synchronised state
        // to do this we load the blueprint entity and its copy properties
        let resync = [];

        this.desync.forEach(path =>
        {
          if (this.model.desync.indexOf(path) < 0)
          {
            resync.push(path);
          }
        });

        if (resync.length > 0)
        {
          BlueprintApi.getById(this.model.id).then(blueprint =>
          {
            this.config.confirm({
              blueprint: this.model,
              update: entity =>
              {
                resync.forEach(path =>
                {
                  entity[path] = blueprint[path];
                });
              }
            });
          });
        }
        else
        {
          //this.state = 'success';
          this.config.confirm({
            blueprint: this.model
          });
        }

        
      },

      //rebuildModel()
      //{
      //  this.selector = Strings.selectorToArray(this.config.path);
      //  let currentValue = this.value;
      //  let found = false;

      //  if (!this.selector || !this.selector.length || !currentValue)
      //  {
      //    found = true;
      //    this.model = null;
      //  }
      //  else
      //  {
      //    for (var key of this.selector)
      //    {
      //      if (key in currentValue)
      //      {
      //        found = true;
      //        currentValue = currentValue[key];
      //      }
      //      else
      //      {
      //        break;
      //      }
      //    }

      //    this.model = found ? currentValue : null;
      //  }
      //},
    }
  }
</script>

<style lang="scss">
  .blueprint-settings-text
  {
    margin: 0 0 var(--padding);
    line-height: 1.5;
  }
  .blueprint-settings-headline
  {
    margin: 0 0 var(--padding) !important;
  }

  .blueprint-settings .ui-property
  {
    display: flex;
    justify-content: space-between;
  }

  .blueprint-settings .ui-property + .ui-property
  {
    margin-top: 20px;
    border-top: none;
    padding-top: 0;
  }

  .blueprint-settings .ui-property-content
  {
    display: inline;
    flex: 0 0 auto;
  }

  .blueprint-settings .ui-property-label
  {
    padding-top: 1px;
  }

  .blueprint-settings .ui-property.not-synced .ui-property-label
  {
    font-weight: 400;
  }

  /*.blueprint-settings .ui-property.not-synced .ui-property-label:before
  {
    content: "\e929";
    font-family: 'Feather';
    margin-right: 0.8em;
    color: var(--color-text-dim);
    font-weight: 400;
  }

  .blueprint-settings .ui-property:not(.not-synced) .ui-property-label:before
  {
    content: "\e8f8";
    font-family: 'Feather';
    margin-right: 0.8em;
    color: var(--color-primary);
    font-weight: 400;
  }*/
</style>