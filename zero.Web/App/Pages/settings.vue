<template>
  <div class="settings">
    <div class="settings-group" v-for="group in groups">
      <h2 class="ui-headline xl settings-group-headline" v-localize="group.name"></h2>
      <div class="settings-group-items">
        <router-link to="/" v-for="item in group.items" :key="item.name" class="settings-group-item">
          <i class="settings-group-item-icon" :class="item.icon || 'fth-settings'" />
          <p class="settings-group-item-text">
            <strong v-localize="item.name"></strong>
            <template v-if="item.text">
              <br>
              {{item.text | localize}}
            </template>
          </p>
        </router-link>
      </div>
    </div>
  </div>
</template>


<script>
  export default {
    name: 'app-settings',

    data: () => ({
      groups: []
    }),

    created()
    {
      this.groups.push({
        name: 'System',
        items: [
          {
            name: 'Updates',
            text: 'Version 1.0.0',
            icon: 'fth-check-circle'
          },
          {
            name: 'Applications',
            text: 'Edit website applications',
            icon: 'fth-layers'
          },
          {
            name: 'Users & Permissions',
            text: 'Administration of backoffice users',
            icon: 'fth-users'
          },
          {
            name: 'Translations',
            text: 'Frontend texts and translations',
            icon: 'fth-type'
          },
          {
            name: 'Countries',
            text: 'Manage list of countries',
            icon: 'fth-map-pin'
          },
          {
            name: 'Logging',
            text: 'Debug und view logs',
            icon: 'fth-file-text'
          },
        ]
      });
      
      this.groups.push({
        name: 'Plugins',
        items: [
          {
            name: 'Installed',
            text: '7 installed plugins',
            icon: 'fth-package'
          },
          {
            name: 'Create a plugin',
            text: 'Create from existing code',
            icon: 'fth-box'
          }
        ]
      });
    },


    methods: {

      onBack()
      {
        console.info('back');
      }

    }
  }
</script>


<style lang="scss">
  .settings
  {
    height: 100%;
    position: relative;
    padding: 95px 95px 0 95px;
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
    grid-gap: 40px;
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

    &:hover .settings-group-item-icon
    {
      border-color: var(--color-highlight);
    }
  }

  .settings-group-item-icon
  {
    width: 80px;
    height: 80px;
    line-height: 76px;
    font-size: 24px;
    text-align: center;
    background: var(--color-bg-mid);
    border: 2px solid transparent;
    border-radius: var(--radius);
    transition: border 0.2s ease;
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