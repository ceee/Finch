<template>
  <div class="app-auth">
    <i class="fth-home app-auth-font-trigger"></i>
    <h1 class="app-auth-headline">zero</h1>
    <ui-form class="app-auth-inner" v-slot="form" @submit="onSubmit">
      <div>
        <h2 v-localize="'@login.headline'"></h2>

        <ui-error :catch-remaining="true" />
        <ui-message type="warn" v-if="rejectReason" :text="rejectReason" />

        <ui-property field="email" label="@login.fields.email" :vertical="true">
          <input v-model="model.email" type="text" class="ui-input" maxlength="120" v-localize:placeholder="'@login.fields.email_placeholder'" />
        </ui-property>

        <ui-property field="password" label="@login.fields.password" :vertical="true">
          <input v-model="model.password" type="password" class="ui-input" maxlength="1024" v-localize:placeholder="'@login.fields.password_placeholder'" />
        </ui-property>

      </div>

      <div class="app-auth-bottom">
        <ui-button :submit="true" label="@login.button" :state="form.state" />
        <ui-button type="blank" label="@login.button_forgot" />
      </div>
    </ui-form>
  </div>
</template>


<script>
  import AuthApi from 'zero/services/auth.js'

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
    background: var(--color-bg);

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
    border-radius: var(--radius);
    /*border: 1px solid var(--color-line);*/
    position: relative;
    z-index: 2;
    padding: var(--padding);
    color: var(--color-fg);
    /*box-shadow: 0 0 60px var(--color-shadow);*/

    h2
    {
      text-align: center;
      margin-bottom: 3rem;
    }
  }

  .app-auth-headline
  {
    text-align: center;
    font-size: 58px;
    font-weight: 700;
    margin: 0;
    color: var(--color-fg);
  }

  .app-auth .ui-property + .ui-property
  {
    padding-top: 0;
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