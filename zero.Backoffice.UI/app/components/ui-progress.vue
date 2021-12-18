<template>
  <div class="ui-progress" :class="{'is-animated': animated }">
    <span class="-value" :style="{ transform: 'translateX(' + ((100 - value) * -1) + '%)' }"></span>
  </div>
</template>


<script>
  export default {
    name: 'uiProgress',

    props: {
      value: {
        type: Number,
        default: 0
      },
      animated: {
        type: Boolean,
        default: true
      }
    },

    computed: {
      progress()
      {
        return this.value > 100 ? 100 : (this.value < 0 ? 0 : this.value);
      }
    }

  }
</script>

<style lang="scss">
  .ui-progress
  {
    display: block;
    width: 100%;
    height: 12px;
    border-radius: 6px;
    background: var(--color-bg-shade-3);
    position: relative;
    overflow: hidden;
  }

  .ui-progress .-value
  {
    position: absolute;
    left: 0;
    top: 0;
    height: 100%;
    width: 100%;
    transform: translateX(-100%);
    border-radius: 6px;
    overflow: hidden;
    background: var(--color-accent);
    transition: transform 0.3s linear;
  }

  .ui-progress.is-animated .-value:before
  {
    --color-progress-overlay: #00000022;
    content: '';
    position: absolute;
    left: -20px;
    right: -20px;
    top: 0;
    bottom: 0;
    background-image: linear-gradient(-60deg, transparent 25%, var(--color-progress-overlay) 25%, var(--color-progress-overlay) 50%, transparent 50%, transparent 75%, var(--color-progress-overlay) 75%, var(--color-progress-overlay));
    background-size: 20px 30px;
    animation: uiProgressAnimation 1s linear infinite;
  }

  @keyframes uiProgressAnimation
  {
    from
    {
      transform: translateX(-20px);
    }

    to
    {
      transform: translateX(0);
    }
  }
</style>