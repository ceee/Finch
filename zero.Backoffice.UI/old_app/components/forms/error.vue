<template>
  <div class="ui-error" v-if="visible">
    <ui-message v-for="error in errors" :key="error.id" type="error" :text="error.message" :title="error.field" />
  </div>
</template>


<script>
  import { isArray as _isArray } from 'underscore'
  import Strings from 'zero/helpers/strings.js'

  export default {
    name: 'uiError',

    props: {
      field: {
        type: String,
        default: ''
      },
      catchRemaining: {
        type: Boolean,
        default: false
      },
      catchAll: {
        type: Boolean,
        default: false
      }
    },

    data: () => ({
      errors: []
    }),

    mounted ()
    {
      
    },

    computed: {
      visible()
      {
        return this.errors && this.errors.length;
      }
    },

    methods: {

      // set and display errors
      setErrors(errors, append)
      {
        if (!errors)
        {
          return this.clearErrors();
        }

        if (!_isArray(errors))
        {
          errors = [errors];
        }

        errors.forEach(error =>
        {
          error.id = Strings.guid();
        });

        if (append)
        {
          errors.forEach(error =>
          {
            this.errors.push(error);
          });
        }
        else
        {
          this.errors = errors;
        }
      },


      // clear errors and hide
      clearErrors()
      {
        this.errors = [];
      }
    }
  }
</script>

<style lang="scss">
  .ui-error + .editor
  {
    margin-top: var(--padding-m);
  }
</style>