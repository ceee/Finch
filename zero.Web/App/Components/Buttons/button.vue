<template>
  <button :type="buttonType" class="ui-button has-state" :class="buttonClass" :disabled="disabled" @click="tryClick">
    <span class="ui-button-text" v-localize="label"></span>
    <i v-if="caret" class="ui-button-caret" :class="caretClass"></i>
    <i v-if="icon" class="ui-button-icon" :class="icon"></i>
    <span v-if="!isDefaultState" class="ui-button-state">
      <i v-if="stateDisplay == 'loading'" class="ui-button-progress"></i>
      <i v-if="stateDisplay == 'success'" class="fth-check"></i>
      <i v-if="stateDisplay == 'error'" class="fth-x"></i>
    </span>
  </button>
</template>


<script>
  const STATE_DEFAULT = 'default';
  const STATE_LOADING = 'loading';
  const STATE_SUCCESS = 'success';
  const STATE_ERROR = 'error';
  const STATES = [STATE_DEFAULT, STATE_LOADING, STATE_SUCCESS, STATE_ERROR];

  export default {
    name: 'uiButton',

    props: {
      label: {
        type: String,
        required: true
      },
      state: {
        type: String,
        default: STATE_DEFAULT
      },
      submit: {
        type: Boolean,
        default: false
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
      stateDuration: {
        type: Number,
        default: 2000
      },
      addEllipsis: {
        type: Boolean,
        default: false
      }
    },

    data: () => ({
      stateDisplay: null,
      stateTimeout: null
    }),

    created()
    {
      this.stateDisplay = this.state; 
    },

    computed: {
      buttonType()
      {
        return this.submit ? 'submit' : 'button';
      },
      buttonClass()
      {
        let classes = [];
        classes.push('type-' + this.type.split(' ').join(' type-'));
        classes.push('state-' + (this.isDefaultState ? STATE_DEFAULT : this.stateDisplay));

        if (this.caret)
        {
          classes.push('caret-' + this.caretPosition);
        }
        if (this.icon)
        {
          classes.push('has-icon');
        }
        if (this.addEllipsis)
        {
          classes.push('has-ellipsis');
        }

        return classes;
      },
      caretClass()
      {
        return 'fth-chevron-' + this.caret;
      },
      isDefaultState()
      {
        return !this.stateDisplay || this.stateDisplay === STATE_DEFAULT || STATES.indexOf(this.stateDisplay) < 0;
      }
    },

    watch: {
      state(value)
      {
        this.stateDisplay = value;

        clearTimeout(this.stateTimeout);

        if (value && STATES.indexOf(value) < 0)
        {
          console.warn(`ui-button: Supported states are "${STATES.join('", "')}"`);
        }
        if (value === STATE_SUCCESS || value === STATE_ERROR)
        {
          this.stateTimeout = setTimeout(() =>
          {
            //const stateUpdate = 'update:state';

            //// check if :state property has .sync modifier
            //let listeners = this.$options._parentListeners;
            //const hasSyncModifier = listeners && listeners[stateUpdate];

            //if (!hasSyncModifier)
            //{
            //  console.warn(`ui-button: Add the .sync modifier to the "state" property, as changing the state to "${STATE_SUCCESS}" or "${STATE_ERROR}" will automatically set it back to "${STATE_DEFAULT}" after the state duration timeout`);
            //}

            this.stateDisplay = STATE_DEFAULT;
          }, this.stateDuration);
        }
      }
    },

    methods: {

      tryClick(ev)
      {
        if (this.isDefaultState)
        {
          this.$emit('click', ev);
        }
      }

    }
  }
</script>