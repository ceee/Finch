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

    <div class="ui-box" v-if="loaded">
      <ui-property class="blueprint-settings-tableheader" :key="-1" label="Property" :vertical="false">
        <b>Synchronized</b>
      </ui-property>
      <ui-property v-for="(field, index) in items" v-if="!field.disabled" :key="index" :label="field.label" :description="field.description" :vertical="false" :class="{'not-synced': !field.synced}">
        <ui-toggle v-model="field.synced" />
      </ui-property>
    </div>
  </ui-overlay-editor>
</template>


<script>
  import Arrays from 'zero/helpers/arrays.js';
  import Localization from 'zero/helpers/localization.js';
  import BlueprintApi from 'zero/api/blueprint.js';

  export default {

    props: {
      model: Object,
      config: Object,
      fields: Array
    },

    data: () => ({
      state: 'default',
      loaded: false,
      editor: null,
      items: []
    }),


    mounted()
    {
      this.fields.forEach(field =>
      {
        let item = JSON.parse(JSON.stringify(field));
        item.synced = field.synced(this.model);
        this.items.push(item);
      });
      this.loaded = true;
    },


    methods: {

      onSave()
      {
        this.state = 'loading';

        let desync = JSON.parse(JSON.stringify(this.model.blueprint.desync));
        let resync = [];

        this.items.forEach(field =>
        {
          let desynced = this.model.blueprint.desync.indexOf(field.path) > -1;

          if (field.synced && desynced)
          {
            Arrays.remove(desync, field.path);
            resync.push(field.path);
          }
          else if (!field.synced && !desynced)
          {
            desync.push(field.path);
          }
        });

        this.model.blueprint.desync = desync;

        // we need to revert changed values which were switch backed to synchronised state
        // to do this we load the blueprint entity and its copy properties

        if (resync.length > 0)
        {
          BlueprintApi.getById(this.model.blueprint.id).then(blueprint =>
          {
            this.config.confirm({
              blueprint: this.model.blueprint,
              update: entity =>
              {
                resync.forEach(path =>
                {
                  entity[path] = blueprint[path]; // TODO does not work for nested paths
                });
              }
            });
          });
        }
        else
        {
          //this.state = 'success';
          this.config.confirm({
            blueprint: this.model.blueprint
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

  .blueprint-settings .blueprint-settings-tableheader
  {
    border-bottom: 1px dashed var(--color-line-dashed);
    padding-bottom: 20px;
    margin-bottom: 26px;

    b
    {
      display: inline-block;
      margin-top: 3px;
    }
  }

  .blueprint-settings .ui-property + .ui-property
  {
    margin-top: var(--padding-s);
    padding-top: 0;
    border-top: none;
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
    color: var(--color-text-dim);
  }

  .blueprint-settings-lock
  {
    color: var(--color-text-dim);
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