<template>
  <form class="ui-form" @submit.prevent="onSubmit" @change="onChange">
    <slot v-bind="slotProps" />
  </form>
</template>


<script>
  import Overlay from 'zero/services/overlay.js'
  import { isArray as _isArray, filter as _filter, groupBy as _groupBy, each as _each } from 'underscore'

  export default {
    name: 'uiForm',

    props: {
      submit: {
        type: Function,
        default: () => { }
      },
      mapState: {
        type: Boolean,
        default: true
      },
      errorComponents: {
        type: Array,
        default: () => [ 'uiError' ]
      }
    },

    data: () => ({
      dirty: false,
      state: 'default',
      errors: [],
      slotProps: {
        state: null
      }
    }),

    watch: {
      state(val)
      {
        this.slotProps.state = val;
      }
    },

    created()
    {
      this.slotProps.state = this.state;
    },

    methods: {

      // shows a confirmation dialog for dirty forms when the route tries to change
      // it only works when this method is attached to the route component
      beforeRouteLeave(to, from, next) 
      {        
        if (this.dirty)
        {
          Overlay.confirm({
            title: 'You have unsaved changes',
            text: 'Are you sure you want to navigate away from this page?',
            confirmLabel: '@unsavedchanges.confirm',
            closeLabel: '@unsavedchanges.close'
          }).then(
            () => next(false),
            () => next()
          );
        }
        else
        {
          next()
        }
      },

      // handles a promise
      handle(promise)
      {
        this.setState('loading');

        return new Promise((resolve, reject) =>
        {
          promise
            .then(
              response =>
              {
                this.setState('success');
                this.setDirty(false);
                resolve(response);
              },
              errors =>
              {
                this.setState('error');
                this.setErrors(errors);
                reject(errors);
              }
            )
            .catch(exception =>
            {
              this.setState('error');
              console.info('catch', exception);

              // TODO should we throw here, probably show an error overlay
              throw exception;
            });
        });
      },


      // submits the form
      onSubmit(e)
      {
        this.submit(this, e);
      },


      // set the form to dirty when one of the fields changes
      onChange(e)
      {
        this.dirty = true;
      },


      // set dirty status
      setDirty(dirty)
      {
        this.dirty = dirty;
      },


      // set state for this component and all child components which attached it's state from the slot props
      setState(state)
      {
        this.state = state;
      },


      // tries to find matching fields for the given errors and displays them
      setErrors(errors)
      {
        if (typeof errors === 'undefined')
        {
          this.errors = [];
        }
        else if (!_isArray(errors))
        {
          this.errors = [errors];
        }
        else
        {
          this.errors = errors;
        }

        let errorGroups = _groupBy(this.errors, 'property');
        let handledGroups = [];

        let errorComponents = [];

        // find components which can output errors
        let traverseChildren = parent =>
        {
          parent.$children.forEach(component =>
          {
            const isErrorComponent = this.errorComponents.indexOf(component.$options.name) > -1;
            const field = component.field;

            if (isErrorComponent)
            {
              errorComponents.push(component);
            }

            if (isErrorComponent && field)
            {
              let errorGroup = errorGroups[field];

              if (errorGroup)
              {
                handledGroups.push(field);
                component.set(errorGroup);
              }
            }
            else
            {
              traverseChildren(component);
            }
          });
        };

        traverseChildren(this);  


        // fill leftovers
        _each(errorGroups, (group, field) =>
        {
          if (handledGroups.indexOf(field) < 0)
          {
            errorComponents.forEach(component =>
            {
              if (component.catchRemaining)
              {
                component.set(group);
              }
            });
          }
        });


        //_each(_groupBy(this.errors, 'property'), (group, field) =>
        //{
        //  let affectedComponents = _filter(errorComponents, component =>
        //  {
        //    return component.catchAll || component
        //  });
        //});

        //errorComponents.forEach(component =>
        //{
        //  const field = component.field;
        //  const catchAll = component.catchAll;
        //  const catchRemaining = component.catchRemaining;
        //});
      }
    }
  }
</script>