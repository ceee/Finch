<template>
  <div class="app-auth">
    <i class="fth-home app-auth-font-trigger"></i>
    <span></span>
    <ui-form class="app-auth-inner" v-slot="form" @submit="onSubmit">
      <div>
        <div class="app-auth-logo">
          <span class="app-nav-logo-circle"></span>
          <img src="/Assets/zero.svg" class="app-auth-image show-light" v-localize:alt="'@zero.name'" />
          <img src="/Assets/zero-dark.svg" class="app-auth-image show-dark" v-localize:alt="'@zero.name'" />
        </div>

        <ui-error :catch-remaining="true" />
        <ui-message type="info" v-if="rejectReason" :text="rejectReason" />

        <ui-property field="email" label="@login.fields.email" :vertical="true">
          <input v-model="model.email" type="text" class="ui-input" maxlength="120" v-localize:placeholder="'@login.fields.email_placeholder'" />
        </ui-property>

        <ui-property field="password" label="@login.fields.password" :vertical="true">
          <input v-model="model.password" type="password" class="ui-input" maxlength="1024" v-localize:placeholder="'@login.fields.password_placeholder'" />
        </ui-property>

      </div>

      <div class="app-auth-bottom">
        <ui-button class="app-auth-confirm" type="accent" :submit="true" label="@login.button" :state="form.state" />
        <ui-button type="blank" label="@login.button_forgot" />
      </div>
    </ui-form>
  </div>
</template>


<script>
  import AuthApi from 'zero/helpers/auth.js';

  export default {
    name: 'app-login',

    data: () => ({
      rejectReason: null,
      model: {
        email: null,
        password: null,
        isPersistent: true
      }
    }),

    created()
    {
      this.rejectReason = AuthApi.rejectReason;
    },

    methods: {
      onSubmit(form)
      {
        this.rejectReason = null;

        form.handle(AuthApi.login(this.model)).then(res =>
        {
          window.location.reload();
          AuthApi.$emit('apprebuild');
        }, errors =>
        {
          console.info('login: error', errors);
        });
      }
    }
  }
</script>


<style lang="scss">
  .app-auth
  {
    grid-column: span 2 / auto;
    align-self: stretch;
    justify-self: stretch;
    display: grid;
    grid-template-rows: 1fr auto 1fr;
    align-items: center;
    justify-content: center;
    background: var(--color-page);

    /*&:before
    {
      content: 'login';
      font-family: var(--font-headline);
      color: rgba(black, 0.008);
      position: fixed;
      left: -10vw;
      bottom: -30vh;
      font-size: 150vh;
      line-height: 150vh;
      font-weight: bold;
    }*/
  }

  .app-auth-font-trigger
  {
    position: absolute;
    pointer-events: none;
    opacity: 0.001;
    bottom: 0;
    right: 0;
  }

  .app-auth-inner
  {
    display: grid;
    grid-template-rows: 1fr auto;
    align-items: stretch;
    max-width: 100%;
    width: 520px;
    background: var(--color-box);
    box-shadow: var(--shadow-short);
    border-radius: var(--radius);
    /*border: 1px solid var(--color-line);*/
    position: relative;
    z-index: 2;
    padding: var(--padding);
    color: var(--color-text);
    /*box-shadow: 0 0 60px var(--color-shadow);*/
  }

  .app-auth-logo
  {
    display: flex;
    align-items: center;
    margin: 0 0 3rem 0;
  }

  .app-auth-image
  {
    height: 15px;
  }

  .app-auth .ui-property + .ui-property
  {
    padding-top: 0;
    margin-top: var(--padding);
    border-top: none;
  }

  .app-auth-bottom
  {
    margin-top: 3rem;
  }

  .app-auth .ui-message
  {
    margin: -16px 0 var(--padding);
  }
</style>