<template>
  <div class="integrations-item" :class="{'is-configured': model.isConfigured }">
    <aside>
      <i class="integrations-item-icon" :style="{'background-color': hasColor ? model.type.color : null }" :class="[model.type.icon || 'fth-box', hasColor ? 'has-color' : '']"></i>
      <button type="button" v-if="!model.isConfigured" @click="open" class="ui-button type-primary type-block">
        <span class="ui-button-text" v-localize="'Setup'" />
      </button>
      <button type="button" v-else @click="open" class="ui-button type-primary type-block">
        <span class="ui-button-text" v-localize="'Edit'" />
      </button>
    </aside>
    <main>
      <p class="integrations-item-text">
        <strong v-localize="model.type.name"></strong>
        <i class="fth-check-circle" v-if="model.isConfigured"></i>
        <template v-if="model.type.description">
          <br>
          <span v-localize="model.type.description"></span>
        </template>
      </p>
      <div>
        <ui-toggle class="integrations-item-toggle" v-if="model.isConfigured" v-model="model.isActive" @input="$emit('onActiveChange', model)" on-content="Active" off-content="Active" />
      </div>
    </main>  
  </div>
</template>


<script>
  import IntegrationOverlay from './integration.vue';
  import Overlay from 'zero/helpers/overlay.js';

  export default {
    props: {
      model: {
        type: Object,
        default: () => { }
      }
    },

    data: () => ({
      active: false
    }),

    computed: {
      hasColor()
      {
        return !!this.model.type.color && this.model.isConfigured;
      }
    },

    methods: {
      open()
      {
        // open editing overlay
        return Overlay.open({
          component: IntegrationOverlay,
          display: 'editor',
          model: this.model.type,
          isCreate: !this.model.isConfigured,
          alias: this.model.type.alias,
          width: 960
        }).then(value =>
        {
          this.$emit('change', value);
        });
      }
    }
  }
</script>

<style lang="scss">
  .integrations-item
  { 
    color: var(--color-text);  
    font-size: var(--font-size);
    display: grid;
    grid-template-columns: auto 1fr;
    gap: var(--padding);
    align-items: flex-start; 
  }

  .integrations-item .ui-button.type-block
  {
    justify-content: center;
    margin-top: 5px;
  }

  .integrations-item + .integrations-item
  {
    margin-top: var(--padding-m);
    border-top: 1px solid var(--color-line);
    padding-top: var(--padding-m);
  }

  .integrations-item-icon
  {
    display: inline-block;
    width: 120px;
    height: 80px;
    line-height: 79px !important; 
    font-size: 22px;
    text-align: center;
    background: var(--color-box-nested);
    border-radius: var(--radius);  
  }

  .integrations-item-icon.has-color
  {
    color: white;
    box-shadow: none;
  }

  .integrations-item-text
  {
    line-height: 1.3;
    color: var(--color-text-dim);
    margin: 0.5em 0 0;
    max-width: 820px;
  } 

  .integrations-item-text i
  {
    color: var(--color-primary); 
    margin-left: 0.5em; 
    font-size: 1.1em;
  }

  .integrations-item-text strong
  {
    display: inline-block;
    margin-bottom: 5px;
    color: var(--color-text);
    font-size: var(--font-size);
  }

  .integrations-item-toggle
  {
    margin-top: var(--padding); 
  }
</style>