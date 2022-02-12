<template>
  <div>
    <div class="ui-password-hash" :class="{ 'is-reloading': reloading }">
      <input value="******************" v-if="!changing" type="text" class="ui-input" readonly="readonly" disabled="disabled" />
      <input v-model="password" v-else type="text" readonly="readonly" class="ui-input" @click="$event.target.select()" />
      <ui-button v-if="!disabled" :disabled="reloading" icon="fth-rotate-ccw" v-localize:title="'@changepasswordoverlay.regenerate'" type="light" @click="getRandomPassword" />
    </div>
    <p v-if="changing" class="ui-property-help" v-localize="'@changepasswordoverlay.regenerate_help'"></p>
  </div>
</template>


<script>
  import api from '../modules/users/api';

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


    data: () => ({
      reloading: false,
      password: null,
      changing: false
    }),


    methods: {

      async getRandomPassword()
      {
        this.reloading = true;
        const result = await api.getRandomPassword(32);
        this.password = result.data.password;
        let passedValue = 'raw:' + this.password;
        this.$emit('input', passedValue);
        this.$emit('update:value', passedValue);
        this.changing = true;
        this.reloading = false;
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

    &.is-reloading .ui-icon
    {
      animation: rotating 1s linear infinite reverse;
    }

    @keyframes rotating
    {
      from
      {
        -webkit-transform: rotate(0);
        transform: rotate(0)
      }

      to
      {
        -webkit-transform: rotate(1turn);
        transform: rotate(1turn)
      }
    }

    & + .ui-property-help
    {
      margin: 10px 0 0;
    }
  }
</style>