<template>
  <div class="app-confirm">
    <h2 class="ui-headline" v-localize="overlay.title"></h2>
    <p v-localize:html="overlay.text"></p>
    <ui-error ref="error" style="margin-top: 25px;" />
    <div class="app-confirm-buttons">
      <ui-button type="light" :label="overlay.closeLabel" :disabled="state == 'loading'" @click="overlay.close"></ui-button>
      <ui-button :type="overlay.confirmType" :state="state" :label="overlay.confirmLabel" @click="confirm"></ui-button>
    </div>
  </div>
</template>


<script>
  import Overlay from 'zero/services/overlay.js'

  export default {

    props: {
      model: Object,
      overlay: Object
    },

    data: () => ({
      state: 'default'
    }),

    mounted()
    {
      
    },

    methods: {

      confirm()
      {
        var instance = this;

        this.overlay.confirm({
          hide()
          {
            instance.overlay.close();
          },
          state(state)
          {
            instance.state = state;
          },
          errors(errors)
          {
            instance.state = 'error';

            if (!errors)
            {
              instance.$refs.error.clear();
            }
            else
            {
              instance.$refs.error.set(errors);
            }
          },
          success: true
        });
      }
    }
  }
</script>

<style lang="scss">
  .app-confirm
  {
    width: 400px;
  }

  .app-confirm-buttons
  {
    margin-top: 30px;
  }
</style>