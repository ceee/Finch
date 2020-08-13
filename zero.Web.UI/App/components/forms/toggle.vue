 <template>
  <div class="ui-toggle" :class="{'is-disabled': disabled, 'is-negative': negative, 'is-active': value, 'is-content-left': contentLeft }">
    <input type="checkbox" :value="value" @input="onChange" :disabled="disabled" />
    <span class="ui-toggle-switch" :class="{ 'is-active': value }"><i></i></span>
    <span class="ui-toggle-text" v-if="onContent && value" v-localize="onContent"></span>
    <span class="ui-toggle-text" v-if="offContent && !value" v-localize="offContent"></span>
  </div>
</template>


<script>
  export default {
    name: 'uiToggle',

    props: {
      value: {
        type: Boolean,
        default: false
      },
      disabled: {
        type: Boolean,
        default: false
      },
      negative: {
        type: Boolean,
        default: false
      },
      onContent: {
        type: String,
        default: null
      },
      offContent: {
        type: String,
        default: null
      },
      contentLeft: {
        type: Boolean,
        default: false
      }
    },

    methods: {

      onChange(ev)
      {
        this.$emit('input', !this.value);
      }
    }
  }
</script>

<style lang="scss">
  .ui-toggle
  {
    display: inline-flex;
    align-items: center;
    position: relative;
    height: 22px;

    input
    {
      position: absolute;
      top: 0;
      left: 0;
      right: 0;
      bottom: 0;
      width: 100%;
      height: 100%;
      margin: 0;
      z-index: 2;
      opacity: 0;
      cursor: pointer;
    }

    &.is-disabled input
    {
      cursor: default;
    }

    &.is-content-left
    {
      flex-direction: row-reverse;

      .ui-toggle-text
      {
        margin-left: 0;
        margin-right: 12px;
      }
    }
  }

  .ui-toggle-text
  {
    margin-top: 1px;
    margin-left: 12px;

    .ui-toggle.is-active &
    {
      font-weight: 600;
    }
  }

  .ui-toggle-switch
  {
    display: inline-block;
    height: 22px;
    width: 36px;
    background: var(--color-bg);
    border: 1px solid var(--color-line);
    border-radius: 20px;
    transition: all 0.2s ease;
    z-index: 1;
    pointer-events: none;
    i
    {
      display: inline-block;
      height: 16px;
      width: 16px;
      border-radius: 20px;
      margin: 2px;
      background: var(--color-bg-dark);
      transition: all 0.2s ease;
    }

    &.is-active
    {
      /*background: var(--color-secondary);
      transform: translateX(18px);*/
      background: var(--color-secondary);
      border-color: transparent;

      i
      {
        background: var(--color-secondary-fg);
        transform: translateX(14px);
      }

      /*.is-primary &
      {
        background: var(--color-primary);
        box-shadow: 0 0 0 4px var(--color-primary-low);
      }*/
    }
  }

  .ui-toggle.is-negative .ui-toggle-switch.is-active
  {
    background: var(--color-negative);
  }

  /*.ui-toggle.is-disabled .ui-toggle-switch i
  {
    background: var(--color-bg-dark);
  }*/
</style>