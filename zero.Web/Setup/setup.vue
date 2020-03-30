<template>
  <div class="app setup">
    <step-database v-model="model.Database" />
    <div class="setup-buttons">
      <ui-button type="outline" label="Go back" caret="left" caret-position="left" />
      <ui-button label="Next" />
    </div>
  </div>
</template>


<script>
  import Vue from 'vue'
  import Sass from '../Sass/setup.scss'
  import UiButton from 'zerocomponents/buttons/button.vue'
  import StepUser from './Steps/step-user.vue'
  import StepApplication from './Steps/step-application.vue'
  import StepDatabase from './Steps/step-database.vue'

  export default {
    name: 'setup',

    components: { UiButton, StepUser, StepApplication, StepDatabase },

    data: () => ({
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

    mounted ()
    {
      
    },

    methods: {

      save()
      {

      }

    }
  }

</script>


<style lang="scss">
  .setup
  {
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    justify-content: space-between;
  }

  .setup-step
  {

  }

  .setup-step h2
  {
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