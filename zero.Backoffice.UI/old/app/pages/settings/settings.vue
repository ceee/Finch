<template>
  <div class="settings">
    <div class="settings-group" v-if="false && apps.length > 0">
      <h2 class="ui-headline settings-group-headline" v-localize="'@application.list'"></h2>
      <applications-items v-model="apps" />
    </div>
    <div class="settings-group" v-for="group in groups">
      <h2 class="ui-headline settings-group-headline" v-localize="group.name"></h2>
      <div class="settings-group-items">
        <router-link :to="item.url || '/'" v-for="item in group.items" :key="item.name" class="settings-group-item">
          <span class="settings-group-item-icon">
            <ui-icon :symbol="item.icon || 'fth-settings'" :size="18" />
          </span>
          <p class="settings-group-item-text">
            <strong v-localize="item.name"></strong>
            <template v-if="item.description">
              <br>
              <ui-localize :value="item.description" :tokens="tokens" />
            </template>
          </p>
        </router-link>
      </div>
    </div>
    <router-view name="footer"></router-view>
  </div>
</template>


<script>
  import ApplicationsItems from 'zero/pages/settings/applications-items.vue'
  import SettingsApi from 'zero/api/settings.js';

  export default {
    name: 'app-settings',

    components: { ApplicationsItems },

    data: () => ({
      page: true,
      groups: [],
      apps: [],
      tokens: {
        'zero_version': zero.version,
        'plugin_count': zero.pluginCount
      }
    }),

    mounted()
    {
      SettingsApi.getAreas().then(response =>
      {
        this.groups = response.groups;
        this.apps = response.applications;
      });
    }
  }
</script>


<style lang="scss">
  .settings
  {
    min-height: 100%;
    position: relative;
    padding: 40px 80px;
    width: 100%;
    max-width: 2000px;
    display: flex;
    flex-direction: column;
    gap: 80px 40px;
  }

  .settings-group-items
  {
    display: grid;
    gap: 20px;
    grid-template-columns: repeat(auto-fill, minmax(320px, 1fr));
    align-items: stretch;
    margin-top: var(--padding-m);
  }

  a.settings-group-item
  {
    color: var(--color-text);
    font-size: var(--font-size);
    display: grid;
    grid-template-columns: auto 1fr;
    gap: 20px;
    align-items: center;
  }

  .settings-group-item-icon
  {
    display: inline-flex;
    justify-content: center;
    align-items: center;
    width: 54px;
    height: 54px;
    background: var(--color-box);
    border-radius: var(--radius);
    box-shadow: var(--shadow-short);
    color: var(--color-text);
    transition: color 0.2s ease;
  }

  a.settings-group-item:hover .settings-group-item-icon
  {
    color: var(--color-text);
  }

  .settings-group-item-text
  {
    line-height: 1.3;
    color: var(--color-text-dim);
    margin: 0;

    strong
    {
      display: inline-block;
      margin-bottom: 5px;
      color: var(--color-text);
    }
  }
</style>