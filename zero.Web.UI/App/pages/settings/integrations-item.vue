<template>
  <div class="integrations-item" :class="{'is-activated': activated }">
    <i class="integrations-item-icon" :style="{'background-color': model.color ? model.color : null }" :class="[model.icon || 'fth-box', hasColor ? 'has-color' : '']" />
    <p class="integrations-item-text">
      <strong v-localize="model.name"></strong>
      <template v-if="model.description">
        <br>
        <span v-localize="model.description"></span>
      </template>
    </p>
    <router-link v-if="!activated" class="ui-button type-light type-onbg" :to="{ name: 'integrations-create', params: { alias: model.alias } }">
      <span class="ui-button-text" v-localize="'Setup'" />
    </router-link>
    <router-link v-else class="ui-button type-light type-onbg" :to="{ name: 'integrations-edit', params: { id: model.alias } }">
      <span class="ui-button-text" v-localize="'Edit'" />
    </router-link>
  </div>
</template>


<script>
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
        return !!this.model.color;
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