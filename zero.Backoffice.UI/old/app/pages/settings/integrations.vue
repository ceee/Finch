<template>
  <div class="integrations">
    <ui-header-bar title="@integration.list" :count="count" :back-button="true" />
    <div class="ui-box">
      <!--<h2 v-if="available.length > 0" class="ui-headline integrations-headline">Available</h2>-->
      <integration-item v-for="(item, index) in items" :key="index" :model="item" @change="onChanged" @onActiveChange="onActiveChanged" />
    </div>
  </div>
</template>


<script>
  import IntegrationsApi from 'zero/api/integrations.js';
  import IntegrationItem from './integrations-item.vue';
  import Notification from 'zero/helpers/notification.js'

  export default {
    data: () => ({
      count: 0,
      items: []
    }),

    components: { IntegrationItem },

    mounted()
    {
      this.load();
    },

    methods: {

      load()
      {
        IntegrationsApi.getTypes().then(res =>
        {
          this.items = res;
        });
      },

      onChanged()
      {
        this.load();
      },

      onActiveChanged(model)
      {
        model.isLoading = true;

        IntegrationsApi.saveActiveState({ alias: model.type.alias, isActive: model.isActive }).then(res =>
        {
          if (!res.success)
          {
            model.isActive = !model.isActive;
            Notification.error('@integration.errors.couldnotupdatestate', res.errors[0].message);
          }
          model.isLoading = false;
        });
      }
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