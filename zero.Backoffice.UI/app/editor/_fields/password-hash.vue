<template>
  <div class="ui-password-hash">
    <input value="******************" type="text" class="ui-input" readonly="readonly" disabled="disabled" />
    <ui-button v-if="!disabled" label="change" type="light" @click="open" />
  </div>
</template>


<script>
  //import Overlay from 'zero/helpers/overlay.js';
  //import PasswordChangeOverlay from 'zero/components/overlays/password-change.vue';

  export default {
    props: {
      value: {
        type: String,
        default: null
      },
      disabled: {
        type: Boolean,
        default: false
      },
      placeholder: {
        type: [String, Function],
        default: null
      },
      entity: Object
    },


    methods: {

      open()
      {
        //return Overlay.open({
        //  component: PasswordChangeOverlay
        //}).then(hash =>
        //{
        //  this.$emit('input', hash);
        //});
      }

    }
  }
</script>

<style lang="scss">
  .ui-password-hash
  {
    display: grid;
    grid-template-columns: 1fr auto;
    grid-gap: var(--padding-xxs);
  }
</style>