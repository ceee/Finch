<template>
  <div class="setup-step setup-step-application">
    <h2>your application</h2>
    <p>With zero you can manage multiple applications with one installation. Start by adding your first application.</p>
    
    <ui-property label="Application name" :vertical="true">
      <input v-model="value.appName" type="text" class="ui-input" maxlength="40" placeholder="My super awesome app maybe?" />
    </ui-property>
  </div>
</template>


<script>
  export default {
    name: 'setupStepApplication',

    props: {
      value: Object 
    },

    mounted ()
    {
      
    },

    methods: {

    }
  }

</script>