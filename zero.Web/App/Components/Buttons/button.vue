<template>
  <button type="button" class="ui-button" :class="buttonClass" @click="tryClick">
    <span v-localize="label"></span>
    <i v-if="caret" class="ui-button-caret" :class="caretClass"></i>
    <i v-if="icon" class="ui-button-icon" :class="icon"></i>
    <span v-if="!isDefaultState" class="ui-button-state">
      <i v-if="state == 'loading'" class="ui-button-progress"></i>
      <i v-if="state == 'success'" class="fth-check"></i>
      <i v-if="state == 'error'" class="fth-x"></i>
    </span>
  </button>
</template>


<script>
  export default {
    name: 'uiButton',

    props: {
      label: {
        type: String,
        required: true
      },
      state: {
        type: String,
        default: 'default'
      },
      type: {
        type: String,
        default: 'action'
      },
      icon: {
        type: String
      },
      caret: String,
      caretPosition: {
        type: String,
        default: 'right'
      },
      disabled: Boolean,
      click: {
        type: Function,
        default: () => { }
      },
      stateDuration: {
        type: Number,
        default: 2000
      }
    },

    data: () => ({
      stateTimeout: null
    }),

    computed: {
      buttonClass()
      {
        let classes = [];
        classes.push('type-' + this.type.split(' ').join(' type-'));
        classes.push('caret-' + this.caretPosition);
        classes.push('state-' + (this.isDefaultState ? 'default' : this.state));

        if (this.icon)
        {
          classes.push('has-icon');
        }

        return classes;
      },
      caretClass()
      {
        return 'fth-chevron-' + this.caret;
      },
      isDefaultState()
      {
        return !this.state || this.state === 'default' || this.state === 'Default';
      }
    },

    watch: {
      state(value)
      {
        clearTimeout(this.stateTimeout);

        if (value === 'error' || value === 'success')
        {
          this.stateTimeout = setTimeout(() =>
          {
            // check if :state property has .sync modifier
            let listeners = this.$options._parentListeners;
            const hasSyncModifier = listeners && listeners['update:state'];

            if (!hasSyncModifier)
            {
              console.warn('ui-button: Add the .sync modifier to the "state" property, as changing the state to "success" or "error" will automatically set it back to "default" after the state duration timeout');
            }

            this.$emit('update:state', 'default');
          }, this.stateDuration);
        }
      }
    },

    mounted ()
    {
      
    },

    methods: {

      tryClick(ev)
      {
        if (this.isDefaultState)
        {
          this.click(ev);
        }
      }

    }
  }
</script>