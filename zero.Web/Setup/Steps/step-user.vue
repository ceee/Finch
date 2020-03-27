<template>
  <div class="setup-step setup-step-user">
    <h2>welcome to zero</h2>
    <p>First we need some basic credentials from you to create a login. This login is the solely <b>superadmin</b> which is required to change critical application data.</p>
    
    <ui-property label="Name" :vertical="true">
      <input type="text" class="ui-input" placeholder="What's your name?" />
    </ui-property>

    <ui-property label="Email" :vertical="true">
      <input type="text" class="ui-input" placeholder="Your email will be used as your login" />
    </ui-property>

    <ui-property label="Password" :vertical="true">
      <input type="password" class="ui-input" autocomplete="off" placeholder="As long as you want, but at last 8 characters" />
    </ui-property>
  </div>
</template>


<script>
  import UiProperty from 'zerocomponents/forms/property.vue'
  import UiButton from 'zerocomponents/buttons/button.vue'

  export default {
    name: 'setupStepUser',

    props: {
      value: Object 
    },

    components: { UiProperty, UiButton },

    mounted ()
    {
      
    },

    methods: {

    }
  }

</script>