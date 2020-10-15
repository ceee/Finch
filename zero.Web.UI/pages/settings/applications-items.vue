<template>
  <div class="apps-items">
    <router-link v-for="app in apps" :key="app.id" :to="app.link" class="apps-item">
      <strong class="apps-item-name">{{app.name}}</strong>
      <span class="apps-item-minor">{{app.domains[0]}}</span>
      <span class="apps-item-status" :class="{ 'is-active': app.isActive }" v-localize="app.statusLocalization"></span>
    </router-link>
    <router-link :to="{ name: addLink }" class="apps-items-add">
      <i class="fth-plus"></i>
    </router-link>
  </div>
</template>


<script>
  import { ref, computed, watch } from 'vue';

  export default {

    props: {
      items: Array
    },

    setup(props)
    {
      const baseRoute = zero.alias.sections.settings + '-' + zero.alias.settings.applications;
      const addLink = computed(() => baseRoute + '-create');
      const apps = ref();

      const buildItems = items =>
      {
        apps.value = props.items.map(x =>
        {
          return {
            ...x,
            link: {
              name: baseRoute + '-edit',
              params: { id: x.id }
            },
            statusLocalization: x.isActive ? '@ui.active' : '@ui.inactive'
          };
        });
      };

      buildItems(props.items);
      watch(() => props.items, val => buildItems(val));

      return {
        apps,
        addLink
      };
    }
  }
</script>


<style lang="scss">
  .apps-items
  {
    display: grid;
    gap: var(--padding);
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
    padding: var(--padding-m);
    text-align: center;
    color: var(--color-text);
    font-size: var(--font-size);
    line-height: 1.5;
    box-shadow: var(--shadow-short);
  }

  .apps-item-name
  {
  }

  .apps-item-minor
  {
    color: var(--color-text-dim);
  }

  .apps-item-image
  {
    text-align: center;
    display: inline-block;
    margin: 0 auto var(--padding-m);
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
    background: var(--color-box-nested);
    color: var(--color-text);
    height: 22px;
    line-height: 22px;
    padding: 0 10px;
    border-radius: 16px;
    letter-spacing: .5px;
  }

  .apps-items-add
  {
    background: transparent;
    border: 1px dashed var(--color-line-dashed);
    color: var(--color-text);
    border-radius: var(--radius);
    text-align: center;
    display: inline-flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    font-size: 22px;
    width: 60px;
  }
</style>