<template>
  <div class="app">
    <h1 class="app-headline">zero</h1>
    <div class="setup">
      <component v-bind:is="steps[step]" v-model="model" :on-back="prev" :on-next="next" />
      <div class="setup-buttons" v-if="step < 3">
        <ui-button v-if="step > 0" type="outline" label="Go back" @click="prev" caret="left" caret-position="left" />
        <ui-button :label="nextLabel" @click="next" />
      </div>
    </div>
  </div>
</template>


<script>
  import Sass from '../Sass/setup.scss'
  import UiButton from 'zero/components/buttons/button.vue'
  import StepUser from './Steps/step-user.vue'
  import StepApplication from './Steps/step-application.vue'
  import StepDatabase from './Steps/step-database.vue'
  import StepInstall from './Steps/step-install.vue'
  import StepFinish from './Steps/step-finish.vue'

  export default {
    name: 'setup',

    components: { UiButton, StepUser, StepApplication, StepDatabase, StepInstall, StepFinish },

    data: () => ({
      step: 0,
      steps: ['step-user', 'step-database', 'step-application', 'step-install', 'step-finish'],
      model: { "appName": "Brothers", "user": { "name": "Tobi", "email": "tobi@brothers.studio", "password": "tobi1TOBI!" }, "database": { "url": "http://localhost:9800", "name": "zero" } }
      //model: {
      //  appName: null,
      //  user: {
      //    name: null,
      //    email: null,
      //    password: null
      //  },
      //  database: {
      //    url: 'http://localhost:9800',
      //    name: null
      //  }
      //}
    }),

    computed: {
      nextLabel()
      {
        return this.step < 2 ? 'Next' : 'Install application';
      }
    },

    mounted()
    {

    },

    methods: {

      next()
      {
        if (this.step + 1 >= this.steps.length)
        {
          return;
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
    max-width: 100%;
    width: 520px;
    background: var(--color-box);
    border-radius: 3px;
    min-height: 600px;
    position: relative;
    z-index: 2;
    padding: 40px;
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