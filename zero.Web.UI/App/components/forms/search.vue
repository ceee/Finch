<template>
  <div class="ui-searchinput">
    <input ref="input" type="search" :value="value" @input="onChange" @keyup.enter="onSubmit" class="ui-input" v-localize:placeholder="placeholder" />
    <slot name="button" v-bind="{ onSubmit: onSubmit }">
      <button type="button" class="ui-searchinput-button" v-localize:title="'@ui.search.button'" @click="onSubmit"><i class="fth-search"></i></button>
    </slot>
  </div>
</template>


<script>
  export default {
    name: 'uiSearch',

    props: {
      value: {
        type: String,
        default: ''
      },
      placeholder: {
        type: String,
        default: '@ui.search.placeholder'
      }
    },

    computed: {
      
    },

    methods: {

      onChange(ev)
      {
        this.$emit('change', ev.target.value);
        this.$emit('input', ev.target.value);
      },

      onSubmit(ev)
      {
        this.$emit('submit', this.$refs.input.value);
      },

      focus()
      {
        this.$refs.input.focus();
      }
    }
  }
</script>

<style lang="scss">
  .ui-searchinput
  {
    position: relative;

    .ui-input
    {
      display: block;
      min-width: 320px;
      padding-right: 40px;
      border: none;
      box-shadow: var(--color-shadow-short);
      background: var(--color-bg-bright);
    }

    .ui-searchinput-button
    {
      position: absolute;
      right: 0;
      top: 0;
      height: 100%;
      width: 40px;
      text-align: center;
      font-size: var(--font-size);
      padding-top: 1px;
    }
  }
</style>