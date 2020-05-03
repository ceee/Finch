<template>
  <div class="ui-error" v-if="visible">
    <ui-message v-for="error in errors" :key="error.id" type="error" :text="error.message" :title="error.field" />
  </div>
</template>


<script>
  import { isArray as _isArray } from 'underscore'
  import Strings from 'zero/services/strings'

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
      set(errors)
      {
        if (!_isArray(errors))
        {
          errors = [errors];
        }

        errors.forEach(error =>
        {
          error.id = Strings.guid();
        });

        this.errors = errors;
      },


      // clear errors and hide
      clear()
      {
        this.errors = [];
      }
    }
  }
</script>