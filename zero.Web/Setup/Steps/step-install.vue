<template>
  <div class="setup-step setup-step-install">
    <div v-if="!errorMessage" class="setup-step-install-inner">
      <div class="setup-step-install-progress"><i :style="{ width: progress + '%' }"></i></div>
      <h2>Installing</h2>
    </div>
    <div v-if="errorMessage" class="setup-step-install-error">
      <div class="setup-step-install-error-main">
        <h2>Oh no!</h2>
        <p>The setup of <b>zero</b> failed with following error(s):</p>
        <pre class="setup-step-install-error-code"><code v-html="errorMessage"></code></pre>
      </div>
      <div class="setup-buttons">
        <ui-button type="outline" :click="onBack" label="Go back" caret="left" caret-position="left" />
        <ui-button label="Retry" :click="install" />
      </div>
    </div>
  </div>
</template>


<script>
  import UiButton from 'zerocomponents/buttons/button.vue'
  import Axios from 'axios'

  export default {
    name: 'setupStepInstall',

    props: {
      value: Object,
      onBack: {
        type: Function,
        required: true
      },
      onNext: {
        type: Function,
        required: true
      }
    },

    components: { UiButton },

    data: () => ({
      errorMessage: null,
      progress: 0
    }),

    mounted()
    {
      this.install();
    },

    methods: {
      install()
      {
        this.errorMessage = null;

        let interval = setInterval(() =>
        {
          if (++this.progress >= 100)
          {
            this.onNext();
            clearInterval(interval);
          }
        }, 30);

        Axios.post('/api/setup/install', this.value)
          .then(response =>
          {
            
          })
          .catch((error) =>
          {
            // The request was made and the server responded with a status code that falls out of the range of 2xx
            if (error.response)
            {
              this.error(error.response.data);
            }
            // The request was made but no response was received
            else if (error.request)
            {
              this.error(error.request);
            }
            else
            {
              this.error(error.message);
            }
          });
      },

      error(error)
      {
        this.errorMessage = error;
      }
    }
  }

</script>


<style lang="scss">
  .setup-step-install
  {
    display: grid;
    grid-template-rows: 1fr auto;
    align-items: center;
    justify-content: stretch;
  }

  .setup-step-install-inner
  {
    text-align: center;

    h2
    {
      margin: 30px 0 0;
    }
  }

  .setup-step-install-progress
  {
    position: relative;
    margin: 0 auto;
    width: 80%;
    height: 12px;
    border-radius: 6px;
    background: var(--color-bg-light);
    overflow: hidden;

    i
    {
      background: var(--color-primary);
      transition: width 0.1s ease;
      display: block;
      height: 100%;
    }
  }

  .setup-step-install-error
  {
    display: grid;
    grid-template-rows: 1fr auto;
    align-items: stretch;
    width: 100%;
    height: 100%;

    h2
    {
      color: var(--color-negative);
    }

    div
    {
      width: 100%;
    }
  }

  .setup-step-install-error-code
  {
    background: var(--color-bg-light);
    padding: 20px;
    font-size: 14px;
    line-height: 20px;
    width: 440px;
    max-height: 50vh;
    overflow: scroll;
    font-family: 'Lucida Console', Consolas, sans-serif;
    margin: 0;
  }

  .setup-step-install-error-main
  {
    display: grid;
    grid-template-rows: auto auto 1fr;
    margin-bottom: 30px;
  }
</style>