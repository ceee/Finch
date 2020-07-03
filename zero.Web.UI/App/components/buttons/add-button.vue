<template>
  <div class="ui-add-button">
    <ui-button v-if="!decision" :type="type" :label="label" icon="fth-plus" @click="onClick(false)" :disabled="disabled" />
    <ui-dropdown v-else ref="dropdown" align="right">
      <template v-slot:button>
        <ui-button :label="label" :type="type" icon="fth-plus" :disabled="disabled" />
      </template>
      <div class="ui-add-button-items">
        <button type="button" class="ui-add-button-item" @click="onClick(true)" :disabled="disabled">
          <i class="fth-radio is-primary"></i>
          <span class="-text">For all apps</span>
        </button>
        <span class="ui-add-button-items-line"></span>
        <button type="button" class="ui-add-button-item" @click="onClick(false)" :disabled="disabled">
          <i class="fth-layers"></i>
          <span class="-text">For {{application.name}}</span>
        </button>
      </div>
    </ui-dropdown>
  </div>
</template>


<script>
  import { find as _find } from 'underscore';

  export default {
    props: {
      decision: {
        type: Boolean,
        default: true
      },
      label: {
        type: String,
        default: '@ui.add'
      },
      type: {
        type: String,
        default: 'action'
      },
      route: {
        type: String,
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      }
    },

    computed: {
      application()
      {
        return _find(zero.applications, x => x.id === zero.appId);
      }
    },

    methods: {

      onClick(isShared)
      {
        this.$refs.dropdown.hide();

        if (!!this.route)
        {
          this.$router.push({
            name: this.route,
            params: { scope: isShared ? zero.sharedAppId : null }
          });
        }

        this.$emit('click', false);
      }

    }

  }
</script>

<style lang="scss">
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
      background: var(--color-bg-xxlight);
    }
  }

  .ui-add-button-items-line
  {
    display: block;
    height: 100%;
    background: var(--color-line-light);
  }
</style>