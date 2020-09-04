<template>
  <div class="app-preview">
    <div class="app-preview-controls">
      <img class="app-preview-logo" src="/Assets/zero-2.png" v-localize:alt="'@zero.name'" />
    </div>
    <div class="app-preview-frame">
      <iframe src="http://localhost:2300"></iframe>
    </div>
  </div>
</template>


<script>
  export default {

    data: () => ({
      path: null
    }),

    mounted()
    {
      window.addEventListener("message", this.receiveMessage, false);

      window.opener.postMessage({
        preview: true,
        loaded: true
      }, window.location.origin);
    },

    methods: {

      receiveMessage(event)
      {
        if (typeof event.data !== 'object' || !event.data.preview)
        {
          return;
        }

        console.info(event.data);
      }

    }

  }
</script>

<style lang="scss">
  .app-preview
  {
    width: 100%;
    height: 100vh;
    display: grid;
    grid-template-columns: auto 1fr;
    background: var(--color-bg);
    color: var(--color-fg);
  }

  .app.is-preview 
  {
    display: grid;
    grid-template-columns: 1fr;
    grid-template-rows: 1fr;

    .app-nav
    {
      display: none;
    }
  }

  .app-preview-controls
  {
    background: var(--color-bg-bright);
    width: 80px;
    color: var(--color-fg);
    height: 100vh;
    box-shadow: 0 0 20px rgba(0,0,0,0.15);
    position: relative;
    z-index: 2;
  }

  .app-preview-logo
  {
    margin: 50px 0 50px -5px;
    max-width: 500px;
    height: 22px;
    transform: rotate(-90deg);
    transform-origin: 50% 50%;
  }

  .app-preview-frame
  {
    width: 100%;
    height: 100vh;
    overflow: hidden;
    background: white;
    position: relative;
    z-index: 1;

    iframe
    {
      border-radius: var(--radius);
      margin: 0;
      border: none;
      width: 100%;
      height: 100%;
      overflow-y: auto;
    }
  }
</style>