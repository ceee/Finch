<template>
  <div class="app setup">
    <component v-bind:is="steps[step]" v-model="model" />
    <div class="setup-buttons" v-if="step < 3">
      <ui-button v-if="step > 0" type="outline" label="Go back" :click="prev" caret="left" caret-position="left" />
      <ui-button label="Next" :click="next" />
    </div>
  </div>
</template>


<script>
  import Sass from '../Sass/setup.scss'
  import UiButton from 'zerocomponents/buttons/button.vue'
  import StepUser from './Steps/step-user.vue'
  import StepApplication from './Steps/step-application.vue'
  import StepDatabase from './Steps/step-database.vue'
  import StepInstall from './Steps/step-install.vue'
  import StepFinish from './Steps/step-finish.vue'

  export default {
    name: 'setup',

    components: { UiButton, StepUser, StepApplication, StepDatabase, StepInstall, StepFinish },

    data: () => ({
      step: 3,
      steps: [ 'step-user', 'step-application', 'step-database', 'step-install', 'step-finish' ],
      model: {
        AppName: null,
        Part: null,
        User: {
          Name: null,
          Email: null,
          Password: null
        },
        Database: {
          Url: 'http://localhost:9800',
          Name: null
        }
      }
    }),

    mounted()
    {

    },

    methods: {

      next()
      {
        if (this.step + 1 >= this.steps.length)
        {

        }

        this.step += 1;
      },

      prev()
      {
        this.step -= 1;
      },

      save()
      {

      }

    }
  }

</script>


<style lang="scss">
  .setup
  {
    display: grid;
    grid-template-rows: 1fr auto;
    align-items: stretch;
  }

  .setup-step
  {
    width: 100%;
  }

  .setup-step h2
  {
    display: block;
    text-align: center;
  }

  .setup-step p
  {
    color: var(--color-fg-mid);
    font-size: 0.9rem;
    line-height: 1.6;
    margin: 0 0 2rem;
  }

  .setup-step p b
  {
    color: var(--color-fg);
    font-weight: 600;
  }

  .setup-step .ui-property + .ui-property
  {
    padding-top: 0;
    border-top: none;
  }
</style>