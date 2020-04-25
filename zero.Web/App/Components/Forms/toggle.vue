 <template>
  <div class="ui-toggle">
    <input type="checkbox" :value="value" @input="onChange" />
    <span class="ui-toggle-switch" :class="{ 'is-active': value }"><i></i></span>
  </div>
</template>


<script>
  export default {
    name: 'uiToggle',

    props: {
      value: {
        type: Boolean,
        default: false
      }
    },

    computed: {
      
    },

    methods: {

      onChange(ev)
      {
        this.$emit('input', !this.value);
      }
      //onChange(ev)
      //{
      //  this.$emit('change', ev.target.value);
      //},
    }
  }
</script>

<style lang="scss">
  .ui-toggle
  {
    display: inline-block;
    position: relative;
    height: 22px;
    cursor: pointer;

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
  }

  .ui-toggle-switch
  {
    display: inline-block;
    height: 22px;
    width: 40px;
    background: var(--color-box);
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
      background: var(--color-primary);
      border-color: transparent;

      i
      {
        background: var(--color-primary-fg);
        transform: translateX(18px);
      }
    }
  }
</style>