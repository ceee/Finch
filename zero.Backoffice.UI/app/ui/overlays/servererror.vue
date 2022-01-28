<template>
  <div class="app-servererror">
    <h2 class="ui-headline"><ui-icon symbol="fth-alert-circle" size="22" /> <span v-localize="'@errors.http.5xx'"></span></h2>
    <div class="app-servererror-content">
      <p>{{model.message}}</p>
      <span class="-code">code: <b>{{model.code}}</b></span>
      <span class="-code">category: <b>{{model.category}}</b></span>
    </div>
    <div class="app-servererror-buttons">
      <ui-button type="light" label="@ui.close" @click="config.close"></ui-button>
    </div>
  </div>
</template>

<script>
  export default {

    props: {
      model: Object,
      config: Object
    }
  }
</script>

<style lang="scss">
  .app-servererror-buttons
  {
    margin-top: 30px;
  }

  .app-overlay[data-alias="servererror"]
  {
    border-top: 5px solid var(--color-negative) !important;
  }

  .app-servererror
  {
    .ui-icon
    {
      position: relative;
      top: 5px;
      color: var(--color-negative);
      margin-right: 0.5em;
    }

    .ui-headline
    {
      color: var(--color-negative);
      margin-top: -8px;
    }
  }

  .app-servererror-content
  {
    border-radius: 3px;
    padding: var(--padding-s);
    border: 1px dashed var(--color-line-dashed);
    background: var(--color-bg-shade-2);
    color: var(--color-text);
    font-family: monospace;

    .-code
    {
      color: var(--color-text-dim);
      display: block;
      font-size: var(--font-size-s);

      b
      {
        font-weight: 400;
      }
    }

    p
    {
      margin-bottom: var(--padding-xs);
    }

    p, .-code
    {
      white-space: pre-wrap;
      line-height: 1.4;
    }
  }
</style>