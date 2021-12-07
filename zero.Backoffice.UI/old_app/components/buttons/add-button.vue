<template>
  <div class="ui-add-button">
    <ui-button v-if="!blueprintsEnabled" :type="type" :label="label" @click="onClick(false)" :disabled="disabled" /> <!-- :attach="true"-->
    <ui-dropdown v-else ref="dropdown" align="right">
      <template v-slot:button>
        <ui-button :label="label" :type="type" :disabled="disabled" />
      </template>
      <div class="ui-add-button-items">
        <button type="button" class="ui-add-button-item" @click="onClick(true)" :disabled="disabled">
          <ui-icon symbol="fth-cloud" :size="20" />
          <span class="-text">All apps</span>
        </button>
        <span class="ui-add-button-items-line"></span>
        <button type="button" class="ui-add-button-item" @click="onClick(false)" :disabled="disabled">
          <ui-icon symbol="fth-arrow-right" :size="20" />
          <span class="-text">{{application.name}}</span>
        </button>
      </div>
    </ui-dropdown>
  </div>
</template>


<script>
  import { find as _find } from 'underscore';

  export default {
    props: {
      label: {
        type: String,
        default: '@ui.add'
      },
      type: {
        type: String,
        default: 'accent'
      },
      route: {
        type: [String, Object],
        default: null
      },
      blueprintAlias: {
        type: String,
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      }
    },

    data: () => ({
      component: null
    }),

    created()
    {
      this.component = zero.overrides['add-button'] || null;
    },

    computed: {
      application()
      {
        return this.zero.config.applications.find(x => x.id === this.zero.config.appId);
      },
      blueprintsEnabled()
      {
        let blueprint = this.zero.config.blueprints.find(x => x.alias == this.blueprintAlias);
        return this.blueprintAlias && blueprint && blueprint.enabled;
      }
    },

    methods: {

      onClick(createBlueprint)
      {
        if (this.$refs.dropdown)
        {
          this.$refs.dropdown.hide();
        }

        if (!!this.route)
        {
          let routeObj = typeof this.route === 'object' ? this.route : { name: this.route };
          routeObj.query = routeObj.query || {};

          if (createBlueprint)
          {
            routeObj.query.scope = 'shared';
          }

          this.$router.push(routeObj);
        }

        this.$emit('click', false);
      }

    }

  }
</script>

<style>
  .ui-add-button
  {
    display: flex;
  }

  .ui-add-button-items
  {
    display: grid;
    grid-template-columns: 1fr 1px 1fr;
  }

  .ui-add-button-item
  {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    padding: 20px 10px;
    font-size: var(--font-size);
    border-radius: var(--radius);
  }

  .ui-add-button-item .ui-icon
  {
    margin-bottom: 12px;
  }

  .ui-add-button-item .is-primary
  {
    font-size: 24px;
    color: var(--color-primary);
  }

  .ui-add-button-item:hover
  {
    background: var(--color-dropdown-selected);
  }

  .ui-add-button-items-line
  {
    display: block;
    height: 100%;
    background: var(--color-line);
  }
</style>