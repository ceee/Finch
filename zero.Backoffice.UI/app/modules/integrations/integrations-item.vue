<template>
  <div class="integrations-item" :class="{'is-configured': model.isConfigured }">
    <aside>
      <span class="integrations-item-icon" :style="{'background-color': hasColor ? model.color : null }">
        <ui-icon :symbol="model.icon || 'fth-box'" :size="26" />
      </span>
    </aside>
    <main>
      <p class="integrations-item-text">
        <strong v-localize="model.name"></strong>
        <template v-if="model.description">
          <br>
          <span v-localize="model.description"></span>
        </template>
      </p>
      <!--<div class="integrations-item-tags">
        <span class="ui-tag" v-for="tag in model.tags">{{tag}}</span>
      </div>-->
      <div class="integrations-item-bottom">
        <ui-button class="integrations-item-button" type="action small" v-if="!model.isConfigured" v-localize="'Setup'" @click="open" />
        <ui-button class="integrations-item-button" type="action small" v-else v-localize="'Edit'" @click="open" />
        <span class="integrations-item-active" v-if="model.isConfigured && model.isActivated">
          <ui-icon symbol="fth-check-circle" :size="16" /> <span>Active</span>
        </span>
      </div>
    </main>  
  </div>
</template>


<script>
  import * as overlays from '../../services/overlay';

  export default {
    props: {
      model: {
        type: Object,
        default: () => { }
      }
    },

    data: () => ({
      active: false
    }),

    computed: {
      hasColor()
      {
        return !!this.model.color && this.model.isConfigured;
      }
    },

    methods: {
      async open()
      {
        const result = await overlays.open({
          component: () => import('./integration.vue'),
          display: 'editor',
          model: this.model,
          width: 960
        });

        if (result.eventType === 'confirm')
        {
          this.$emit('change', result.value);
        }
      }
    }
  }
</script>

<style lang="scss">
  .integrations-item
  { 
    color: var(--color-text);  
    font-size: var(--font-size);
    display: grid;
    grid-template-columns: auto 1fr;
    gap: var(--padding);
    align-items: flex-start; 

    > aside
    {
      align-self: stretch;
    }
  }

  .integrations-item .ui-button.type-block
  {
    justify-content: center;
    margin-top: 5px;
  }

  .integrations-item + .integrations-item
  {
    margin-top: var(--padding-m);
    border-top: 1px solid var(--color-line);
    padding-top: var(--padding-m);
  }

  .integrations-item-icon
  {
    display: inline-flex;
    justify-content: center;
    align-items: center;
    width: 120px;
    min-height: 90px;
    height: 100%;
    font-size: 22px;
    text-align: center;
    background: var(--color-box-nested);
    border-radius: var(--radius);
  }

  .integrations-item-icon.has-color
  {
    color: white;
    box-shadow: none;
  }

  .integrations-item-text
  {
    line-height: 1.3;
    color: var(--color-text-dim);
    margin: 0;
    max-width: 820px;
  } 

  .integrations-item-text strong
  {
    display: inline-block;
    margin-bottom: 5px;
    color: var(--color-text);
    font-size: var(--font-size);
  }

  .integrations-item-active
  {
    margin-left: var(--padding-s);
    display: inline-flex;
    align-items: center;
    font-weight: 700;
    color: var(--color-accent);

    .ui-icon
    {
      margin-right: var(--padding-xxs);
    }
  }

  .integrations-item-bottom
  {
    margin-top: var(--padding-m);
    display: flex;
    align-items: center;
  }
</style>