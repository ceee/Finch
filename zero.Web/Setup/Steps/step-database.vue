<template>
  <div class="setup-step setup-step-database">
    <h2>database configuration</h2>
    <p>Zero uses <b>RavenDB</b> as it’s database. If you want a more sophisticated setup (i.e. cluster) you can override the IDocumentStore service which is registered as a singleton.</p>
    
    <ui-property label="Instance URL" :vertical="true">
      <input v-model="value.Database.Url" type="text" class="ui-input" placeholder="Enter URL to the database" />
    </ui-property>

    <ui-property label="Database name" :vertical="true">
      <input v-model="value.Database.Name" type="text" class="ui-input" placeholder="Name of the database" />
    </ui-property>
  </div>
</template>


<script>
  import UiProperty from 'zerocomponents/forms/property.vue'
  import UiButton from 'zerocomponents/buttons/button.vue'

  export default {
    name: 'setupStepDatabase',

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