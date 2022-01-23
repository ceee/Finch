<template>
  <div class="ui-property" :class="{'is-vertical': vertical, 'is-text': isText, 'hide-label': hideLabel, 'is-disabled': disabled, 'is-locked': locked, 'can-unlock': canUnlock }">
    <label v-if="label && !hideLabel" class="ui-property-label" :for="field" v-localize:title="description" :class="{ 'has-description': !!description }">
      <button type="button" v-if="canUnlock" class="ui-property-label-small is-lock" :class="{'is-unlocked': !locked}" :title="locked ? 'Unlock property...' : 'Lock property'" @click="$emit(locked ? 'unlock' : 'lock')">
        <ui-icon :size="13" :symbol="locked ? 'fth-lock' : 'fth-unlock'"></ui-icon>
      </button>
      <span v-localize="label"></span>
      <strong class="ui-property-required" v-if="required">*</strong>
      <slot name="label-after"></slot>
      <small class="ui-property-label-small" v-if="description" :size="14" v-localize:title="description">?</small>
    </label>

    <div class="ui-property-content">
      <slot></slot>
    </div> 
    <ui-error v-if="field" :field="field" />
    <slot name="after"></slot>
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
      vertical: {
        type: Boolean,
        default: true 
      },
      locked: Boolean,
      canUnlock: Boolean,
      isText: Boolean,
      disabled: {
        type: Boolean,
        default: false
      }
    }
  }
</script>

<style lang="scss">
  .ui-property
  {
    position: relative;
    display: grid;
    grid-gap: 12px 40px;
    grid-template-columns: minmax(auto, 1fr) auto;

    &.is-disabled .ui-property-content,
    &.is-locked .ui-property-content
    {
      pointer-events: none;
    }

    &.is-disabled, &.is-locked
    {
      cursor: not-allowed;
    }

    &:not(.is-vertical) > .ui-error
    {
      grid-column: span 2 / auto;
    }
  }

  .ui-property + .ui-split,
  .ui-split + .ui-property,
  .ui-property + .ui-property
  { 
    padding-top: 30px;
    margin-top: 30px;
    border-top: 1px dashed var(--color-line-dashed);

    .is-narrow &
    {
      margin-top: 30px;
      padding-top: 0;
      border-top: none;
    }
  }

  .ui-property.is-vertical
  {
    grid-template-columns: minmax(auto, 1fr);
    grid-gap: 12px;
    flex-direction: column;
    //border-top: none;

    > .ui-property-label
    {
      width: 100%;
      padding-right: 0;
    }

    > .ui-property-label + .ui-property-content
    {
      margin-top: 0; 
    }

    > .ui-property-content
    {
      display: block;
    }
  }

  .ui-property.is-text
  {
    grid-gap: 2px;
  }

  .ui-property.full-width > .ui-property-content,
  .ui-property.hide-label > .ui-property-content,
  .ui-property.is-static > .ui-property-content
  {
    max-width: 100%;
  }

  .ui-property.hide-label > .ui-property-label,
  .ui-property.is-static > .ui-property-label
  {
    display: none;
  }

  .ui-property.is-static,
  .ui-property.is-static + .ui-property
  {
    margin-top: 0 !important;
  }

  .ui-property-label
  {
    display: block;
    color: var(--color-text);
    //flex-basis: 30%;
    font-size: var(--font-size);
    line-height: 1.5;
    font-weight: 700;
    flex-basis: 100%;

    &.has-description:hover
    {
      cursor: help;
    }

    /*.ui-property:focus-within &
    {
      color: var(--color-accent-info);
    }*/
  }

  .ui-property-label-small
  {
    display: inline-block;
    font-size: 11px;
    font-weight: 600;
    position: relative;
    top: -1px;
    left: 10px;
    color: var(--color-text-dim);
    cursor: help;
    width: 16px;
    height: 16px;
    text-align: center;
    line-height: 16px;
    border-radius: 16px;
    background: var(--color-bg-shade-4);

    &:hover
    {
      color: var(--color-text);
    }

    &.is-lock
    {
      left: 0;
      margin-right: 6px;
      background: none;
      top: 2px;
      cursor: pointer;

      &[disabled]
      {
        cursor: not-allowed;
      }

      &.is-unlocked
      {
        color: var(--color-text);
      }
    }
  }

  .ui-property-required
  {
    color: var(--color-required-marker);
    margin-left: 0.2em;
    font-weight: 400;
    pointer-events: none;
    user-select: none;
  }

  .ui-property-content
  {
    flex: 1;
    max-width: 932px;
    font-size: var(--font-size);
    display: flex;
    align-items: center;
  }

  .ui-property-help
  {
    max-width: 932px;
    display: grid;
    grid-template-columns: auto 1fr;
    grid-gap: 10px;
    font-size: var(--font-size-xs);
    color: var(--color-text-dim);
    margin: 5px 0 0;
    line-height: 1.4;
    //letter-spacing: 0.3px;

    &:before
    {
      content: "\e87f";
      font-family: var(--font-icon);
      color: var(--color-primary);
      font-size: var(--font-size-l);
      float: left;
      position: relative;
      top: -3px;
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

  .ui-property.ui-property-parent
  {
    display: grid;
    grid-template-columns: repeat(12, minmax(0, 1fr));
    grid-gap: 0; //var(--padding) var(--padding-m);

    > .ui-property
    {
      grid-column-start: 1;
      grid-column-end: 13;
      margin-left: 0;
      margin-right: 0;
      padding-left: 0;
      padding-right: 0;
    }

    &:empty:first-child + .ui-property, 
    > .ui-property[data-cols] + .ui-property[data-cols]
    {
      margin-top: 0;
      border-top: none;
      padding-top: 0;
    }
  }

  .ui-property.ui-property-parent:empty
  {
    display: none;
  }
</style>