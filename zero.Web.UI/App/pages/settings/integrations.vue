<template>
  <div class="integrations">
    <ui-header-bar title="@integration.list" :count="count" :back-button="true" />
    <div class="ui-box" v-if="activated.length > 0">
      <h2 class="ui-headline integrations-headline">Activated</h2>
      <integration-item v-for="(item, index) in activated" :key="'x' + index" :model="item" :activated="true" />
    </div>
    <div class="ui-box is-light" v-if="available.length > 0">
      <h2 v-if="activated.length > 0" class="ui-headline integrations-headline">Available</h2>
      <integration-item v-for="(item, index) in available" :key="index" :model="item" />
    </div>
  </div>
</template>


<script>
  import IntegrationsApi from 'zero/api/integrations.js';
  import IntegrationItem from './integrations-item.vue';

  export default {
    data: () => ({
      count: 0,
      activated: [],
      available: []
    }),

    components: { IntegrationItem },

    mounted()
    {
      IntegrationsApi.getAll().then(res =>
      {
        this.activated = res.activated;
        this.available = res.available;
      });
    }
  }
</script>

<style lang="scss">
  h2.ui-headline.integrations-headline
  {
    font-family: var(--font);
    color: var(--color-text);
    margin: 0 0 var(--padding);
    font-size: var(--font-size-l);
    font-weight: 700;
    padding-bottom: var(--padding-m);
    border-bottom: 1px dashed var(--color-line-dashed); 
  }
</style>