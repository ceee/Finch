<template>
  <div class="my-plugin" style="padding: 30px;">
    hi from <b>my-plugin</b>
    <br><br>
    <ui-button type="white" label="Settings" @click="goToSettings" icon="fth-settings" />
  </div>
</template>


<script>
  export default {
    methods: {
      goToSettings()
      {
        this.$router.push({ name: 'MyPluginSettings' });
      }
    }
  }

</script>