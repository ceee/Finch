<template>
  <div class="setup-step setup-step-user">
    <h2>welcome to zero</h2>
    <p>First we need some basic credentials from you to create a login. This login is the solely <b>superadmin</b> which is required to change critical application data.</p>
    
    <ui-property label="Name" :vertical="true">
      <input v-model="value.user.name" type="text" class="ui-input" maxlength="40" placeholder="What's your name? It doesn't have to be your real name" />
    </ui-property>

    <ui-property label="Email" :vertical="true">
      <input v-model="value.user.email" type="text" class="ui-input" placeholder="Your email will be used as your login" />
    </ui-property>

    <ui-property label="Password" :vertical="true">
      <input v-model="value.user.password" type="password" class="ui-input" autocomplete="off" maxlength="1024" placeholder="From 8 characters to a galaxy far, far away" />
    </ui-property>
  </div>
</template>


<script>
  export default {
    name: 'setupStepUser',

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