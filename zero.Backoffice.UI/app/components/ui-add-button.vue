<template>

  <ui-button v-if="!hasDropdown" :type="type" :label="label" @click="onConfirm(null, false)" :disabled="disabled" />

  <ui-dropdown v-else ref="dropdown" align="right">

    <template v-slot:button>
      <ui-button :label="label" :type="type" :disabled="disabled" />
    </template>

    <template v-if="flavors.length > 0">
      <ui-dropdown-button v-for="(flavor, index) in flavors" :key="index" :icon="flavor.icon" :value="flavor" :label="flavor.name" :prevent="true" @click="selectFlavor" :selected="selectedflavor == flavor.alias" />
      <ui-dropdown-separator />
      <div class="ui-decision">
        <ui-toggle v-model:on="asBlueprint" :on-content="'Shared'" :off-content="'Not shared'" />
      </div>
      <ui-dropdown-separator />
      <div class="ui-decision-button">
        <ui-button label="@addoverlay.gotoeditor" type="accent" icon="fth-arrow-right" :disabled="disabled" @click="onConfirm(selectedflavor, asBlueprint)" />
      </div>
    </template>

    <div v-else class="ui-add-button-items">
      <button type="button" class="ui-add-button-item" @click="onConfirm(null, true)" :disabled="disabled">
        <ui-icon symbol="fth-cloud" :size="20" />
        <span class="-text">Shared</span>
      </button>
      <span class="ui-add-button-items-line"></span>
      <button type="button" class="ui-add-button-item" @click="onConfirm(null, false)" :disabled="disabled">
        <ui-icon symbol="fth-arrow-right" :size="20" />
        <span class="-text">Local</span>
      </button>
    </div>

  </ui-dropdown>
</template>


<script lang="ts">
  import { useUiStore } from '../ui/store';
  import { defineComponent } from 'vue';
  import { UiFlavorProvider } from 'zero/ui';

  export default defineComponent({
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
      alias: {
        type: String,
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      }
    },

    data: () => ({
      flavors: [],
      loading: false,
      asBlueprint: false,
      allowBlueprint: false,
      selectedflavor: null,
      store: null
    }),

    computed: {
      hasDropdown()
      {
        return this.flavors.length > 0 || this.allowBlueprint;
      }
    },

    async created()
    {
      this.store = useUiStore();
      this.buildFlavors();
    },

    methods: {

      buildFlavors()
      {
        let flavors = [];
        let flavorConfig = null;

        if (this.alias)
        {
          flavorConfig = this.store.flavors[this.alias];
        }

        this.allowBlueprint = this.store.blueprints.indexOf(this.alias) > -1;

        flavorConfig = flavorConfig || {
          canUseWithoutFlavors: true,
          flavors: []
        } as UiFlavorProvider;

        flavors = JSON.parse(JSON.stringify(flavorConfig.flavors));

        if (flavorConfig.canUseWithoutFlavors && flavors.length > 0)
        {
          flavors.splice(0, 0, {
            name: 'Default',
            description: 'Create the default entity',
            alias: null,
            icon: 'fth-box'
          });
        }

        this.flavors = flavors;
      },


      selectFlavor(flavor, opts)
      {
        this.selectedflavor = flavor.alias;
      },


      onConfirm(flavor, shared)
      {
        if (!!this.route)
        {
          let routeObj = typeof this.route === 'object' ? this.route : { name: this.route };
          routeObj.query = routeObj.query || {};
          if (flavor)
          {
            routeObj.query['zero.flavor'] = flavor;
          }
          if (shared)
          {
            routeObj.query['zero.shared'] = true;
          }
          this.$router.push(routeObj);
        }

        this.$emit('click', { flavor, shared });
      }

    }

  })
</script>

<style>
  
  .ui-decision
  {
    display: flex;
    flex-direction: column;
    gap: 12px;
    padding: 16px 16px;
    font-size: var(--font-size);
  }

  .ui-decision-button
  {
    padding: 16px 16px;
  }

  .ui-dropdown.theme-dark .ui-decision 
  {
    .ui-toggle-switch:not(.is-active)
    {
      background: var(--color-bg);
    }

    input:focus + .ui-toggle-switch
    {
      border-color: transparent;
      box-shadow: none;
    }
  }

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