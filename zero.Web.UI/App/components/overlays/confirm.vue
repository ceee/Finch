<template>
  <div class="app-confirm">
    <h2 class="ui-headline" v-localize="config.title"></h2>
    <p v-localize:html="config.text"></p>
    <ui-error ref="error" style="margin-top: 25px;" />
    <div class="app-confirm-buttons">
      <ui-button type="light" :label="config.closeLabel" :disabled="state == 'loading'" @click="config.close"></ui-button>
      <ui-button :type="config.confirmType" :state="state" :label="config.confirmLabel" @click="confirm"></ui-button>
    </div>
  </div>
</template>


<script>
  import Overlay from 'zero/services/overlay.js'

  export default {

    props: {
      model: Object,
      config: Object
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

        this.config.confirm({
          hide()
          {
            instance.config.close();
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
  .app-confirm-buttons
  {
    margin-top: 30px;
  }

  .app-confirm p
  {
    line-height: 1.4;
  }
</style>