<template>
    <div class="ui-modules-start">

      <button v-if="!isSelecting" type="button" class="ui-modules-start-button" @click="isSelecting=true">
        <i class="ui-modules-start-button-icon fth-plus"></i>
        <p class="ui-modules-start-button-text"><strong>Add content</strong> <br>Compose the page by adding modules</p>
      </button>

      <div class="ui-modules-select" v-if="isSelecting">
        <ui-icon-button class="ui-modules-select-close" @click="isSelecting=false" icon="fth-x" title="@ui.close" />
        <nav class="ui-modules-select-groups">
          <button v-for="group in moduleTypes" type="button" class="ui-modules-select-group" :class="{'is-active': activeGroup.index === group.index }" @click="selectGroup(group)">
            {{group.name}}
            <span class="ui-modules-select-group-count">{{group.count}}</span>
          </button>
        </nav>
        <div class="ui-modules-select-items">
          <button v-for="item in activeGroup.items" :key="item.alias" type="button" class="ui-modules-select-item" :disabled="item.isDisabled" @click="editModule(item, true)">
            <div class="ui-modules-select-item-icon">
              <i :class="item.icon"></i>
            </div>
            <div class="ui-modules-select-item-text">
              <strong>{{item.name}}</strong>
              <span class="is-minor">{{item.description}}</span>
            </div>
            <span v-if="item.isDisabled" class="ui-modules-select-item-disabled">Not allowed <i class="fth-slash"></i></span>
          </button>
        </div>
      </div>
    </div>
</template>


<script>
  import { groupBy as _groupBy, keys as _keys, each as _each } from 'underscore';

  export default {
    name: 'uiModulesSelect',

    props: {
      value: Array,
      config: Object,
      types: {
        type: Array,
        default: () => []
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
            name: key || '@modules.default_group',
            index: index++,
            count: items.length,
            items: items,
            isDisabled: false
          });
        });

        this.activeGroup = this.moduleTypes[0];
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
    padding: var(--padding);
    background: var(--color-bg-bright);
    border-radius: var(--radius);
    box-shadow: var(--color-shadow-short);
    display: flex;
    justify-content: center;
  }

  .ui-modules-start-button
  {
    color: var(--color-fg);
    font-size: var(--font-size);
    display: inline-grid;
    grid-template-columns: auto 1fr;
    grid-gap: 25px;
    align-items: center;
  }

  .ui-modules-start-button-icon
  {
    width: 70px;
    height: 70px;
    line-height: 68px !important;
    font-size: 20px;
    text-align: center;
    background: var(--color-bg-bright-two);
    border-radius: var(--radius);
  }

  .ui-modules-start-button-text
  {
    line-height: 1.3;
    color: var(--color-fg-dim);

    strong
    {
      display: inline-block;
      margin-bottom: 5px;
      color: var(--color-fg);
    }
  }

  .ui-modules-select
  {
    width: 100%;
    position: relative;
  }

  .ui-modules-select-close
  {
    position: absolute;
    right: 0;
    top: 0;
  }

  .ui-modules-select-groups
  {
    display: flex;
    margin-bottom: var(--padding);
    padding-right: 50px;
  }

  .ui-modules-select-group
  {
    display: inline-flex;
    align-items: center;
    padding: 6px 6px 6px 16px;
    background: transparent;
    border-radius: 30px;
    font-size: var(--font-size-s);

    & + .ui-modules-select-group
    {
      margin-left: 16px;
    }

    .ui-modules-select-group-count
    {
      display: inline-flex;
      justify-content: center;
      align-items: center;
      width: 20px;
      height: 20px;
      border-radius: 10px;
      background: var(--color-bg-dim);
      margin-left: 12px;
      font-size: 10px;
      line-height: 1;
      color: var(--color-fg-dim);
    }

    &.is-active
    {
      background: var(--color-bg-bright-two);

      .ui-modules-select-group-count
      {
        background: var(--color-bg-bright);
        color: var(--color-fg);
      }
    }
  }

  .ui-modules-select-items
  {
    display: grid;
    grid-gap: 10px;
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
    background: var(--color-bg-bright-two);
    padding: 10px var(--padding) 10px 0;
    position: relative;

    &[disabled]
    {
      opacity: .6;
    }

    &:hover
    {
      background: var(--color-bg-bright-three);
    }
  }

  .ui-modules-select-item-icon
  {
    display: inline-flex;
    justify-content: center;
    color: var(--color-fg-dim);
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
      color: var(--color-fg-dim);
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
    color: var(--color-fg-dim-two);
    line-height: 1;
    font-weight: 600;

    i
    {
      margin-left: 6px;
      font-size: 13px;
    }
  }
</style>