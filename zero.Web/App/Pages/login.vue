<template>
  <div class="app-auth">
    <h1 class="app-auth-headline">zero</h1>
    <div class="app-auth-inner">
      <div>
        <h2>hello and welcome</h2>

        <ui-property label="Email" :vertical="true">
          <input type="text" class="ui-input" maxlength="120" placeholder="Enter your email or username" />
        </ui-property>

        <ui-property label="Password" :vertical="true">
          <input type="password" class="ui-input" maxlength="1024" placeholder="Password" />
        </ui-property>
      </div>

      <div class="app-auth-bottom">
        <ui-button label="Login" :state.sync="state" :click="login" />
        <ui-button type="blank" label="Forgot password?" />
      </div>
    </div>
  </div>
</template>


<script>
  export default {
    name: 'app-auth',

    data: () => ({
      state: 'default'
    }),


    methods: {

      login()
      {
        this.state = 'loading';
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
    border-radius: 3px;
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
    margin-top: var(--padding);
  }
</style>