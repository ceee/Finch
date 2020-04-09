<template>
  <div class="app-auth">
    <h1 class="app-auth-headline">zero</h1>
    <ui-form class="app-auth-inner" :submit="onSubmit">
      <div>
        <h2 v-localize="'@login.headline'"></h2>

        <ui-property field="email" label="@login.fields.email" :vertical="true">
          <input v-model="model.email" type="text" class="ui-input" maxlength="120" v-localize:placeholder="'@login.fields.email_placeholder'" />
        </ui-property>

        <ui-property field="password" label="@login.fields.password" :vertical="true">
          <input v-model="model.password" type="password" class="ui-input" maxlength="1024" v-localize:placeholder="'@login.fields.password_placeholder'" />
        </ui-property>
      </div>

      <div class="app-auth-bottom">
        <ui-button :submit="true" label="@login.button" />
        <ui-button type="blank" label="@login.button_forgot" />
      </div>
    </ui-form>
  </div>
</template>


<script>
  import AuthApi from 'zeroservices/auth.js'

  export default {
    name: 'app-login',

    //inject: ['form'],

    data: () => ({
      model: {
        email: null,
        password: null
      }
    }),

    methods: {
      onSubmit(form)
      {
        //console.info(this.form);
        form.handle(AuthApi.login(this.model)).then(result =>
        {
          console.info('logged in', result);
        }, () => { });
      },

      onSuccess(response)
      {
        console.info(response)
      }
    }
  }
</script>


<style lang="scss">
  @import './Sass/Core/all';

  .app-auth
  {
    grid-column: span 2 / auto;
    align-self: stretch;
    justify-self: stretch;
    display: grid;
    grid-template-rows: 1fr auto 1fr;
    align-items: center;
    justify-content: center;

    &:before
    {
      content: 'login';
      @extend %font-headline;
      color: rgba(black, 0.008);
      position: fixed;
      left: -10vw;
      bottom: -30vh;
      font-size: 150vh;
      line-height: 150vh;
      font-weight: bold;
    }
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
    box-shadow: 0 0 60px var(--color-shadow);

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
</style>