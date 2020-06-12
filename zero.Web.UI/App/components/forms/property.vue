<template>
  <div class="ui-property" :class="{'is-vertical': vertical, 'is-text': isText, 'hide-label': hideLabel }">
    <label v-if="label" class="ui-property-label" :for="field">
      <span v-localize="label"></span>
      <strong class="ui-property-required" v-if="required">*</strong>
      <small v-if="description" v-localize="description"></small>
    </label>

    <div class="ui-property-content">    
      <slot></slot>
      <ui-error :field="field" />
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

    &.is-disabled
    {
      pointer-events: none;
    }
  }

  .ui-split + .ui-property,
  .ui-property + .ui-property
  {
    padding-top: 25px;
    margin-top: 25px;
    /*border-top: 1px solid var(--color-line);*/
  }

  .ui-property.is-vertical
  {
    flex-direction: column;

    .ui-property-label
    {
      width: 100%;
    }

    .ui-property-content
    {
      margin-top: 5px;
    }
  }

  .ui-property.is-text .ui-property-content
  {
    margin-top: 0;
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
    width: 280px;
    padding-right: 50px;
    margin-bottom: 5px;
    font-size: $font-size;
    line-height: 1.5;
    font-weight: 700;
  }

  .ui-property-label small
  {
    display: block;
    padding-top: 5px;
    font-size: $font-size-s;
    font-weight: 400;
    line-height: 1.5;
    text-decoration: none;
    color: var(--color-fg-light);

    &:empty
    {
      display: none;
    }
  }

  .ui-property-required
  {
    color: var(--color-negative);
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
    font-size: var(--font-size-s);
    color: var(--color-fg-light);
    margin: 15px 0 0;

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
</style>