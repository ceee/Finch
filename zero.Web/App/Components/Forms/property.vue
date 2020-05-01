<template>
  <div class="ui-property" :class="{'is-vertical': vertical, 'is-text': isText }">
    <label v-if="label" class="ui-property-label" for="">
      <span v-localize="label"></span>
      <strong class="ui-property-required" v-if="required">*</strong>
      <small v-if="description" v-localize="description"></small>
    </label>

    <div class="ui-property-content">    
      <slot></slot>
    </div>

    <ui-error :field="field" />
  </div>
</template>


<script>
  export default {
    name: 'uiProperty',

    props: {
      field: String,
      label: String,
      description: String,
      required: Boolean,
      vertical: Boolean,
      isText: Boolean
    }
  }
</script>

<style lang="scss">
  @import 'Sass/Core/settings';

  .ui-property
  {
    position: relative;
    display: flex;

    & + .ui-property
    {
      padding-top: 25px;
      margin-top: 25px;
      /*border-top: 1px solid var(--color-line);*/
    }

    &.is-disabled
    {
      pointer-events: none;
    }
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

  .ui-property.full-width > .ui-property-content
  {
    max-width: 100%;
  }

  .ui-property-label
  {
    display: block;
    color: var(--color-text);
    width: 240px;
    padding-right: 2rem;
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
  }

  .ui-property-content
  {
    flex: 1;
    max-width: 800px;
    font-size: var(--font-size);
  }
</style>