<template>
  <div class="ui-add-button">
    <ui-button v-if="!component" :type="type" :label="label" @click="onClick" :disabled="disabled" /> <!-- :attach="true"-->
    <component v-else :is="component" :type="type" :label="label" :disabled="disabled" :route="route" @click="onClick" />
  </div>
</template>


<script>
  import { find as _find } from 'underscore';

  export default {
    props: {
      label: {
        type: String,
        default: '@ui.add'
      },
      type: {
        type: String,
        default: 'primary'
      },
      route: {
        type: [String, Object],
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      }
    },

    data: () => ({
      component: null
    }),

    created()
    {
      this.component = zero.overrides['add-button'] || null;
    },

    methods: {

      onClick()
      {
        if (this.$refs.dropdown)
        {
          this.$refs.dropdown.hide();
        }

        if (!!this.route)
        {
          let routeObj = typeof this.route === 'object' ? this.route : { name: this.route };
          this.$router.push(routeObj);
        }

        this.$emit('click', false);
      }

    }

  }
</script>

<style lang="scss">
  .ui-add-button
  {
    display: flex;
  }
  /*.ui-add-button .ui-dropdown-button
  {
    font-weight: 700;
  }*/

  .ui-add-button-items
  {
    display: grid;
    grid-template-columns: 1fr 1px 1fr;
  }

  .ui-add-button-item
  {
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    padding: 20px 10px;
    font-size: var(--font-size);
    border-radius: var(--radius);

    i
    {
      font-size: 20px;
      line-height: 24px;
      margin-bottom: 12px;
    }

    .is-primary
    {
      font-size: 24px;
      color: var(--color-primary);
    }

    &:hover
    {
      background: var(--color-button-light);
    }
  }

  .ui-add-button-items-line
  {
    display: block;
    height: 100%;
    background: var(--color-line);
  }
</style>