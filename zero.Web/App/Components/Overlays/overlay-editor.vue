<template>
  <div class="ui-overlay-editor">
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

  }
</script>

<style lang="scss">
  .ui-overlay-editor
  {
    display: grid;
    grid-template-rows: auto 1fr auto;
    height: 100vh;
    background: var(--color-bg);

    > header
    {
      display: block;
    }

    > footer
    {
      display: flex;
      justify-content: flex-end;
      padding: var(--padding);
      padding-top: 0;
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