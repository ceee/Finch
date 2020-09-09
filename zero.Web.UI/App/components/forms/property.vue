<template>
  <div class="ui-property" :class="{'is-vertical': vertical, 'is-text': isText, 'hide-label': hideLabel }">
    <label v-if="label" class="ui-property-label" :for="field">
      <span v-localize="label"></span>
      <strong class="ui-property-required" v-if="required">*</strong>
      <small v-if="description" v-localize="description"></small>
    </label>

    <div class="ui-property-content">    
      <slot></slot>
      <ui-error v-if="field" :field="field" />
    </div>

  </div>
</template>


<script>
  export default {
    name: 'uiProperty',

    props: {
      field: String,
      label: String,
      hideLabel: Boolean,
      description: String,
      required: Boolean,
      vertical: Boolean,
      isText: Boolean
    }
  }
</script>

<style lang="scss">
  @import 'Sass/Core/all';

  .ui-property
  {
    position: relative;
    display: flex;  
    margin: 0 -32px 0;
    padding: 0 32px 0;

    &.is-disabled
    {
      pointer-events: none;
    }
  }

  .ui-split + .ui-property,
  .ui-property + .ui-property
  {
    margin-top: 50px;
    //padding-top: 30px;
    //margin-top: 30px;
    //border-top: 1px solid var(--color-line);
  }

  .ui-property.is-vertical
  {
    flex-direction: column;
    border-top: none;

    .ui-property-label
    {
      width: 100%;
      padding-right: 0;
    }

    .ui-property-content
    {
      margin-top: 5px;
    }
  }

  .ui-property.is-text .ui-property-content
  {
    margin-top: 2px;
  }

  .ui-property.full-width > .ui-property-content,
  .ui-property.hide-label > .ui-property-content
  {
    max-width: 100%;
  }

  .ui-property.hide-label > .ui-property-label
  {
    display: none;
  }

  .ui-property-label
  {
    display: block;
    color: var(--color-text);
    width: 260px;
    padding-right: 60px;
    font-size: var(--font-size);
    line-height: 1.5;
    font-weight: 700;

    /*.ui-property:focus-within &
    {
      color: var(--color-accent-info);
    }*/
  }

  .ui-property-label small
  {
    display: block;
    padding-top: 5px;
    font-size: var(--font-size-xs);
    font-weight: 400;
    line-height: 1.4;
    text-decoration: none;
    color: var(--color-text-dim);
    letter-spacing: 0.3px;

    &:empty
    {
      display: none;
    }
  }

  .ui-property-required
  {
    color: var(--color-text-dim);
    margin-left: 0.2em;
    font-weight: 400;
  }

  .ui-property-content
  {
    flex: 1;
    max-width: 800px;
    font-size: var(--font-size);
  }

  .ui-property-help
  {
    max-width: 800px;
    font-size: var(--font-size-xs);
    color: var(--color-text-dim);
    margin: 15px 0 0;
    letter-spacing: 0.3px;

    &:before
    {
      content: "\e87f";
      @extend %font-icon;
      color: var(--color-primary);
      font-size: var(--font-size-l);
      float: left;
      position: relative;
      top: -1px;
      margin-right: 6px;
    }
  }

  .ui-properties-floating
  {
    .ui-property
    {
      display: inline-flex;
      min-height: 52px;
    }

    .ui-property + .ui-property
    {
      padding-top: 0;
      margin-top: 0;
      border-left: 1px solid var(--color-line);
      padding-left: 50px;
      margin-left: 50px;
    }
  }
</style>