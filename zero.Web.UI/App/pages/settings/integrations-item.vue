<template>
  <div class="integrations-item" :class="{'is-activated': activated }">
    <i class="integrations-item-icon" :style="{'background-color': hasColor ? model.color : null }" :class="[model.icon || 'fth-box', hasColor ? 'has-color' : '']" />
    <p class="integrations-item-text">
      <strong v-localize="model.name"></strong>
      <template v-if="model.description">
        <br>
        <span v-localize="model.description"></span>
      </template>
    </p>
    <button type="button" v-if="!activated" @click="open" class="ui-button type-light" :class="{ 'type-onbg': !activated }">
      <span class="ui-button-text" v-localize="'Setup'" />
    </button>
    <button type="button" v-else @click="open" class="ui-button type-light" :class="{ 'type-onbg': !activated }">
      <span class="ui-button-text" v-localize="'Edit'" />
    </button>
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
      },
      activated: {
        type: Boolean,
        default: false
      }
    },

    computed: {
      hasColor()
      {
        return !!this.model.color && this.activated;
      }
    },

    methods: {
      open()
      {
        // open editing overlay
        return Overlay.open({
          component: IntegrationOverlay,
          display: 'editor',
          model: this.model,
          isCreate: !this.activated,
          alias: this.model.alias,
          width: 960
        }).then(value =>
        {
          console.log(value);
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
    grid-template-columns: auto 1fr auto;
    gap: 20px;
    align-items: center;
  }

  .integrations-item + .integrations-item
  {
    margin-top: var(--padding-m);
  }

  .integrations-item-icon
  {
    width: 100px;
    height: 80px;
    line-height: 79px !important;
    font-size: 22px;
    text-align: center;
    background: var(--color-box); 
    border-radius: var(--radius);
    box-shadow: var(--shadow-short);
  }

  .integrations-item.is-active .integrations-item-icon
  {
    opacity: 1;
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
    margin: 0;
  }

  .integrations-item-text strong
  {
    display: inline-block;
    margin-bottom: 5px;
    color: var(--color-text);
    font-size: var(--font-size);
  }
</style>