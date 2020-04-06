<template>
  <div class="setup-step setup-step-finish">
    <div class="setup-step-finish-inner">
      <img src="/Icons/bird.svg" class="bird" alt="bird" width="100" />
      <h2>You are done!</h2>
    </div>
    <ui-button label="Continue to zero" type="big" />
  </div>
</template>


<script>
  export default {
    name: 'setupStepFinish',

    mounted ()
    {
      
    },

    methods: {

    }
  }

</script>


<style lang="scss">
  .setup-step-finish
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