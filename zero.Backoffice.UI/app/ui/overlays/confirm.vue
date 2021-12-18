<template>
  <div class="app-confirm">
    <h2 class="ui-headline" v-localize="model.title"></h2>
    <p v-localize:html="model.text"></p>
    <ui-error ref="error" style="margin-top: 25px;" />
    <ui-message v-if="model.warning" type="error" :text="model.warning" style="margin-top: 25px;" />
    <div class="app-confirm-buttons">
      <ui-button :type="model.confirmType" :state="state" :label="model.confirmLabel" @click="confirm"></ui-button>
      <ui-button type="light" :label="model.closeLabel" :disabled="state == 'loading'" @click="config.close(true)"></ui-button>
    </div>
  </div>
</template>


<script>
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
          close()
          {
            instance.config.close(true);
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
              instance.$refs.error.clearErrors();
            }
            else
            {
              instance.$refs.error.setErrors(errors);
            }

            setTimeout(() => instance.state = 'default', 1500);
          },
          success: true
        });
      }
    }
  }
</script>