<template>
  <div class="settings">
    <div class="settings-group" v-for="group in groups">
      <h2 class="ui-headline settings-group-headline" v-localize="group.name"></h2>
      <div class="settings-group-items">
        <ui-link :to="item.url || '/'" v-for="item in group.items" :key="item.name" class="settings-group-item">
          <span class="settings-group-item-icon">
            <ui-icon :symbol="item.icon || 'fth-settings'" :size="18" />
            <span v-if="item.alias === 'applications'" class="settings-group-item-count">{{appCount}}</span>
          </span>
          <p class="settings-group-item-text">
            <strong v-localize="item.name"></strong>
            <template v-if="item.description">
              <br>
              <ui-localize :value="item.description" :tokens="tokens" />
            </template>
          </p>
        </ui-link>
      </div>
    </div>
    <div class="settings-footer" v-if="meta">
      <p>
        zero version:<br /><span class="-version">{{meta.zeroVersion}}</span>
      </p>
      <p>
        app version:<br /><span class="-version">{{meta.appVersion}}</span>
      </p>
      <p>
        date:<br /><span class="-version" v-date="meta.appLastModifiedDate"></span>
      </p>
    </div>
  </div>
</template>


<script>
  import { useUiStore } from '../../ui/store';
  import { useAppStore } from '../applications/store';

  export default {
    name: 'app-settings',

    data: () => ({
      page: true,
      appCount: 0,
      groups: [],
      meta: null,
      tokens: {
        'zero_version': '0.0.1',
        'plugin_count': 3
      }
    }),

    mounted()
    {
      this.meta = JSON.parse(document.getElementById('zero-meta').innerHTML);

      this.groups = useUiStore().settingGroups;
      this.appCount = useAppStore().applications.length;

      if (!this.groups[1].items.find(x => x.alias === 'demo'))
      {
        this.groups[1].items.push({
          alias: "demo",
          description: "Demo all editor components",
          icon: "fth-box",
          isPlugin: true,
          name: "Components",
          url: "/settings/demo"
        });
      }
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

    /*&:hover .settings-group-item-icon
    {
      transform: rotate(5deg);
    }*/
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
    transition: color 0.2s ease, transform 0.2s ease;
    position: relative;
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
      margin-bottom: 3px;
      color: var(--color-text);
    }
  }

  .settings-group-item-count
  {
    display: inline-block;
    font-size: 11px;
    font-weight: 700;
    background: var(--color-accent);
    color: var(--color-accent-fg);
    height: 22px;
    width: 22px;
    line-height: 22px;
    padding: 0 0;
    text-align: center;
    border-radius: 16px;
    position: absolute;
    top: -4px;
    right: -4px;
  }

  .settings-footer
  {
    font-size: var(--font-size-xs);
    color: var(--color-text-dim);
    display: flex;
    gap: var(--padding);

    p
    {
      margin: 0;
      line-height: 1.4;
    }

    .-version
    {
      color: var(--color-text);
    }
  }
</style>