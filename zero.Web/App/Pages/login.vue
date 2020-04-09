<template>
  <div class="app-auth">
    auth/login
  </div>
</template>


<script>
  export default {
    name: 'app-auth',

    data: () => ({
      
    }),


    methods: {

    }
  }
</script>


<style lang="scss">
  .app-auth
  {
    grid-column: span 2 / auto;
    align-self: center;
    justify-self: center;

    display: grid;
    grid-template-rows: 1fr auto;
    align-items: stretch;
    max-width: 100%;
    width: 520px;
    background: var(--color-bg-mid);
    border-radius: 3px;
    min-height: 600px;
    position: relative;
    z-index: 2;
    padding: 40px;
  }
</style>