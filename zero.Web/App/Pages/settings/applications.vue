<template>
  <div class="apps">
    <ui-header-bar title="@application.list" :back-button="true">
      <!--<ui-button label="Add" icon="fth-plus" />-->
    </ui-header-bar>
    <div class="ui-blank-box">
      <div class="apps-items">
        <router-link v-for="app in apps" :to="getAppLink(app)" class="apps-item">
          <!--<i class="apps-item-icon" :class="role.icon"></i>-->
          <strong>{{app.name}}</strong>
          <span class="apps-item-minor">{{app.alias}}</span>
        </router-link>
        <ui-button class="apps-items-add" label="Add" type="big" icon="fth-plus" />
      </div>
    </div>
  </div>
</template>


<script>
  import ApplicationsApi from 'zero/resources/applications.js';

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
          name: zero.alias.sections.settings + '-' + zero.alias.settings.applications + '-edit',
          params: { id: item.id }
        };
      },

    }
  }
</script>


<style lang="scss">
  .apps-items
  {
    display: block;
    grid-gap: var(--padding);
    margin-bottom: calc(var(--padding) * 2);
  }

  .apps-items-add
  {
    margin-top: var(--padding);
  }

  a.apps-item
  {
    display: flex;
    flex-direction: column;
    justify-content: center;
    background: var(--color-box);
    border-radius: var(--radius);
    padding: var(--padding-s) var(--padding);
    color: var(--color-fg);
    font-size: var(--font-size);
    line-height: 1.5;
    transition: box-shadow 0.2s ease;

    &:hover
    {
      box-shadow: 0 0 20px var(--color-shadow);
    }
  }

  .apps-item-minor
  {
    color: var(--color-fg-mid);
  }

  .apps-item-icon
  {
    font-size: 26px;
    display: inline-block;
    margin: 0 auto var(--padding-s);
    position: relative;
  }
</style>