<template>
  <div class="apps">
    <ui-header-bar title="@application.list" :back-button="true">
      <!--<ui-button label="Add" icon="fth-plus" />-->
    </ui-header-bar>
    <div class="ui-blank-box">
      <div class="apps-items">
        <router-link v-for="app in apps" :to="getAppLink(app)" class="apps-item">
          <img class="apps-item-image" :src="app.image" :alt="app.name" />
          <strong class="apps-item-name">{{app.name}}</strong>
          <span class="apps-item-minor">{{app.domains[0]}}</span>
        </router-link>
        <router-link :to="getAddLink()" class="apps-items-add">
          <i class="fth-plus"></i>
        </router-link>
      </div>
    </div>
  </div>
</template>


<script>
  import ApplicationsApi from 'zero/resources/applications.js';

  const baseRoute = zero.alias.sections.settings + '-' + zero.alias.settings.applications;

  export default {
    data: () => ({
      apps: []
    }),

    created()
    {
      ApplicationsApi.getAll().then(response =>
      {
        this.apps = response.items;
      });
    },

    methods: {

      getAppLink(item)
      {
        return {
          name: baseRoute + '-edit',
          params: { id: item.id }
        };
      },

      getAddLink()
      {
        return { name: baseRoute + '-create' };
      }

    }
  }
</script>


<style lang="scss">
  .apps-items
  {
    display: grid;
    grid-gap: var(--padding);
    grid-template-columns: repeat(auto-fill, minmax(200px, 1fr));
    align-items: stretch;
    margin-bottom: calc(var(--padding) * 2);
  }

  a.apps-item
  {
    display: flex;
    flex-direction: column;
    background: var(--color-box);
    border-radius: var(--radius);
    padding: var(--padding-s);
    text-align: center;
    color: var(--color-fg);
    font-size: var(--font-size);
    line-height: 1.5;
    transition: box-shadow 0.2s ease;
    box-shadow: var(--color-shadow-short);

    &:hover
    {
      box-shadow: 0 0 20px var(--color-shadow);
    }
  }

  .apps-item-name
  {
    border-top: 1px solid var(--color-line-light);
    padding-top: 20px;
  }

  .apps-item-minor
  {
    color: var(--color-fg-mid);
  }

  .apps-item-image
  {
    text-align: center;
    display: inline-block;
    margin: 0 auto var(--padding-s);
    position: relative;
    max-width: 120px;
    max-height: 50px;
  }

  .apps-items-add
  {
    background: var(--color-bg-xmid);
    color: var(--color-fg);
    border-radius: var(--radius);
    text-align: center;
    display: inline-flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    font-size: 24px;
    width: 100px;
  }
</style>