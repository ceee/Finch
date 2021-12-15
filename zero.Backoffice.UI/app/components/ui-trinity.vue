<template>
  <div class="ui-trinity">
    <header>
      <slot name="header"></slot>
    </header>
    <content>
      <slot></slot>
    </content>
    <footer>
      <slot name="footer"></slot>
    </footer>
  </div>
</template>


<script>
  export default {
    name: 'uiTrinity'
  }
</script>

<style lang="scss">
  .ui-trinity
  {
    display: grid;
    grid-template-rows: auto 1fr auto;
    height: 100vh; 

    > header
    {
      display: block;
    }

    > footer
    {
      display: flex;
      justify-content: flex-end;
      padding: 20px var(--padding);
      //background: var(--color-overlay-footer);
    }

    > content
    {
      display: block;
      padding: var(--padding);
      height: 100%;
      overflow-y: auto;
    }
  }
</style>