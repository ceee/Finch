<template>
  <div class="settings">
    <div class="settings-group">
      <h2 class="ui-headline xl settings-group-headline" v-localize="'@application.list'"></h2>
      <applications-items />
    </div>
    <div class="settings-group" v-for="group in groups">
      <h2 class="ui-headline xl settings-group-headline" v-localize="group.name"></h2>
      <div class="settings-group-items">
        <router-link :to="item.url || '/'" v-for="item in group.items" :key="item.name" class="settings-group-item">
          <i class="settings-group-item-icon" :class="item.icon || 'fth-settings'" />
          <p class="settings-group-item-text">
            <strong v-localize="item.name"></strong>
            <template v-if="item.description">
              <br>
              {{item.description | localize({ tokens: tokens })}}
            </template>
          </p>
        </router-link>
      </div>
    </div>
    <router-view name="footer"></router-view>
  </div>
</template>


<script>
  import ApplicationsItems from 'zero/pages/settings/applications-items'

  export default {
    name: 'app-settings',

    components: { ApplicationsItems },

    data: () => ({
      page: true,
      groups: zero.settingsAreas,
      tokens: {
        'zero_version': '1.0.0-alpha.1',
        'plugin_count': 7
      }
    })
  }
</script>


<style lang="scss">
  .settings
  {
    height: 100%;
    position: relative;
    padding: 95px;
    width: 100%;
    max-width: 1600px;
  }

  .settings-group
  {
    & + .settings-group
    {
      margin-top: 80px;
    }
  }

  .settings-group-items
  {
    display: grid;
    grid-gap: 30px;
    grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
    align-items: stretch;
    margin-top: 40px;
  }

  a.settings-group-item
  {
    color: var(--color-text);
    font-size: var(--font-size);
    display: grid;
    grid-template-columns: auto 1fr;
    grid-gap: 25px;
    align-items: center;
  }

  .settings-group-item-icon
  {
    width: 70px;
    height: 70px;
    line-height: 68px !important;
    font-size: 24px;
    text-align: center;
    background: var(--color-bg-light);
    border-radius: var(--radius);
    transition: box-shadow 0.2s ease;
    box-shadow: var(--color-shadow-short);
  }

  .settings-group-item-text
  {
    line-height: 1.3;
    color: var(--color-fg-light);

    strong
    {
      display: inline-block;
      margin-bottom: 5px;
      color: var(--color-fg);
    }
  }
</style>