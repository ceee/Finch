<template>
    <div class="ui-modules-start">

      <button v-if="!isSelecting" type="button" class="ui-modules-start-button" @click="startSelection">
        <i class="ui-modules-start-button-icon fth-plus"></i>
        <p class="ui-modules-start-button-text"><strong>Add content</strong> <!--<br>Compose the page by adding modules--></p>
      </button>

      <div class="ui-modules-select" v-if="isSelecting">
        <ui-icon-button class="ui-modules-select-close" @click="isSelecting=false" icon="fth-x" title="@ui.close" />
        <ui-inline-tabs class="ui-modules-select-groups">
          <ui-tab v-for="group in moduleTypes" :key="group.key" :label="group.name" :count="group.count">
            <div class="ui-modules-select-items">
              <button v-for="item in group.items" :key="item.alias" type="button" class="ui-modules-select-item" :disabled="item.isDisabled" @click="editModule(item, true)">
                <div class="ui-modules-select-item-icon">
                  <i :class="item.icon"></i>
                </div>
                <div class="ui-modules-select-item-text">
                  <strong v-localize="item.name"></strong>
                  <span class="is-minor" v-localize="item.description"></span>
                </div>
                <span v-if="item.isDisabled" class="ui-modules-select-item-disabled">Not allowed <i class="fth-slash"></i></span>
              </button>
            </div>
          </ui-tab>
        </ui-inline-tabs>
      </div>
    </div>
</template>


<script>
  import { groupBy as _groupBy, keys as _keys, each as _each } from 'underscore';
  import Notification from 'zero/services/notification.js';

  export default {
    name: 'uiModulesSelect',

    props: {
      value: Array,
      config: Object,
      types: {
        type: Array,
        default: () => []
      },
      disabled: {
        type: Boolean,
        default: false
      }
    },


    data: () => ({
      canAdd: true,
      isSelecting: false,
      moduleTypes: [],
      activeGroup: null
    }),


    watch: {
      types(val)
      {
        this.rebuildGroups(val);
      }
    },


    created()
    {
      this.rebuildGroups(this.types);
    },


    methods: {

      rebuildGroups(value)
      {
        let groups = _groupBy(value, val => val.group);
        let index = 0;

        _each(groups, (items, key) =>
        {
          this.moduleTypes.push({
            name: !key || key === 'null' ? '@modules.default_group' : key,
            index: index++,
            count: items.length,
            items: items,
            isDisabled: false
          });
        });

        this.activeGroup = this.moduleTypes[0];
      },


      startSelection()
      {
        if (this.types.length > 1)
        {
          this.isSelecting = true;
        }
        else if (this.types.length === 1)
        {
          this.$emit('selected', this.types[0], true);
        }
        else
        {
          Notification.error('No modules allowed', 'There are no modules configured which are allowed for this data type.', { duration: 5000 });
        }
      },


      selectGroup(group)
      {
        this.activeGroup = group;
      },


      editModule(module, isAdd)
      {
        this.$emit('selected', module, isAdd);
      },


      reset()
      {
        this.isSelecting = false;
        this.activeGroup = this.moduleTypes[0];
      }
    }
  }
</script>

<style lang="scss">
  .ui-modules-start
  {
    margin: 0;
    display: flex;

    .ui-modules-inner-sortable + &
    {
      margin-top: var(--padding);
    }
  }

  .ui-modules-start-button
  {
    color: var(--color-primary);
    font-size: var(--font-size);
    display: inline-grid;
    grid-template-columns: auto 1fr;
    gap: 25px;
    align-items: center;
  }

  .ui-modules-start-button-icon
  {
    width: 52px;
    height: 52px;
    line-height: 50px !important;
    font-size: 20px;
    text-align: center;
    background: var(--color-button-light);
    border-radius: var(--radius);
  }

  .ui-modules-start-button-text
  {
    line-height: 1.3;
    color: var(--color-text-dim);
    margin: 0;
    font-size: var(--font-size-s);

    strong
    {
      display: inline-block;
      margin-bottom: 2px;
      color: var(--color-text);
      font-size: var(--font-size);
    }
  }

  .ui-modules-select
  {
    width: 100%;
    position: relative;

    .ui-inline-tabs-list
    {
      padding-right: 50px;
    }
  }

  .ui-modules-select-close
  {
    position: absolute;
    right: 0;
    top: 0;
    background: none !important;
  }

  .ui-modules-select-items
  {
    display: grid;
    gap: 10px;
    grid-template-columns: repeat(auto-fill, minmax(260px, 1fr));
    align-items: stretch;
  }

  .ui-modules-select-item
  {
    display: grid;
    grid-template-columns: 76px 1fr;
    align-items: center;
    height: 100px;
    border-radius: var(--radius);
    background: var(--color-button-light);
    padding: 10px var(--padding) 10px 0;
    position: relative;

    &[disabled]
    {
      opacity: .6;
    }
  }

  .ui-modules-select-item-icon
  {
    display: inline-flex;
    justify-content: center;
    color: var(--color-text-dim);
    font-size: 26px;
  }

  .ui-modules-select-item-text
  {
    strong
    {
      display: block;
      margin-bottom: 3px;
    }

    .is-minor
    {
      color: var(--color-text-dim);
      font-size: var(--font-size-s);
    }
  }

  .ui-modules-select-item-disabled
  {
    position: absolute;
    right: 5px;
    top: 5px;
    display: inline-flex;
    align-items: center;
    font-size: 8px;
    text-transform: uppercase;
    color: var(--color-text-dim-one);
    line-height: 1;
    font-weight: 600;

    i
    {
      margin-left: 6px;
      font-size: 13px;
    }
  }
</style>