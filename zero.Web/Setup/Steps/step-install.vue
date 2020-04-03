<template>
  <div class="setup-step setup-step-install">
    <div class="setup-step-finish-inner">
      <img src="/Icons/bird.svg" class="bird" alt="bird" width="100" />
      <h2>Installing</h2>
    </div>
  </div>
</template>


<script>
  import UiButton from 'zerocomponents/buttons/button.vue'

  export default {
    name: 'setupStepInstall',

    components: { UiButton },

    mounted ()
    {
      
    },

    methods: {

    }
  }

</script>


<style lang="scss">
  .setup-step-install
  {
    display: grid;
    grid-template-rows: 1fr auto;
    align-items: center;
    justify-content: center;
    text-align: center;

    .bird
    {
      margin-bottom: 30px;
    }
  }
</style>