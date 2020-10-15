<template>
  <div class="settings">
    <div class="settings-group" v-if="false && apps.length > 0">
      <h2 class="ui-headline settings-group-headline" v-localize="'@application.list'"></h2>
      <applications-items :items="apps" />
    </div>
    <div class="settings-group" v-for="group in groups">
      <h2 class="ui-headline settings-group-headline" v-localize="group.name"></h2>
      <div class="settings-group-items">
        <router-link :to="item.url || '/'" v-for="item in group.items" :key="item.name" class="settings-group-item">
          <i class="settings-group-item-icon" :class="item.icon || 'fth-settings'" />
          <p class="settings-group-item-text">
            <strong v-localize="item.name"></strong>
            <template v-if="item.description">
              <br>
              <span v-localize="{ key: item.description, tokens: tokens }"></span>
            </template>
          </p>
        </router-link>
      </div>
    </div>
    <router-view name="footer"></router-view>
  </div>
</template>


<script>
  import ApplicationsItems from '@zero/pages/settings/applications-items.vue';
  import SettingsApi from '@zero/resources/settings.js';
  import { ref, onBeforeMount } from 'vue';

  export default {
    components: { ApplicationsItems },

    setup()
    {
      const groups = ref();
      const apps = ref();
      const tokens = ref({
        'zero_version': zero.version,
        'plugin_count': zero.pluginCount
      });

      onBeforeMount(async () =>
      {
        const areas = await SettingsApi.getAreas();
        groups.value = areas.groups;
        apps.value = areas.applications;
      });

      return {
        groups,
        apps,
        tokens
      }
    }
  }
</script>


<style lang="scss">
  .settings
  {
    min-height: 100%;
    position: relative;
    padding: 45px var(--padding);
    width: 100%;
    max-width: 2000px;
    display: grid;
    gap: 80px 40px;
    grid-template-columns: repeat(auto-fill, minmax(400px, 1fr));
    padding-left: 80px;
  }

  .settings-group-items
  {
    display: grid;
    gap: 20px;
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
    gap: 20px;
    align-items: center;
  }

  .settings-group-item-icon
  {
    width: 60px;
    height: 60px;
    line-height: 59px !important;
    font-size: 18px;
    text-align: center;
    background: var(--color-box);
    border-radius: var(--radius);
    box-shadow: var(--shadow-short);
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