<template>
  <div class="app setup">
    <step-user v-model="model.User" />
    <ui-button label="Next" />
  </div>
</template>


<script>
  import Vue from 'vue'
  import StepUser from './Steps/step-user.vue'
  import Sass from '../Sass/setup.scss'
  import UiButton from 'zerocomponents/buttons/button.vue'

  export default {
    name: 'setup',

    components: { StepUser, UiButton },

    data: () => ({
      model: {
        AppName: null,
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

    }
  }

</script>