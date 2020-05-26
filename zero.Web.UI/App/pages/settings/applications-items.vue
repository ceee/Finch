<template>
  <div class="apps-items">
    <router-link v-for="app in value" :key="app.id" :to="getAppLink(app)" class="apps-item">
      <strong class="apps-item-name">{{app.name}}</strong>
      <span class="apps-item-minor">{{app.domains[0]}}</span>
      <span class="apps-item-status" :class="{ 'is-active': app.isActive }" v-localize="getStatus(app)"></span>
    </router-link>
    <router-link :to="getAddLink()" class="apps-items-add">
      <i class="fth-plus"></i>
    </router-link>
  </div>
</template>


<script>
  const baseRoute = zero.alias.sections.settings + '-' + zero.alias.settings.applications;

  export default {
    props: {
      value: {
        type: Array,
        default: () => []
      }
    },

    methods: {

      getAppLink(item)
      {
        return {
          name: baseRoute + '-edit',
          params: { id: item.id }
        };
      },

      getStatus(item)
      {
        return item.isActive ? '@ui.active' : '@ui.inactive';
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
    grid-template-columns: repeat(auto-fill, minmax(240px, 1fr));
    align-items: stretch;
    margin-top: 40px;
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

  .apps-item-status
  {
    align-self: center;
    display: inline-block;
    font-size: 9px;
    font-weight: 700;
    margin-top: 15px;
    text-transform: uppercase;
    background: var(--color-bg);
    color: var(--color-fg);
    height: 22px;
    line-height: 22px;
    padding: 0 10px;
    border-radius: 16px;
    letter-spacing: .5px;

    &.is-active
    {
      background: var(--color-bg);
      color: var(--color-fg);
    }
  }

  .apps-items-add
  {
    background: transparent;
    color: var(--color-fg);
    border-radius: var(--radius);
    text-align: center;
    display: inline-flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    font-size: 22px;
    width: 60px;
    margin-left: -20px;

    &:hover
    {
      background: var(--color-bg-xmid);
    }
  }
</style>