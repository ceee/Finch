<template>
  <div class="ui-property" :class="{'is-vertical': vertical }">
    <label v-if="label" class="ui-property-label" for="">
      <span v-localize="label"></span>
      <strong class="ui-property-required" v-if="required">*</strong>
      <small v-if="description" v-localize="description"></small>
    </label>

    <div class="ui-property-content">    
      <slot></slot>
    </div>
  </div>
</template>


<script>
  export default {
    name: 'uiProperty',

    props: {
      label: String,
      description: String,
      required: Boolean,
      vertical: Boolean
    },

    mounted ()
    {
      
    },

    methods: {

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
  }
</style>